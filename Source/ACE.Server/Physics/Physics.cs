using System;
using System.Collections.Generic;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    [Flags]
    public enum PhysicsState
    {
        Static = 0x1,
        Unused1 = 0x2,
        Ethereal = 0x4,
        ReportCollisions = 0x8,
        IgnoreCollisions = 0x10,
        NoDraw = 0x20,
        Missile = 0x40,
        Pushable = 0x80,
        AlignPath = 0x100,
        PathClipped = 0x200,
        Gravity = 0x400,
        LightingOn = 0x800,
        ParticleEmitter = 0x1000,
        Unused2 = 0x2000,
        Hidden = 0x4000,
        ScriptedCollision = 0x8000,
        HasPhysicsBSP = 0x10000,
        Inelastic = 0x20000,
        HasDefaultAnim = 0x40000,
        HasDefaultScript = 0x80000,
        Cloaked = 0x100000,
        ReportCollisionsAsEnvironment = 0x200000,
        EdgeSlide = 0x400000,
        Sledding = 0x800000,
        Frozen = 0x1000000
    };

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

    public class Physics
    {
        public ObjectMaint ObjMaint;
        public SmartBox SmartBox;
        public PhysicsObj Player;
        public List<PhysicsObj> Iter;

        public static List<PhysicsObj> StaticAnimatingObjects;
        public static double LastUpdate;

        static Physics()
        {
            StaticAnimatingObjects = new List<PhysicsObj>();
        }

        public Physics(ObjectMaint objMaint, SmartBox smartBox)
        {
            ObjMaint = objMaint;
            SmartBox = smartBox;

            PhysicsTimer.CurrentTime = Timer.CurrentTime;

            SmartBox.Physics = this;
        }

        public static void AddStaticAnimatingObject(PhysicsObj obj)
        {
            StaticAnimatingObjects.Add(obj);
        }

        public static void RemoveStaticAnimatingObject(PhysicsObj obj)
        {
            StaticAnimatingObjects.Remove(obj);
        }

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
            var deltaTime = Timer.CurrentTime - LastUpdate;
            if (deltaTime < 0.0f)
            {
                LastUpdate = Timer.CurrentTime;
                return;
            }
            if (deltaTime < PhysicsGlobals.MinQuantum) return;

            foreach (var obj in Iter)
            {
                obj.update_object();
                if (Player.Equals(obj))
                    SmartBox.PlayerPhysicsUpdatedCallback();
            }
            foreach (var obj in StaticAnimatingObjects)
                obj.animate_static_object();

            LastUpdate = Timer.CurrentTime;
        }
    }
}
