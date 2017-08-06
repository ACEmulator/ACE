// WeenieType.Scroll

using ACE.DatLoader.FileTypes;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ACE.Entity
{
    public class Scroll : WorldObject
    {
        private static readonly UniversalMotion motionReading = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Reading));
        private static readonly UniversalMotion motionReady = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Ready));

        public Scroll(AceObject aceObject)
            : base(aceObject)
        {
            SpellTable table = SpellTable.ReadFromDat();

            Use = $"Inscribed spell: {table.Spells[SpellId].Name}\n";
            Use += $"{table.Spells[SpellId].Desc}";

            LongDesc = "Use this item to attempt to learn its spell.";

            Power = table.Spells[SpellId].Power;
            School = table.Spells[SpellId].School;
        }

        private uint SpellId
        {
            get { return (uint)Spell.Value; }
        }

        private uint Power
        {
            get;
            set;
        }

        private MagicSchool School
        {
            get;
            set;
        }

        public override void OnUse(Session session)
        {
            bool success = true;
            string failReason = "You are unable to read the scroll.";

            switch (Power)
            {
                // research: http://asheron.wikia.com/wiki/Announcements_-_2002/06_-_Castling
                case 50:  // Level 2
                case 100: // Level 3
                case 150: // Level 4
                case 200: // Level 5
                case 250: // Level 6
                    if (session.Player.CheckMagicSkillLevel(School, Power))
                        success = true;
                    else
                    {
                        success = false;
                        failReason = "You are not skilled enough in the inscribed spell's school of magic to understand the writing on this scroll.";
                    }
                    break;
                default: // Level 1 or Level 7+ never fail
                    success = true;
                    break;
            }

            if (!session.Player.UnknownSpell(SpellId))
            {
                success = false;
                failReason = "You already know the spell inscribed upon this scroll.";
            }

            ActionChain readScrollChain = new ActionChain();
            readScrollChain.AddAction(session.Player, () => session.Player.HandleActionMotion(motionReading));
            readScrollChain.AddDelaySeconds(2);

            if (success)
            {
                readScrollChain.AddAction(session.Player, () => session.Player.HandleActionLearnSpell(SpellId));
                readScrollChain.AddAction(session.Player, () => session.Player.HandleActionMotion(motionReady));
                var removeObjMessage = new GameMessageRemoveObject(this);
                var destroyMessage = new GameMessageSystemChat("The scroll is destroyed.", ChatMessageType.Magic);
                readScrollChain.AddAction(session.Player, () => session.Network.EnqueueSend(destroyMessage, removeObjMessage));
                readScrollChain.AddAction(session.Player, () => session.Player.RemoveFromInventory(Guid));
            }
            else
            {
                readScrollChain.AddDelaySeconds(2);
                readScrollChain.AddAction(session.Player, () => session.Player.HandleActionMotion(motionReady));
                var failMessage = new GameMessageSystemChat($"{failReason}", ChatMessageType.Magic);
                readScrollChain.AddAction(session.Player, () => session.Network.EnqueueSend(failMessage));
            }
            var sendUseDoneEvent = new GameEventUseDone(session.Player.Session);
            readScrollChain.AddAction(session.Player, () => session.Network.EnqueueSend(sendUseDoneEvent));
            readScrollChain.EnqueueChain();
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {          
            var propertiesInt = PropertiesInt.Where(x => x.PropertyId == (uint)PropertyInt.Value
                                                          || x.PropertyId == (uint)PropertyInt.EncumbranceVal).ToList();

            if (propertiesInt.Count > 0)
            {
                flags |= IdentifyResponseFlags.IntStatsTable;
            }

            var propertiesString = new List<AceObjectPropertiesString>();

            var useString = new AceObjectPropertiesString();            
            useString.AceObjectId = Guid.Full;
            useString.PropertyId = (ushort)PropertyString.Use;
            useString.PropertyValue = Use;
            propertiesString.Add(useString);

            var longDescString = new AceObjectPropertiesString();
            longDescString.AceObjectId = Guid.Full;
            longDescString.PropertyId = (ushort)PropertyString.LongDesc;
            longDescString.PropertyValue = LongDesc;
            propertiesString.Add(longDescString);

            var propertiesSpellId = new List<AceObjectPropertiesSpell>();

            var propSpell = new AceObjectPropertiesSpell();
            propSpell.AceObjectId = Guid.Full;
            propSpell.SpellId = SpellId;
            propertiesSpellId.Add(propSpell);

            if (propertiesSpellId.Count > 0)
            {
                flags |= IdentifyResponseFlags.SpellBook;
            }

            if (propertiesString.Count > 0)
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
            }

            WriteIdentifyObjectHeader(writer, flags, success);
            WriteIdentifyObjectIntProperties(writer, flags, propertiesInt);
            WriteIdentifyObjectStringsProperties(writer, flags, propertiesString);
            WriteIdentifyObjectSpellIdProperties(writer, flags, propertiesSpellId);
        }
    }
}
