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

        private const uint spellLevel1 = 0;
        private const uint spellLevel2 = 50;
        private const uint spellLevel3 = 100;
        private const uint spellLevel4 = 150;
        private const uint spellLevel5 = 200;
        private const uint spellLevel6 = 250;
        private const uint spellLevel7 = 300;
        private const uint spellLevel8 = 350;

        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.IntStatsTable | IdentifyResponseFlags.StringStatsTable | IdentifyResponseFlags.SpellBook;

        public Scroll(AceObject aceObject)
            : base(aceObject)
        {
            SpellTable table = SpellTable.ReadFromDat();

            Use = $"Inscribed spell: {table.Spells[SpellId].Name}\n";
            Use += $"{table.Spells[SpellId].Desc}";

            LongDesc = "Use this item to attempt to learn its spell.";

            Power = table.Spells[SpellId].Power;
            School = table.Spells[SpellId].School;

            Burden = 30;

            switch (Power)
            {
                case spellLevel1:
                    Value = 1;
                    break;
                case spellLevel2:
                    Value = 5;
                    break;
                case spellLevel3:
                    Value = 20;
                    break;
                case spellLevel4:
                    Value = 100;
                    break;
                case spellLevel5:
                    Value = 200;
                    break;
                case spellLevel6:
                    Value = 1000;
                    break;
                case spellLevel7:
                    Value = 2000;
                    break;
                case spellLevel8:
                    Value = 60000;
                    break;
            }

            ScrollPropertiesInt = PropertiesInt.Where(x => x.PropertyId == (uint)PropertyInt.Value
                                                          || x.PropertyId == (uint)PropertyInt.EncumbranceVal).ToList();

            if (ScrollPropertiesString == null)
                ScrollPropertiesString = new List<AceObjectPropertiesString>();
            if (ScrollPropertiesSpellId == null)
                ScrollPropertiesSpellId = new List<AceObjectPropertiesSpell>();

            var useString = new AceObjectPropertiesString();
            useString.AceObjectId = Guid.Full;
            useString.PropertyId = (ushort)PropertyString.Use;
            useString.PropertyValue = Use;
            ScrollPropertiesString.Add(useString);

            var longDescString = new AceObjectPropertiesString();
            longDescString.AceObjectId = Guid.Full;
            longDescString.PropertyId = (ushort)PropertyString.LongDesc;
            longDescString.PropertyValue = LongDesc;
            ScrollPropertiesString.Add(longDescString);

            var propSpell = new AceObjectPropertiesSpell();
            propSpell.AceObjectId = Guid.Full;
            propSpell.SpellId = SpellId;
            ScrollPropertiesSpellId.Add(propSpell);
        }

        private List<AceObjectPropertiesInt> ScrollPropertiesInt
        {
            get;
            set;
        }

        private List<AceObjectPropertiesString> ScrollPropertiesString
        {
            get;
            set;
        }

        private List<AceObjectPropertiesSpell> ScrollPropertiesSpellId
        {
            get;
            set;
        }

        private uint SpellId
        {
            get { return (uint)Spell.Value; }
        }

        // Minimum Skill Level for 50% fizzle rate
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
                case spellLevel2: // Level 2
                case spellLevel3: // Level 3
                case spellLevel4: // Level 4
                case spellLevel5: // Level 5
                case spellLevel6: // Level 6
                    if (session.Player.CanReadScroll(School, Power))
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
            WriteIdentifyObjectHeader(writer, idFlags, true); // Always succeed in assessing a scroll.
            WriteIdentifyObjectIntProperties(writer, idFlags, ScrollPropertiesInt);
            WriteIdentifyObjectStringsProperties(writer, idFlags, ScrollPropertiesString);
            WriteIdentifyObjectSpellIdProperties(writer, idFlags, ScrollPropertiesSpellId);
        }
    }
}
