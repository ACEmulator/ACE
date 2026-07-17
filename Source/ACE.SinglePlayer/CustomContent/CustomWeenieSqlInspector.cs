using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ACE.SinglePlayer.CustomContent;

internal sealed record CustomWeenieDefinition(
    string FilePath,
    uint ClassId,
    string ClassName,
    int WeenieType,
    string Sql,
    string Sha256)
{
    public string FileName => Path.GetFileName(FilePath);

    public string TypeName => WeenieType switch
    {
        2 => "Container",
        6 => "Generic",
        7 => "Food",
        9 => "Gem",
        10 => "Creature",
        12 => "Vendor",
        16 => "Melee weapon",
        17 => "Missile weapon",
        18 => "Caster",
        19 => "Clothing",
        20 => "Armor",
        21 => "Scroll",
        22 or 51 => "Stackable",
        35 => "Generator",
        _ => $"Type {WeenieType}"
    };
}

internal sealed record CustomWeenieIssue(string FilePath, string Message);

internal sealed record CustomWeenieInspectionResult(
    IReadOnlyList<CustomWeenieDefinition> Definitions,
    IReadOnlyList<CustomWeenieIssue> Issues);

internal sealed class CustomWeenieSqlInspector
{
    internal const int MaximumFiles = 200;
    internal const long MaximumFileBytes = 5 * 1024 * 1024;
    internal const long MaximumTotalBytes = 25 * 1024 * 1024;

