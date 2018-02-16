using System.Collections.Generic;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Entity
{
    /// <summary>
    /// A static mesh contains all data that is common
    /// to each instance with this model id
    /// </summary>
    public class StaticMesh
    {
        /// <summary>
        /// The unique model identifier
        /// </summary>
        public uint ModelId;

        /// <summary>
        /// A multi-part model
        /// </summary>
        public SetupModel SetupModel;

        /// <summary>
        /// The individual model parts
        /// </summary>
        public List<GfxObj> GfxObjs;

        /// <summary>
        /// The list of polygons for this model
        /// </summary>
        public List<ModelPolygon> Polygons;

        /// <summary>
        /// Constructs a new static mesh from a model id
        /// </summary>
        public StaticMesh(uint modelId)
        {
            ModelId = modelId;

            var modelType = modelId >> 24;

            if (modelType == 0x01)
            {
                LoadModelPart(modelId);
            }
            else if (modelType == 0x02)
            {
                SetupModel = DatManager.PortalDat.ReadFromDat<SetupModel>(modelId);
                foreach (var part in SetupModel.Parts)
                    LoadModelPart(part);
            }
        }

        /// <summary>
        /// Loads an individual piece of a model
        /// </summary>
        public void LoadModelPart(uint modelId)
        {
            if (GfxObjs == null) GfxObjs = new List<GfxObj>();
            if (Polygons == null) Polygons = new List<ModelPolygon>();

            var gfxObj = DatManager.PortalDat.ReadFromDat<GfxObj>(modelId);
            GfxObjs.Add(gfxObj);

            foreach (var poly in gfxObj.Polygons.Values)
            {
                Polygons.Add(new ModelPolygon(poly, gfxObj.VertexArray));
            }
        }
    }
}
