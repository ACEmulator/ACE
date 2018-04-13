using System;
using System.IO;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class ProjectileSpell : WorldObject
    {
        private WorldObject projectileCaster;
        private WorldObject projectileTarget;
        private DamageType projectileElement;
        private uint spellPower;

        public WorldObject ParentWorldObject { get => projectileCaster; set => projectileCaster = value; }
        public WorldObject TargetWorldObject { get => projectileTarget; set => projectileTarget = value; }
        public DamageType ProjectileElement { get => projectileElement; set => projectileElement = value; }
        public uint SpellPower { get => spellPower; set => spellPower = value; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public ProjectileSpell(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public ProjectileSpell(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            flags |= IdentifyResponseFlags.WeaponProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            WriteIdentifyObjectWeaponsProfile(writer, this, success);
        }

        public override void HeartBeat()
        {
            ActionChain selfDestructChain = new ActionChain();
            selfDestructChain.AddAction(this, () =>
            {
                float effect = Math.Max(0.0f, Math.Min(1.0f, ((spellPower - 1.0f) / 7.0f)));
                CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Explode, effect));
            });
            selfDestructChain.AddDelaySeconds(5.0);
            selfDestructChain.AddAction(this, () => LandblockManager.RemoveObject(this));
            selfDestructChain.EnqueueChain();
            return;
        }
    }
}
