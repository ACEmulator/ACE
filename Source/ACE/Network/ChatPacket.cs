namespace ACE.Network
{
    public static class ChatPacket
    {
        public static void SendSystemMessage(Session session, string message)
        {
            var gm = new GameMessageSystemChat(session, message);

            if (session == null)
            {
                // TODO: broadcast
            }
            else
            {
                session.SendWorldFragmentPacket(gm);
            }
        }
    }
}
