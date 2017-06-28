using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class CorpseObjectFactory
    {
        public static Container CreateCorpse(WorldObject template, Position newPosition)
        {
            ushort wcidCorpse = 21;
            var weenie = WeenieHeaderFlag.ItemCapacity | WeenieHeaderFlag.ContainerCapacity | WeenieHeaderFlag.Usable | WeenieHeaderFlag.UseRadius | WeenieHeaderFlag.Burden;
            var objDesc = ObjectDescriptionFlag.CanOpen | ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable | ObjectDescriptionFlag.Corpse; // = bitfield 8213
            var name = $"Corpse of {template.Name}";
            Container wo = new Container(ObjectType.Container, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), name, wcidCorpse, objDesc, weenie, newPosition);

            // TODO: Find the correct motionstate to create a corpse with. For now only the dead motionstate works 
            // wo.CurrentMotionState = new GeneralMotion(MotionStance.Standing);
            wo.CurrentMotionState = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            wo.MTableResourceId = template.MTableResourceId; // MotionTableId in db
            wo.Stable = 536871106; // SoundTableId in DB - constant value according to pcap
            wo.CSetup = template.CSetup; // ModelTableId in DB
            wo.Petable = 872415342; // phstableid in DB - constant value according to pcap
            wo.ObjScale = template.ObjScale;

            wo.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.ObjScale | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Movement; // 104579 - according to pcap
            wo.PhysicsState = PhysicsState.Ethereal | PhysicsState.IgnoreCollision | PhysicsState.Gravity; // 1044 - according to pcap

            uint tmpIcon = 100667504;
            wo.Icon = tmpIcon;

            wo.ItemCapacity = 120; // constant value according to pcap
            wo.ContainerCapacity = 10; // constant value according to pcap
            wo.Usable = Usable.UsableViewedRemote; // constant value according to pcap
            wo.UseRadius = 2.0f; // constant value according to pcap
            wo.Burden = 6000; // Testdata, has to be set as the sum of the spawned items in the corpse

            wo.PaletteGuid = template.PaletteGuid;
            template.GetModels.ForEach(mo => wo.AddModel(mo.Index, mo.ModelID));
            template.GetTextures.ForEach(mt => wo.AddTexture(mt.Index, mt.OldTexture, mt.NewTexture));
            template.GetPalettes.ForEach(mp => wo.AddPalette(mp.PaletteId, mp.Offset, mp.Length));

            // Calculation of the TTL: 5 real time minutes * player level with a minimum of 1 hour, so we set the minimum here
            wo.DespawnTime = 360 + WorldManager.PortalYearTicks;

            return wo;
        }
    }
}
