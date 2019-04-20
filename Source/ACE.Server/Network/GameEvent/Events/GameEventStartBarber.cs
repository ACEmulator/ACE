namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventStartBarber : GameEventMessage
    {
        public static readonly uint EmpyreanMaleMotionDID   = 0x0900020E;
        public static readonly uint EmpyreanFemaleMotionDID = 0x0900020D;

        public GameEventStartBarber(Session session)
            : base(GameEventType.StartBarber, GameMessageGroup.UIQueue, session)
        {
            var player = Session.Player;

            Writer.Write(player.PaletteBaseDID ?? 0);
            Writer.Write(player.HeadObjectDID ?? 0);
            Writer.Write(player.Character.HairTexture);
            Writer.Write(player.Character.DefaultHairTexture);

            Writer.Write(player.EyesTextureDID ?? 0);
            Writer.Write(player.DefaultEyesTextureDID ?? 0);

            Writer.Write(player.NoseTextureDID ?? 0);
            Writer.Write(player.DefaultNoseTextureDID ?? 0);

            Writer.Write(player.MouthTextureDID ?? 0);
            Writer.Write(player.DefaultMouthTextureDID ?? 0);

            Writer.Write(player.SkinPaletteDID ?? 0);
            Writer.Write(player.HairPaletteDID ?? 0);
            Writer.Write(player.EyesPaletteDID ?? 0);

            Writer.Write(player.SetupTableId);

            // option1 - specifies the toggle option for some races, such as floating empyrean or flaming head on undead
            // 0 = using the "default" animation (normal running for most, float for empyrean)
            // 1 = using the "bound/running" animation for empyrean
            var option1 = player.MotionTableId == EmpyreanFemaleMotionDID || player.MotionTableId == EmpyreanMaleMotionDID ? 1 : 0;
            Writer.Write(option1);

            // option2 - seems to be unused
            Writer.Write(0);
        }
    }
}
