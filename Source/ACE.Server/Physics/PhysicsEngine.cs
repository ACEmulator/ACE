using System;
using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    [Flags]
    public enum TransientStateFlags
    {
        Contact = 0x1,
        OnWalkable = 0x2,
        Sliding = 0x4,
        WaterContact = 0x8,
        StationaryFall = 0x10,
        StationaryStop = 0x20,
        StationaryStuck = 0x40,
        StationaryComplete = StationaryStuck | StationaryStop | StationaryFall,
        Active = 0x80,
        CheckEthereal = 0x100
    };

    public enum PhysicsTimeStamp
    {
        Position = 0x0,
        Movement = 0x1,
        State = 0x2,
        Vector = 0x3,
        Teleport = 0x4,
        ServerControlledMove = 0x5,
        ForcePosition = 0x6,
        ObjDesc = 0x7,
        Instance = 0x8,
        NumPhysics = 0x9
    };

    public class PhysicsEngine
    {
        public ObjectMaint ObjMaint;
        public SmartBox SmartBox;
        public PhysicsObj Player;
        public List<PhysicsObj> Iter;

        public static PhysicsEngine Instance;
        public bool Server;

        //public static List<PhysicsObj> StaticAnimatingObjects; // This is not used
        public static double LastUpdate;

        static PhysicsEngine()
        {
            //StaticAnimatingObjects = new List<PhysicsObj>();
        }

        public PhysicsEngine(ObjectMaint objMaint, SmartBox smartBox)
        {
            ObjMaint = objMaint;
            SmartBox = smartBox;

            SmartBox.Physics = this;
            Instance = this;
        }

        /*public static void AddStaticAnimatingObject(PhysicsObj obj) // Was used in PhysicsObj.InitDefaults
        {
            StaticAnimatingObjects.Add(obj);
        }*/

        /*public static void RemoveStaticAnimatingObject(PhysicsObj obj) // Was used in PhysicsObj.Destroy
        {
            StaticAnimatingObjects.Remove(obj);
        }*/

        public static bool SetObjectMovement(PhysicsObj obj, object buffer, int size, int movementTimestamp, int serverControlTimestamp, bool autonomous)
        {
            var checkTime = false;
            var lastMoveTime = obj.UpdateTimes[1];
            if (Math.Abs(movementTimestamp - lastMoveTime) > Int16.MaxValue)
                checkTime = movementTimestamp < lastMoveTime;
            else
                checkTime = lastMoveTime < movementTimestamp;
            if (checkTime)
                obj.UpdateTimes[1] = movementTimestamp;
            else
                return false;

            var lastServerTime = obj.UpdateTimes[5];
            if (Math.Abs(serverControlTimestamp - lastServerTime) > Int16.MaxValue)
                checkTime = serverControlTimestamp < lastServerTime;
            else
                checkTime = lastServerTime < serverControlTimestamp;
            if (checkTime)
                obj.UpdateTimes[5] = serverControlTimestamp;
            else
                return false;

            var isPlayer = obj.WeenieObj != null && !obj.WeenieObj.IsPlayer();
            if (!isPlayer || !autonomous)
            {
                obj.LastMoveWasAutonomous = autonomous;
                if (isPlayer) return true;
            }
            return false;
        }

        public void SetPlayer(PhysicsObj player)
        {
            Player = player;
        }

        public void UseTime()
        {
            var deltaTime = PhysicsTimer.CurrentTime - LastUpdate;
            if (deltaTime < 0.0f)
            {
                LastUpdate = PhysicsTimer.CurrentTime;
                return;
            }
            if (deltaTime < PhysicsGlobals.MinQuantum) return;

            foreach (var obj in Iter)
            {
                obj.update_object();
                if (Player.Equals(obj))
                    SmartBox.PlayerPhysicsUpdatedCallback();
            }
            //foreach (var obj in StaticAnimatingObjects)
            //    obj.animate_static_object();

            LastUpdate = PhysicsTimer.CurrentTime;
        }
    }
}
