
namespace ACE.Network.GameEvent.Events
{
    public class GameEventSetTurbineChatChannels : GameEventMessage
    {
        /// <summary>
        /// unknown1, unknown2 unknown3 do not appear to be used by the latest client.<para />
        /// They are possibly accessed by an admin client. They could be used in the future for admin chat via a decal plugin (with some sort of auth layer). 
        /// </summary>
        public GameEventSetTurbineChatChannels(Session session, uint allegiance, uint general, uint trade, uint lfg, uint rolePlay, uint olhtoi, uint society, uint unknown1, uint unknown2, uint unknown3) : base(GameEventType.SetTurbineChatChannels, session)
        {
            Writer.Write(allegiance);
            Writer.Write(general);
            Writer.Write(trade);
            Writer.Write(lfg);
            Writer.Write(rolePlay);
            Writer.Write(olhtoi);
            Writer.Write(society);
            Writer.Write(unknown1);
            Writer.Write(unknown2);
            Writer.Write(unknown3);
        }
    }
}