    private static readonly Regex InsertPrefix = new(
        @"\AINSERT\s+INTO\s+`?(?<table>[a-z0-9_]+)`?\s*\((?<columns>[^)]*)\)\s*VALUES\s*(?<values>[\s\S]+)\z",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex DeleteWeenie = new(
        @"\ADELETE\s+FROM\s+`?weenie`?\s+WHERE\s+`?class_Id`?\s*=\s*(?<id>\d+)\s*\z",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex SetParentId = new(
        @"\ASET\s+@parent_id\s*=\s*LAST_INSERT_ID\s*\(\s*\)\s*\z",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex UnsupportedAceForgeContent = new(
        @"\b(?:DELETE\s+FROM|INSERT\s+INTO)\s+`?(?:quest|event|recipe|cook_book|treasure_death)\b",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    private static readonly Regex ExecutableComment = new(@"/\*(?:!\d*|\+)",
        RegexOptions.CultureInvariant);

    private static readonly HashSet<string> AllowedPropertyTables = new(StringComparer.OrdinalIgnoreCase)
    {
        "weenie_properties_anim_part",
        "weenie_properties_attribute",
        "weenie_properties_attribute_2nd",
        "weenie_properties_body_part",
        "weenie_properties_book",
        "weenie_properties_book_page_data",
        "weenie_properties_bool",
        "weenie_properties_create_list",
        "weenie_properties_d_i_d",
        "weenie_properties_emote",
        "weenie_properties_emote_action",
        "weenie_properties_event_filter",
        "weenie_properties_float",
        "weenie_properties_generator",
        "weenie_properties_i_i_d",
        "weenie_properties_int",
        "weenie_properties_int64",
        "weenie_properties_palette",
        "weenie_properties_position",
        "weenie_properties_skill",
        "weenie_properties_spell_book",
        "weenie_properties_string",
        "weenie_properties_texture_map"
    };

    public CustomWeenieInspectionResult InspectFiles(IEnumerable<string> filePaths)
    {
        var paths = filePaths
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(Path.GetFullPath)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (paths.Length > MaximumFiles)
        {
            return new CustomWeenieInspectionResult(Array.Empty<CustomWeenieDefinition>(),
                new[] { new CustomWeenieIssue(string.Empty, $"Select no more than {MaximumFiles} SQL files at once.") });
        }

        var definitions = new List<CustomWeenieDefinition>();
        var issues = new List<CustomWeenieIssue>();
        long totalBytes = 0;
        foreach (var path in paths)
        {
            try
            {
                var file = new FileInfo(path);
                if (!file.Exists)
                    throw new FileNotFoundException("The SQL file no longer exists.", path);
                if (!string.Equals(file.Extension, ".sql", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Only .sql files can be imported here.");
                if (file.Length > MaximumFileBytes)
                    throw new InvalidDataException($"The file is larger than {MaximumFileBytes / 1024 / 1024} MB.");
                totalBytes += file.Length;
                if (totalBytes > MaximumTotalBytes)
                    throw new InvalidDataException($"The selected files exceed the {MaximumTotalBytes / 1024 / 1024} MB import limit.");

                var sql = File.ReadAllText(path);
                var inspected = Inspect(path, sql);
                if (inspected.Definitions.Count == 1)
                    definitions.Add(inspected.Definitions[0]);
                issues.AddRange(inspected.Issues);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidDataException)
            {
                issues.Add(new CustomWeenieIssue(path, ex.Message));
            }
        }

        var duplicateIds = definitions
            .GroupBy(item => item.ClassId)
            .Where(group => group.Count() > 1)
            .ToArray();
        foreach (var duplicate in duplicateIds)
        {
            foreach (var definition in duplicate)
                issues.Add(new CustomWeenieIssue(definition.FilePath,
                    $"WCID {duplicate.Key} is also defined by another selected file; both copies were skipped."));
        }
        if (duplicateIds.Length > 0)
        {
            var duplicateSet = duplicateIds.Select(group => group.Key).ToHashSet();
            definitions.RemoveAll(item => duplicateSet.Contains(item.ClassId));
        }

        return new CustomWeenieInspectionResult(definitions, issues);
    }

    internal CustomWeenieInspectionResult Inspect(string filePath, string sql)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new InvalidDataException("The SQL file is empty.");
            if (ExecutableComment.IsMatch(sql))
                throw new InvalidDataException("Executable SQL comments are not allowed.");

            var cleaned = StripComments(sql);
            var statements = SplitStatements(cleaned);
            if (statements.Count == 0)
                throw new InvalidDataException("The SQL file does not contain any statements.");

            var inserts = new List<ParsedInsert>();
            foreach (var statement in statements.Where(statement =>
                         statement.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase)))
            {
                if (!TryParseInsert(statement, out var parsed, out var error))
                    throw new InvalidDataException(error);
                inserts.Add(parsed!);
            }

            var rootInserts = inserts.Where(insert =>
                string.Equals(insert.Table, "weenie", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (rootInserts.Length != 1)
            {
                if (rootInserts.Length == 0 && UnsupportedAceForgeContent.IsMatch(cleaned))
                    throw new InvalidDataException("This is an AceForge quest, event, recipe, or treasure script. The Custom Weenies section currently imports weenie SQL only.");
                throw new InvalidDataException("Each file must define exactly one weenie record.");
            }

            var root = rootInserts[0];
            if (root.Rows.Count != 1)
                throw new InvalidDataException("The weenie INSERT must contain exactly one row.");
            var classId = ReadUIntColumn(root, root.Rows[0], "class_Id");
            if (classId == 0)
                throw new InvalidDataException("WCID 0 is not valid.");
            var className = ReadStringColumn(root, root.Rows[0], "class_Name");
            var weenieType = ReadIntColumn(root, root.Rows[0], "type");

            var deleteCount = 0;
            var rootInsertIndex = statements.FindIndex(statement => string.Equals(statement, root.Statement, StringComparison.Ordinal));
            var lastParentSetIndex = -1;
            for (var index = 0; index < statements.Count; index++)
            {
                var statement = statements[index];
                var delete = DeleteWeenie.Match(statement);
                if (delete.Success)
                {
                    deleteCount++;
                    if (!uint.TryParse(delete.Groups["id"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var deletedId) ||
                        deletedId != classId)
                        throw new InvalidDataException($"The DELETE statement must target WCID {classId}.");
                    if (index > rootInsertIndex)
                        throw new InvalidDataException("The weenie DELETE must appear before its INSERT.");
                    continue;
                }

                if (SetParentId.IsMatch(statement))
                {
                    lastParentSetIndex = index;
                    continue;
                }

                if (!statement.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("Only DELETE FROM weenie, INSERT INTO weenie property tables, and AceForge's SET @parent_id statement are allowed.");
                if (!TryParseInsert(statement, out var insert, out var parseError))
                    throw new InvalidDataException(parseError);
                ValidateInsert(insert!, classId, index, lastParentSetIndex);
            }

            if (deleteCount != 1)
                throw new InvalidDataException("Each file must contain exactly one DELETE FROM weenie statement.");

            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(sql))).ToLowerInvariant();
            var definition = new CustomWeenieDefinition(filePath, classId, className, weenieType, sql, hash);
            return new CustomWeenieInspectionResult(new[] { definition }, Array.Empty<CustomWeenieIssue>());
        }
        catch (InvalidDataException ex)
        {
            return new CustomWeenieInspectionResult(Array.Empty<CustomWeenieDefinition>(),
                new[] { new CustomWeenieIssue(filePath, ex.Message) });
        }
    }

    private static void ValidateInsert(ParsedInsert insert, uint classId, int statementIndex, int lastParentSetIndex)
    {
        if (string.Equals(insert.Table, "weenie", StringComparison.OrdinalIgnoreCase))
        {
            if (insert.Rows.Count != 1 || ReadUIntColumn(insert, insert.Rows[0], "class_Id") != classId)
                throw new InvalidDataException("The weenie INSERT contains an unexpected WCID.");
            return;
        }

        if (!AllowedPropertyTables.Contains(insert.Table))
            throw new InvalidDataException($"Table '{insert.Table}' is not allowed in a Custom Weenie import.");
        if (insert.Rows.Count == 0)
            throw new InvalidDataException($"The INSERT into '{insert.Table}' contains no rows.");

        if (string.Equals(insert.Table, "weenie_properties_emote_action", StringComparison.OrdinalIgnoreCase))
        {
            var emoteIndex = FindColumn(insert.Columns, "emote_Id");
            if (emoteIndex < 0 || insert.Rows.Any(row => row.Count <= emoteIndex ||
                    !string.Equals(row[emoteIndex].Trim(), "@parent_id", StringComparison.OrdinalIgnoreCase)))
                throw new InvalidDataException("Emote actions must be linked through AceForge's @parent_id value.");
            if (lastParentSetIndex < 0 || lastParentSetIndex > statementIndex)
                throw new InvalidDataException("An emote action is missing its preceding SET @parent_id statement.");
            return;
        }

        var objectIndex = FindColumn(insert.Columns, "object_Id");
        if (objectIndex < 0)
            throw new InvalidDataException($"The INSERT into '{insert.Table}' does not identify its owning weenie.");
        foreach (var row in insert.Rows)
        {
            if (row.Count <= objectIndex ||
                !uint.TryParse(row[objectIndex].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var objectId) ||
                objectId != classId)
                throw new InvalidDataException($"The INSERT into '{insert.Table}' attempts to change a different WCID.");
        }
    }

    private static uint ReadUIntColumn(ParsedInsert insert, IReadOnlyList<string> row, string column)
    {
        var index = FindColumn(insert.Columns, column);
        if (index < 0 || row.Count <= index ||
            !uint.TryParse(row[index].Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            throw new InvalidDataException($"The weenie INSERT has an invalid {column} value.");
        return value;
    }

    private static int ReadIntColumn(ParsedInsert insert, IReadOnlyList<string> row, string column)
    {
        var index = FindColumn(insert.Columns, column);
        if (index < 0 || row.Count <= index ||
            !int.TryParse(row[index].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            throw new InvalidDataException($"The weenie INSERT has an invalid {column} value.");
        return value;
    }

    private static string ReadStringColumn(ParsedInsert insert, IReadOnlyList<string> row, string column)
    {
        var index = FindColumn(insert.Columns, column);
        if (index < 0 || row.Count <= index || !TryUnquote(row[index].Trim(), out var value) || string.IsNullOrWhiteSpace(value))
            throw new InvalidDataException($"The weenie INSERT has an invalid {column} value.");
        return value;
    }

    private static int FindColumn(IReadOnlyList<string> columns, string name)
    {
        for (var index = 0; index < columns.Count; index++)
        {
            if (string.Equals(NormalizeIdentifier(columns[index]), name, StringComparison.OrdinalIgnoreCase))
                return index;
        }
        return -1;
    }

    private static string NormalizeIdentifier(string value) => value.Trim().Trim('`');

    private static bool TryUnquote(string value, out string result)
    {
        result = string.Empty;
        if (value.Length < 2 || value[0] is not ('\'' or '"') || value[^1] != value[0])
            return false;
        var quote = value[0];
        var content = value[1..^1];
        content = content.Replace(new string(quote, 2), quote.ToString(), StringComparison.Ordinal)
            .Replace("\\'", "'", StringComparison.Ordinal)
            .Replace("\\\"", "\"", StringComparison.Ordinal)
            .Replace("\\\\", "\\", StringComparison.Ordinal);
        result = content;
        return true;
    }

    private static bool TryParseInsert(string statement, out ParsedInsert? insert, out string error)
    {
        insert = null;
        error = "The INSERT statement is not in the supported AceForge format.";
        var match = InsertPrefix.Match(statement);
        if (!match.Success)
            return false;

        try
        {
            var columns = SplitCommaSeparated(match.Groups["columns"].Value);
            var rows = ParseValueRows(match.Groups["values"].Value);
            if (columns.Count == 0 || rows.Any(row => row.Count != columns.Count))
            {
                error = "An INSERT row does not match its declared columns.";
                return false;
            }
            insert = new ParsedInsert(statement, match.Groups["table"].Value, columns, rows);
            return true;
        }
        catch (InvalidDataException ex)
        {
            error = ex.Message;
            return false;
        }
    }

    private static IReadOnlyList<IReadOnlyList<string>> ParseValueRows(string valueText)
    {
        var rows = new List<IReadOnlyList<string>>();
        var index = 0;
        while (index < valueText.Length)
        {
            SkipWhitespace(valueText, ref index);
            if (index >= valueText.Length || valueText[index] != '(')
                throw new InvalidDataException("INSERT VALUES must be a comma-separated list of rows.");
            var start = ++index;
            var depth = 1;
            var quote = '\0';
            while (index < valueText.Length && depth > 0)
            {
                var character = valueText[index];
                if (quote != '\0')
                {
                    if (character == '\\' && index + 1 < valueText.Length)
                        index += 2;
                    else if (character == quote)
                    {
                        if (index + 1 < valueText.Length && valueText[index + 1] == quote && quote != '`')
                            index += 2;
                        else
                        {
                            quote = '\0';
                            index++;
                        }
                    }
                    else
                        index++;
                    continue;
                }

                if (character is '\'' or '"' or '`')
                {
                    quote = character;
                    index++;
                }
                else if (character == '(')
                {
                    depth++;
                    index++;
                }
                else if (character == ')')
                {
                    depth--;
                    if (depth == 0)
                        break;
                    index++;
                }
                else
                    index++;
            }
            if (depth != 0)
                throw new InvalidDataException("An INSERT row has an unmatched parenthesis or quote.");
            rows.Add(SplitCommaSeparated(valueText[start..index]));
            index++;
            SkipWhitespace(valueText, ref index);
            if (index >= valueText.Length)
                break;
            if (valueText[index] != ',')
                throw new InvalidDataException("Unsupported text appears after an INSERT row.");
            index++;
        }
        return rows;
    }

    private static IReadOnlyList<string> SplitCommaSeparated(string value)
    {
        var result = new List<string>();
        var start = 0;
        var depth = 0;
        var quote = '\0';
        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            if (quote != '\0')
            {
                if (character == '\\' && index + 1 < value.Length)
                    index++;
                else if (character == quote)
                {
                    if (index + 1 < value.Length && value[index + 1] == quote && quote != '`')
                        index++;
                    else
                        quote = '\0';
                }
                continue;
            }
            if (character is '\'' or '"' or '`')
                quote = character;
            else if (character == '(')
                depth++;
            else if (character == ')')
                depth--;
            else if (character == ',' && depth == 0)
            {
                result.Add(value[start..index].Trim());
                start = index + 1;
            }
        }
        if (quote != '\0' || depth != 0)
            throw new InvalidDataException("A SQL value has an unmatched quote or parenthesis.");
        result.Add(value[start..].Trim());
        return result;
    }

    private static List<string> SplitStatements(string sql)
    {
        var statements = new List<string>();
        var start = 0;
        var quote = '\0';
        for (var index = 0; index < sql.Length; index++)
        {
            var character = sql[index];
            if (quote != '\0')
            {
                if (character == '\\' && index + 1 < sql.Length)
                    index++;
                else if (character == quote)
                {
                    if (index + 1 < sql.Length && sql[index + 1] == quote && quote != '`')
                        index++;
                    else
                        quote = '\0';
                }
                continue;
            }
            if (character is '\'' or '"' or '`')
                quote = character;
            else if (character == ';')
            {
                var statement = sql[start..index].Trim();
                if (statement.Length > 0)
                    statements.Add(statement);
                start = index + 1;
            }
        }
        if (quote != '\0')
            throw new InvalidDataException("The SQL contains an unmatched quote.");
        var tail = sql[start..].Trim();
        if (tail.Length > 0)
            statements.Add(tail);
        return statements;
    }

    private static string StripComments(string sql)
    {
        var output = new StringBuilder(sql.Length);
        var quote = '\0';
        for (var index = 0; index < sql.Length; index++)
        {
            var character = sql[index];
            if (quote != '\0')
            {
                output.Append(character);
                if (character == '\\' && index + 1 < sql.Length)
                    output.Append(sql[++index]);
                else if (character == quote)
                {
                    if (index + 1 < sql.Length && sql[index + 1] == quote && quote != '`')
                        output.Append(sql[++index]);
                    else
                        quote = '\0';
                }
                continue;
            }

            if (character is '\'' or '"' or '`')
            {
                quote = character;
                output.Append(character);
                continue;
            }
            if (character == '/' && index + 1 < sql.Length && sql[index + 1] == '*')
            {
                index += 2;
                while (index + 1 < sql.Length && !(sql[index] == '*' && sql[index + 1] == '/'))
                {
                    if (sql[index] is '\r' or '\n')
                        output.Append(sql[index]);
                    index++;
                }
                if (index + 1 >= sql.Length)
                    throw new InvalidDataException("The SQL contains an unterminated block comment.");
                index++;
                output.Append(' ');
                continue;
            }
            if (character == '#' || (character == '-' && index + 1 < sql.Length && sql[index + 1] == '-' &&
                                     (index + 2 >= sql.Length || char.IsWhiteSpace(sql[index + 2]))))
            {
                while (index < sql.Length && sql[index] is not ('\r' or '\n'))
                    index++;
                if (index < sql.Length)
                    output.Append(sql[index]);
                continue;
            }
            output.Append(character);
        }
        if (quote != '\0')
            throw new InvalidDataException("The SQL contains an unmatched quote.");
        return output.ToString();
    }

    private static void SkipWhitespace(string value, ref int index)
    {
        while (index < value.Length && char.IsWhiteSpace(value[index]))
            index++;
    }

    private sealed record ParsedInsert(
        string Statement,
        string Table,
        IReadOnlyList<string> Columns,
        IReadOnlyList<IReadOnlyList<string>> Rows);
}
