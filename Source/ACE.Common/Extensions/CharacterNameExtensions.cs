namespace ACE.Common.Extensions
{
    /// <summary>
    /// This is a Helper Extension to service common String functions for Names
    /// </summary>
    public static class CharacterNameExtensions
    {
        /// <summary>
        /// This is a Player Name Helper that converts a string array into a string with necessary spaces throughout. This is used by server commands.
        /// </summary>
        /// <param name="nameStrings">Array of strings, may contain spaces.</param>
        /// <param name="startingElement">The starting position of the array where the character name begins. This is usually 1 when working with server commands.</param>
        /// <returns>string containing a character or account name</returns>
        public static string StringArrayToCharacterName(string[] nameStrings, int startingElement = 0)
        {
            // Store the first part of the player name.
            string characterName = nameStrings[startingElement];
            // Determine if the name has a space
            if (nameStrings.Length > 2)
            {
                // Build the name
                // characterName should already contain one element, so we start at the next position:
                for (int i = (startingElement + 1); i < nameStrings.Length; i++)
                {
                    characterName = characterName + " " + nameStrings[i];
                }
            }
            return characterName;
        }
    }
}
