using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ACE.Server.Command
{
    /// <summary>
    /// Command handler sanity preserving parameter parsing
    /// </summary>
    public class CommandParameterHelpers
    {
        /// <summary>
        /// Types of parameter values
        /// </summary>
        public enum ACECommandParameterType
        {
            /// <summary>
            /// don't use this one
            /// </summary>
            Invalid,
            /// <summary>
            /// normal coordinates, example: 37.3s,67w
            /// output is of type ACE.Entity.Position
            /// </summary>
            Location,
            /// <summary>
            /// a character name, example: the fat bastard
            /// output is of type ACE.Server.WorldObjects.Player
            /// </summary>
            Player,
            /// <summary>
            /// a number, example: 01231242
            /// output is of type ulong: 1231242
            /// </summary>
            ULong
        }
        /// <summary>
        /// A player supplied parameter
        /// </summary>
        public class ACECommandParameter
        {
            /// <summary>
            /// The type of the parameter value
            /// </summary>
            public ACECommandParameterType Type { get; set; } = ACECommandParameterType.Invalid;
            /// <summary>
            /// The value to use upon missing or invalid supplied parameter
            /// </summary>
            public object DefaultValue { get; set; } = null;
            /// <summary>
            /// If this parameter is required
            /// </summary>
            public bool Required { get; set; } = false;
            /// <summary>
            /// The resultant parsed Value (or the default value)
            /// </summary>
            public object Value { get; set; } = null;
            public Position AsPosition { get { return (Position)Value; } }
            public Player AsPlayer { get { return (Player)Value; } }
            public ulong AsULong { get { return (ulong)Value; } }
            /// <summary>
            /// The parameter either wasn't supplied or was invalid (doesn't parse, player doesn't exist, etc.)
            /// </summary>
            public bool Defaulted { get; set; } = true;
            /// <summary>
            /// The broadcast message to send to the session when the parameter is required and didn't parse or wasn't supplied.
            /// </summary>
            public string ErrorMessage { get; set; } = null;
            /// <summary>
            /// Automatically assigned during GetParameters procedure
            /// </summary>
            public int ParameterNo { get; set; } = -1;
        }
        /// <summary>
        /// Resolve the parameters supplied by the player into usable values.
        /// </summary>
        /// <param name="session">the session of the player who sent the command</param>
        /// <param name="rawParameters">the collection of parameters supplied by the default parameter parser</param>
        /// <param name="parameters">the resolution details for every parameter</param>
        /// <returns>the parameters were successfully resolved or not</returns>
        public static bool ResolveACEParameters(Session session, IEnumerable<string> rawParameters, IEnumerable<ACECommandParameter> parameters)
        {
            var parameterBlob = rawParameters.Count() > 0 ? rawParameters.Aggregate((a, b) => a + " " + b).Trim(new char[] { ' ', ',' }) : string.Empty;
            var acps = parameters.ToList();
            for (int i = acps.Count - 1; i > -1; i--)
            {
                var acp = acps[i];
                acp.ParameterNo = i + 1;
                if (parameterBlob.Length > 0)
                {
                    try
                    {
                        switch (acp.Type)
                        {
                            case ACECommandParameterType.ULong:
                                var match2 = Regex.Match(parameterBlob, @"(\d+)$", RegexOptions.IgnoreCase);
                                if (match2.Success)
                                {
                                    acp.Value = ulong.Parse(match2.Groups[1].Value);
                                    acp.Defaulted = false;
                                    parameterBlob = (match2.Groups[1].Index == 0) ? string.Empty : parameterBlob.Substring(0, match2.Groups[1].Index).Trim(new char[] { ' ', ',' });
                                }
                                break;
                            case ACECommandParameterType.Location:
                                Position position = null;
                                var match = Regex.Match(parameterBlob, @"([\d\.]+[ns])[^\d\.]*([\d\.]+[ew])$", RegexOptions.IgnoreCase);
                                if (match.Success)
                                {
                                    var ns = match.Groups[1].Value;
                                    var ew = match.Groups[2].Value;
                                    if (!TryParsePosition(new string[] { ns, ew }, out string errorMessage, out position))
                                    {
                                        ChatPacket.SendServerMessage(session, errorMessage, ChatMessageType.Broadcast);
                                        return false;
                                    }
                                    else
                                    {
                                        acp.Value = position;
                                        acp.Defaulted = false;
                                        var coordsStartPos = Math.Min(match.Groups[1].Index, match.Groups[2].Index);
                                        parameterBlob = (coordsStartPos == 0) ? string.Empty : parameterBlob.Substring(0, coordsStartPos).Trim(new char[] { ' ', ',' });
                                    }
                                }
                                break;
                            case ACECommandParameterType.Player:
                                if (i != 0) throw new Exception("Player name parameter must be the first parameter, since it can contain spaces.");
                                var targetPlayerSession = WorldManager.FindByPlayerName(parameterBlob);
                                if (targetPlayerSession == null)
                                {
                                    ChatPacket.SendServerMessage(session, $"Unable to find player {parameterBlob}", ChatMessageType.Broadcast);
                                    return false;
                                }
                                else
                                {
                                    acp.Value = targetPlayerSession.Player;
                                    acp.Defaulted = false;
                                }
                                break;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (acp.Defaulted) acp.Value = acp.DefaultValue;
                if (acp.Required && acp.Defaulted)
                {
                    if (!string.IsNullOrWhiteSpace(acp.ErrorMessage))
                        ChatPacket.SendServerMessage(session, acp.ErrorMessage, ChatMessageType.Broadcast);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Try to parse a player supplied coordinate into a position
        /// </summary>
        /// <param name="parameters">parameter array containing 2 contiguous elements: first is north/south, second is east/west</param>
        /// <param name="errorMessage">the problem encountered while trying to parse</param>
        /// <param name="position">the resultant ACE.Entity.Position</param>
        /// <param name="startingElement">the first zero based element index of the 2 contiguous elements in the parameter array</param>
        /// <returns>the parsing was successful or not</returns>
        public static bool TryParsePosition(string[] parameters, out string errorMessage, out Position position, int startingElement = 0)
        {
            errorMessage = string.Empty;
            position = null;
            if (parameters.Length - startingElement - 1 < 1)
            {
                errorMessage = "not enough parameters";
                return false;
            }

            string northSouth = parameters[startingElement].ToLower().Replace(",", "").Trim();
            string eastWest = parameters[startingElement + 1].ToLower().Replace(",", "").Trim();


            if (!northSouth.EndsWith("n") && !northSouth.EndsWith("s"))
            {
                errorMessage = "Missing n or s indicator on first parameter";
                return false;
            }

            if (!eastWest.EndsWith("e") && !eastWest.EndsWith("w"))
            {
                errorMessage = "Missing e or w indicator on second parameter";
                return false;
            }

            if (!float.TryParse(northSouth.Substring(0, northSouth.Length - 1), out var coordNS))
            {
                errorMessage = "North/South coordinate is not a valid number.";
                return false;
            }

            if (!float.TryParse(eastWest.Substring(0, eastWest.Length - 1), out var coordEW))
            {
                errorMessage = "East/West coordinate is not a valid number.";
                return false;
            }

            if (northSouth.EndsWith("s"))
                coordNS *= -1.0f;
            if (eastWest.EndsWith("w"))
                coordEW *= -1.0f;

            try
            {
                position = new Position(coordNS, coordEW);
                var cellLandblock = DatManager.CellDat.ReadFromDat<CellLandblock>(position.Cell >> 16 | 0xFFFF);
                position.PositionZ = cellLandblock.GetZ(position.PositionX, position.PositionY);
            }
            catch (System.Exception)
            {
                errorMessage = $"There was a problem with that location (bad coordinates?).";
                return false;
            }
            return true;
        }
    }
}
