using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class CorpseObjectFactory
    {
        public static ImmutableWorldObject CreateCorpse(WorldObject template, Position newPosition)
        {
            ushort wcidCorpse = 21;
            var weenie = WeenieHeaderFlag.Usable | WeenieHeaderFlag.Container | WeenieHeaderFlag.UseRadius;
            var objDesc = ObjectDescriptionFlag.Corpse | ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.CanOpen; // = bitfield 8132 
            var name = $"Corpse of {template.Name}";
            ImmutableWorldObject wo = new ImmutableWorldObject(ObjectType.Container, new ObjectGuid(CommonObjectFactory.DynamicObjectId, GuidType.None), name, wcidCorpse, objDesc, weenie, newPosition);

            wo.PhysicsData.MTableResourceId = template.PhysicsData.MTableResourceId; // aco.MotionTableId - according to pcap
            wo.PhysicsData.Stable = 536871106; // aco.SoundTableId - according to pcap
            wo.PhysicsData.CSetup = template.PhysicsData.CSetup; // aco.ModelTableId - according to pcap
            wo.PhysicsData.Petable = 872415342; // phstableid - according to pcap
            wo.PhysicsData.ObjScale = 1.0f;
            wo.PhysicsData.ObjScale = template.PhysicsData.ObjScale; // aco.ObjectScale - according to pcap

            // this should probably be determined based on the presence of data.
            wo.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)104579; // aco.PhysicsBitField - according to pcap
            wo.PhysicsData.PhysicsState = (PhysicsState)1044; // aco.PhysicsState - according to pcap

            uint tmpIcon = 100667504;
            wo.Icon = (ushort)tmpIcon; // aco.IconId - according to pcap

            wo.GameData.ItemCapacity = 120; // according to pcap
            wo.GameData.ContainerCapacity = 10; // according to pcap
            wo.GameData.Usable = Usable.UsableViewedRemote; // aco.Usability - according to pcap
            wo.GameData.UseRadius = 2.0f; // aco.UseRadius - according to pcap

            wo.GameData.Burden = 1000; // Testdata, has to be set as the sum of the spawned items in the corpse

            // is this needed?
            wo.GameData.RadarColour = RadarColor.Default; // 8 aco.BlipColor - check
            wo.GameData.RadarBehavior = RadarBehavior.NeverShow; // 1 co.Radar - check

            template.ModelData.GetModels.ForEach(mo => wo.ModelData.AddModel(mo.Index, mo.ModelID));
            template.ModelData.GetTextures.ForEach(mt => wo.ModelData.AddTexture(mt.Index, mt.OldTexture, mt.NewTexture));
            template.ModelData.GetPalettes.ForEach(mp => wo.ModelData.AddPalette(mp.PaletteId, mp.Offset, mp.Length));
            wo.ModelData.PaletteGuid = template.ModelData.PaletteGuid; // according to pcap

            return wo;
        }
    }
}
