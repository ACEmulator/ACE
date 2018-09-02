using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Scroll : WorldObject
    {
        private static readonly UniversalMotion motionReading = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Reading));
        private static readonly UniversalMotion motionReady = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Ready));

        private const uint spellLevel1 = 0;
        private const uint spellLevel2 = 50;
        private const uint spellLevel3 = 100;
        private const uint spellLevel4 = 150;
        private const uint spellLevel5 = 200;
        private const uint spellLevel6 = 250;
        private const uint spellLevel7 = 300;
        private const uint spellLevel8 = 350;

        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.IntStatsTable | IdentifyResponseFlags.StringStatsTable | IdentifyResponseFlags.SpellBook;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Scroll(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Scroll(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // TODO we shouldn't be auto setting properties that come from our weenie by default

            var table = DatManager.PortalDat.SpellTable;

            Use = $"Inscribed spell: {table.Spells[SpellId].Name}\n";
            Use += $"{table.Spells[SpellId].Desc}";

            LongDesc = "Use this item to attempt to learn its spell.";

            Power = table.Spells[SpellId].Power;
            School = table.Spells[SpellId].School;

            EncumbranceVal = 30;

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

            //ScrollPropertiesInt = GetAllPropertyInt().Where(x => x.Key == PropertyInt.Value
            //                                               || x.Key == PropertyInt.EncumbranceVal)
            //    .ToList();

            /*if (ScrollPropertiesString == null)
                ScrollPropertiesString = new List<AceObjectPropertiesString>();
            if (ScrollPropertiesSpellId == null)
                ScrollPropertiesSpellId = new List<AceObjectPropertiesSpell>();*/

            /*var useString = new AceObjectPropertiesString();
            useString.AceObjectId = Guid.Full;
            useString.PropertyId = (ushort)PropertyString.Use;
            useString.PropertyValue = Use;
            ScrollPropertiesString.Add(useString);*/

            /*var longDescString = new AceObjectPropertiesString();
            longDescString.AceObjectId = Guid.Full;
            longDescString.PropertyId = (ushort)PropertyString.LongDesc;
            longDescString.PropertyValue = LongDesc;
            ScrollPropertiesString.Add(longDescString);*/

            /*var propSpell = new AceObjectPropertiesSpell();
            propSpell.AceObjectId = Guid.Full;
            propSpell.SpellId = SpellId;
            ScrollPropertiesSpellId.Add(propSpell);*/
        }

        /*private List<AceObjectPropertiesInt> ScrollPropertiesInt
        {
            get;
            set;
        }*/

        /*private List<AceObjectPropertiesString> ScrollPropertiesString
        {
            get;
            set;
        }*/

        /*private List<AceObjectPropertiesSpell> ScrollPropertiesSpellId
        {
            get;
            set;
        }*/

        private uint SpellId => (uint)Spell.Value;

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

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the player using the item.<para />
        /// The item should be in the players possession.
        /// </summary>
        public override void UseItem(Player player, ActionChain actionChain)
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
                    if (!player.CanReadScroll(School, Power))
                    {
                        success = false;
                        failReason = "You are not skilled enough in the inscribed spell's school of magic to understand the writing on this scroll.";
                    }
                    break;
            }

            if (player.SpellIsKnown(SpellId))
            {
                success = false;
                failReason = "You already know the spell inscribed upon this scroll.";
            }

            actionChain
                .AddAction(player, () => player.HandleActionMotion(motionReading))
                .AddDelaySeconds(2);

            if (success)
            {
                actionChain.AddAction(player, () =>
                {
                    player.LearnSpellWithNetworking(SpellId);
                    player.HandleActionMotion(motionReady);
                    if (player.TryRemoveFromInventoryWithNetworking(this))
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat("The scroll is destroyed.", ChatMessageType.Magic));
                });
            }
            else
            {
                actionChain
                    .AddDelaySeconds(2)
                    .AddAction(player, () =>
                    {
                        player.HandleActionMotion(motionReady);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{failReason}", ChatMessageType.Magic));
                    });
            }

            actionChain
                .AddAction(player, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session)));
        }

        //public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        //{
        //    WriteIdentifyObjectHeader(writer, idFlags, true); // Always succeed in assessing a scroll.
        //    //WriteIdentifyObjectIntProperties(writer, idFlags, ScrollPropertiesInt);
        //    //WriteIdentifyObjectStringsProperties(writer, idFlags, ScrollPropertiesString);
        //    WriteIdentifyObjectSpellIdProperties(writer, idFlags, ScrollPropertiesSpellId);
        //}
    }
}
