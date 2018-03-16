using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Motion;
using System;
using System.Collections.Generic;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private static readonly UniversalMotion dead = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

        public void Die()
        {
            ActionChain dieChain = new ActionChain();
            dieChain.AddAction(this, () =>
            {
                CurrentLandblock.EnqueueBroadcastMotion(this, dead);
            });
            dieChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead));
            dieChain.AddAction(this, () =>
            {
                LandblockManager.RemoveObject(this);
                CreateCorpse();
            });
            dieChain.EnqueueChain();
        }

        public uint? Killer
        {
            get => GetProperty(PropertyInstanceId.Killer);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Killer); else SetProperty(PropertyInstanceId.Killer, value.Value); }
        }

        public void Smite(ObjectGuid smiter)
        {
            //SetProperty(PropertyInstanceId.CurrentAttacker, smiter.Full);
            //SetProperty(PropertyInstanceId.CurrentDamager, smiter.Full);
            //Health.Current = 0;
            Killer = smiter.Full;
            Die();
        }

        public bool? NoCorpse
        {
            get => GetProperty(PropertyBool.NoCorpse);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.NoCorpse); else SetProperty(PropertyBool.NoCorpse, value.Value); }
        }

        public void CreateCorpse()
        {
            if (!(NoCorpse ?? false))
            {
                var corpse = WorldObjectFactory.CreateNewWorldObject(DatabaseManager.World.GetCachedWeenie("corpse"));
                
                corpse.SetupTableId = SetupTableId;
                corpse.MotionTableId = MotionTableId;
                corpse.SoundTableId = SoundTableId;
                corpse.PaletteBaseDID = PaletteBaseDID;
                corpse.ClothingBase = ClothingBase;
                corpse.PhysicsTableId = PhysicsTableId;

                if (ObjScale.HasValue)
                    corpse.ObjScale = ObjScale;
                if (PaletteTemplate.HasValue)
                    corpse.PaletteTemplate = PaletteTemplate;
                if (Shade.HasValue)
                    corpse.Shade = Shade;

                //corpse.Location = Location;
                corpse.Location = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationFinalPositionFromStart(Location, ObjScale ?? 1, MotionCommand.Dead);

                corpse.Name = $"Corpse of {Name}";
                corpse.LongDesc = $"Killed by {CurrentLandblock.GetObject(new ObjectGuid(Killer ?? 0)).Name}";

                // Transfer of generated treasure from creature to corpse here

                LandblockManager.AddObject(corpse);
            }
        }
    }
}
