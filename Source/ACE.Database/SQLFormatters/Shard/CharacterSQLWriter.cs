using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;

namespace ACE.Database.SQLFormatters.Shard
{
    public class CharacterSQLWriter : SQLWriter
    {
        /// <summary>
        ///Default is formed from: input.Id.ToString("X8") + " " + name
        /// </summary>
        public string GetDefaultFileName(Character input)
        {
            return input.Id.ToString("X8") + " " + input.Name + ".sql";
        }

        public void CreateSQLDELETEStatement(Character input, StreamWriter writer)
        {
            writer.WriteLine($"DELETE FROM `character` WHERE `id` = {input.Id};");
        }

        public void CreateSQLINSERTStatement(Character input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character` (`id`, `account_Id`, `name`, `is_Plussed`, `is_Deleted`, `delete_Time`, `last_Login_Timestamp`, `total_Logins`, `character_Options_1`, `character_Options_2`, `gameplay_Options`, `spellbook_Filters`, `hair_Texture`, `default_Hair_Texture`)");

            var output = $"VALUES ({input.Id}, {input.AccountId}, {GetSQLString(input.Name)}, {input.IsPlussed}, {input.IsDeleted}, {input.DeleteTime}, {input.LastLoginTimestamp}, {input.TotalLogins}, {input.CharacterOptions1}, {input.CharacterOptions2}, {input.GameplayOptions}, {input.SpellbookFilters}, {input.HairTexture}, {input.DefaultHairTexture});";

            output = FixNullFields(output);

            writer.WriteLine(output);

            if (input.CharacterPropertiesContractRegistry != null && input.CharacterPropertiesContractRegistry.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesContractRegistry.OrderBy(r => r.ContractId).ToList(), writer);
            }

            if (input.CharacterPropertiesFillCompBook != null && input.CharacterPropertiesFillCompBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesFillCompBook.OrderBy(r => r.SpellComponentId).ToList(), writer);
            }

            if (input.CharacterPropertiesFriendList != null && input.CharacterPropertiesFriendList.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesFriendList.OrderBy(r => r.FriendId).ToList(), writer);
            }

            if (input.CharacterPropertiesQuestRegistry != null && input.CharacterPropertiesQuestRegistry.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesQuestRegistry.OrderBy(r => r.QuestName).ToList(), writer);
            }

            if (input.CharacterPropertiesShortcutBar != null && input.CharacterPropertiesShortcutBar.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesShortcutBar.OrderBy(r => r.ShortcutBarIndex).ToList(), writer);
            }

            if (input.CharacterPropertiesSpellBar != null && input.CharacterPropertiesSpellBar.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesSpellBar.OrderBy(r => r.SpellBarNumber).ThenBy(r => r.SpellBarIndex).ToList(), writer);
            }

            if (input.CharacterPropertiesTitleBook != null && input.CharacterPropertiesTitleBook.Count > 0)
            {
                writer.WriteLine();
                CreateSQLINSERTStatement(input.Id, input.CharacterPropertiesTitleBook.OrderBy(r => r.TitleId).ToList(), writer);
            }
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesContractRegistry> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_contract_registry` (`character_Id`, `contract_Id`, `delete_Contract`, `set_As_Display_Contract`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].ContractId}, {input[i].DeleteContract}, {input[i].SetAsDisplayContract})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesFillCompBook> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_fill_comp_book` (`character_Id`, `spell_Component_Id`, `quantity_To_Rebuy`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].SpellComponentId}, {input[i].QuantityToRebuy})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesFriendList> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_friend_list` ( `character_Id`, `friend_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].FriendId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesQuestRegistry> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_quest_registry` (`character_Id`, `quest_Name`, `last_Time_Completed`, `num_Times_Completed`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {GetSQLString(input[i].QuestName)}, {input[i].LastTimeCompleted}, {input[i].NumTimesCompleted})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesShortcutBar> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_shortcut_bar` (`character_Id`, `shortcut_Bar_Index`, `shortcut_Object_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].ShortcutBarIndex}, {input[i].ShortcutObjectId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesSpellBar> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_spell_bar` (`character_Id`, `spell_Bar_Number`, `spell_Bar_Index`, `spell_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].SpellBarNumber}, {input[i].SpellBarIndex}, {input[i].SpellId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }

        public void CreateSQLINSERTStatement(uint characterId, IList<CharacterPropertiesTitleBook> input, StreamWriter writer)
        {
            writer.WriteLine("INSERT INTO `character_properties_title_book` (`character_Id`, `title_Id`)");

            var lineGenerator = new Func<int, string>(i => $"{characterId}, {input[i].TitleId})");

            ValuesWriter(input.Count, lineGenerator, writer);
        }
    }
}
