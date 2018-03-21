using System;
using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class SmartBox
    {
        public int TestMode;
        public Position Viewer;
        public ObjCell ViewerCell;
        public int HeadIndex;
        public Position ViewerSoughtPosition;
        public float ViewerLightIntensity;
        public float ViewerLightFalloff;
        //public CameraManager CameraManager;
        //public CellManager CellManager;
        public PhysicsEngine Physics;
        //public ObjectMaint ObjMaint;
        //public LScape LScape;
        //public Ambient AmbientSounds;
        //public CommandInterpreter CmdInterp;
        public int CreatureMode;
        public float GameFOV;
        public float ViewDistFOV;
        public bool UseViewDistance;
        public float GameAmbientLevel;
        public int GameAmbientColor;
        public int GameDegradesDisabled;
        public int Hidden;
        public int PositionUpdateComplete;
        public int WaitingForTeleport;
        public int HasBeenTeleported;
        //public List<NetBlob> InQueue;
        //public List<NetBlob> NetBlobList;
        //public object PositionAndMovementFile;
        public int PlayerID;
        public PhysicsObj Player;
        public int TargetObjectID;
        public Delegate Callback;
        public int NumCells;
        //public List<EnvCell> Cells;
        public int NumObjects;
        public List<PhysicsObj> Objects;
        //public Delegate RenderingCallback;

        public void PlayerPhysicsUpdatedCallback()
        {
            Viewer = new Position();
            ViewerCell = new ObjCell();
            ViewerSoughtPosition = new Position();
            GameFOV = 1.5707963705062866f;
        }
    }
}
