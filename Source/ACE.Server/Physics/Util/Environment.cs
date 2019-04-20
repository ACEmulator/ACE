using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.Util
{
    public class Environment
    {
        public EnvCell EnvCell;
        public List<DatLoader.FileTypes.Environment> Environments = new List<DatLoader.FileTypes.Environment>();

        public List<DatLoader.Entity.Polygon> Polygons = new List<DatLoader.Entity.Polygon>();

        public List<int> PolyOffsets = new List<int>();
        public int TotalVertices;

        public BBox BBox;

        public Environment(EnvCell envCell)
        {
            EnvCell = envCell;
            LoadEnv(envCell.EnvironmentId);
        }

        public void LoadEnv(uint envID)
        {
            var env = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.Environment>(envID);

            var cellOffset = 0;
            foreach (var cell in env.Cells.Values)
            {
                foreach (var poly in cell.Polygons.Values)
                {
                    PolyOffsets.Add(cellOffset);
                    poly.LoadVertices(cell.VertexArray);
                    Polygons.Add(poly);
                }
                cellOffset += cell.VertexArray.Vertices.Count;
                TotalVertices += cell.VertexArray.Vertices.Count;
            }
            Environments.Add(env);

            BuildBBox();
        }

        public void BuildBBox()
        {
            var origin = EnvCell.Position.Origin;
            var orientation = EnvCell.Position.Orientation;
            var translate = Matrix4x4.CreateTranslation(origin);
            var rotate = Matrix4x4.CreateFromQuaternion(new Quaternion(orientation.X, orientation.Y, orientation.Z, orientation.W));
            BBox = new BBox(Polygons, rotate * translate);
        }
    }
}
