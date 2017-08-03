using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionBookPageData
    {
        [GameAction(GameActionType.BookPageData)]
        public static void Handle(ClientMessage message, Session session)
        {
            var objectId = message.Payload.ReadUInt32();
            var pageNum = message.Payload.ReadUInt32();

            // TODO - GET THESE FROM THE DATABASE
            uint aceObjectId = 2012229680;
            uint pages = 1;
            string authorName = "Training Master";
            string authorAccount = "Password is cheese";
            uint authorID = 0xFFFFFFFF;

            ActionChain chain = new ActionChain();
            session.Player.CurrentLandblock.ChainOnObject(chain, session.Player.Guid, (WorldObject wo) =>
            {
                PageData pageData = new PageData();
                pageData.AuthorID = authorID;
                pageData.AuthorName = authorName;
                pageData.AuthorAccount = authorAccount;
                pageData.PageIdx = 0;
                pageData.PageText = "You can hold down the MOUSE WHEEL BUTTON and drag your mouse to change your view.\n\nOn your NUMERIC KEYPAD, the[Keypad 0] key resets your view, and[Keypad.] key shifts to a first - person view.\n\nThe numeric keypad has many other camera controls -  try them out!Remember to press[Keypad 0] to reset your view.";
                    
                var BookDataResponse = new GameEventBookPageDataResponse(session, aceObjectId, pageData);
                session.Network.EnqueueSend(BookDataResponse);
            });

            // Run on the player
            chain.EnqueueChain();
        }
    }
}