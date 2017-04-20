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
            // wo.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing);
            wo.PhysicsData.CurrentMotionState = new GeneralMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            wo.PhysicsData.MTableResourceId = template.PhysicsData.MTableResourceId; // MotionTableId in db
            wo.PhysicsData.Stable = 536871106; // SoundTableId in DB - constant value according to pcap
            wo.PhysicsData.CSetup = template.PhysicsData.CSetup; // ModelTableId in DB
            wo.PhysicsData.Petable = 872415342; // phstableid in DB - constant value according to pcap
            wo.PhysicsData.ObjScale = template.PhysicsData.ObjScale;

            wo.PhysicsData.PhysicsDescriptionFlag = PhysicsDescriptionFlag.CSetup | PhysicsDescriptionFlag.MTable | PhysicsDescriptionFlag.ObjScale | PhysicsDescriptionFlag.Stable | PhysicsDescriptionFlag.Petable | PhysicsDescriptionFlag.Position | PhysicsDescriptionFlag.Movement; // 104579 - according to pcap
            wo.PhysicsData.PhysicsState = PhysicsState.Ethereal | PhysicsState.IgnoreCollision | PhysicsState.Gravity; // 1044 - according to pcap

            uint tmpIcon = 100667504;
            wo.Icon = tmpIcon;

            wo.GameData.ItemCapacity = 120; // constant value according to pcap
            wo.GameData.ContainerCapacity = 10; // constant value according to pcap
            wo.GameData.Usable = Usable.UsableViewedRemote; // constant value according to pcap
            wo.GameData.UseRadius = 2.0f; // constant value according to pcap
            wo.GameData.Burden = 6000; // Testdata, has to be set as the sum of the spawned items in the corpse

            wo.ModelData.PaletteGuid = template.ModelData.PaletteGuid;
            template.ModelData.GetModels.ForEach(mo => wo.ModelData.AddModel(mo.Index, mo.ModelID));
            template.ModelData.GetTextures.ForEach(mt => wo.ModelData.AddTexture(mt.Index, mt.OldTexture, mt.NewTexture));
            template.ModelData.GetPalettes.ForEach(mp => wo.ModelData.AddPalette(mp.PaletteId, mp.Offset, mp.Length));

            // Calculation of the TTL: 5 real time minutes * player level with a minimum of 1 hour, so we set the minimum here
            wo.DespawnTime = 360 + WorldManager.PortalYearTicks;

            return wo;
        }
    }
}
