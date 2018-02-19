using System.IO;

using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            bool hideCreatureProfile = NpcLooksLikeObject ?? false;

            if (!hideCreatureProfile)
                flags |= IdentifyResponseFlags.CreatureProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            if (!hideCreatureProfile)
                WriteIdentifyObjectCreatureProfile(writer, this, success);
        }

        protected static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature creature, bool success)
        {
            uint header = 0;

            // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
            if (success)
                header = 8;

            writer.Write(header);
            writer.Write(creature.Health.Current);
            writer.Write(creature.Health.MaxValue);
            if (header == 0)
            {
                for (int i = 0; i < 10; i++)
                    writer.Write(0u);
            }
            else
            {
                // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                writer.Write(creature.Strength.Base);
                writer.Write(creature.Endurance.Base);
                writer.Write(creature.Quickness.Base);
                writer.Write(creature.Coordination.Base);
                writer.Write(creature.Focus.Base);
                writer.Write(creature.Self.Base);
                writer.Write(creature.Stamina.Base);
                writer.Write(creature.Mana.Base);
                writer.Write(creature.Stamina.MaxValue);
                writer.Write(creature.Mana.MaxValue);
                // this only gets sent if the header can be masked with 1
                // Writer.Write(0u);
            }
        }

        public void HandleActionWorldBroadcast(string message, ChatMessageType messageType)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoWorldBroadcast(message, messageType));
            chain.EnqueueChain();
        }

        public void DoWorldBroadcast(string message, ChatMessageType messageType)
        {
            GameMessageSystemChat sysMessage = new GameMessageSystemChat(message, messageType);

            WorldManager.BroadcastToAll(sysMessage);
        }
    }
}
