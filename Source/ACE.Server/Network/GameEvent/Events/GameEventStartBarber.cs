using ACE.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventStartBarber : GameEventMessage
    {
        public GameEventStartBarber(Session session)
            : base(GameEventType.StartBarber, GameMessageGroup.Group09, session)
        {
            // These are the motion tables for Empyrean float and not-float (one for each gender). They are hard-coded into the client.
            const uint EmpyreanMaleMotionDID = 0x0900020Eu;
            const uint EmpyreanFemaleMotionDID = 0x0900020Du;

            // We will use this function to get the current player's appearance values.
            AceCharacter characterClone = (AceCharacter)Session.Player.GetAceObject();

            Writer.Write((uint)session.Player.PaletteBaseId); // base palette for character
            Writer.Write(characterClone.HeadObject); // Default Hair Model
            Writer.Write(characterClone.HairTexture); // Hair Texture
            Writer.Write(characterClone.DefaultHairTexture); // Default Hair Texture

            Writer.Write(characterClone.EyesTexture); // Eyes Texture
            Writer.Write(characterClone.DefaultEyesTexture); // Default Eyes Texture

            Writer.Write(characterClone.NoseTexture); // Nose Texture
            Writer.Write(characterClone.DefaultNoseTexture); // Default Nose Texture

            Writer.Write(characterClone.MouthTexture); // Mouth Texture
            Writer.Write(characterClone.DefaultMouthTexture); // Default Mouth Texture

            Writer.Write(characterClone.SkinPalette); // Skin Palette
            Writer.Write(characterClone.HairPalette); // Hair Palette
            Writer.Write(characterClone.EyesPalette); // Eyes Palette

            Writer.Write((uint)characterClone.SetupTableId); // Setup Model

            // Check for Empyrean "Float" option
            if (characterClone.MotionTableId == EmpyreanFemaleMotionDID || characterClone.MotionTableId == EmpyreanMaleMotionDID)
                Writer.Write(0x0000001); // Currently using the "bound/running" animation
            else
                Writer.Write(0x0000000); // Current "default" animation (normal running for most, float for Empyrean)
            
            Writer.Write(0x0u); // Unknown - Client seems to have this hard-coded as 0, so likely was just a TBD for potential future use
        }
    }
}
