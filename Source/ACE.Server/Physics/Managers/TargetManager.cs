using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Combat
{
    public class TargetManager
    {
        public PhysicsObj PhysicsObj;
        public TargetInfo TargetInfo;
        public Dictionary<uint, TargettedVoyeurInfo> VoyeurTable;
        public double LastUpdateTime;

        public TargetManager() { }

        public TargetManager(PhysicsObj physObj)
        {
            PhysicsObj = physObj;
        }

        public void SetTarget(uint contextID, uint objectID, float radius, double quantum)
        {
            if (PhysicsObj == null) return;

            ClearTarget();

            if (objectID == 0)
            {
                var failedTargetInfo = new TargetInfo();
                failedTargetInfo.ContextID = contextID;
                failedTargetInfo.Status = TargetStatus.TimedOut;
                PhysicsObj.HandleUpdateTarget(failedTargetInfo);
                return;
            }

            TargetInfo = new TargetInfo(contextID, objectID, radius, quantum);
            var target = PhysicsObj.GetObjectA(objectID);
            if (target != null)
                target.add_voyeur(PhysicsObj.ID, radius, quantum);
        }

        public void SetTargetQuantum(double quantum)
        {
            if (PhysicsObj == null || TargetInfo == null) return;

            TargetInfo.Quantum = quantum;

            var targetObj = PhysicsObj.GetObjectA(TargetInfo.ObjectID);
            if (targetObj == null) return;

            targetObj.add_voyeur(PhysicsObj.ID, TargetInfo.Radius, quantum);
        }

        public void HandleTargetting()
        {
            if (PhysicsObj == null) return;

            if (PhysicsTimer.CurrentTime - LastUpdateTime < 0.5f) return;

            if (TargetInfo != null && TargetInfo.TargetPosition == null)
            {
                //Console.WriteLine("TargetManager.HandleTargetting - null position");
                return;
            }

            if (TargetInfo != null && TargetInfo.Status == TargetStatus.Undefined && TargetInfo.LastUpdateTime + 10.0f < PhysicsTimer.CurrentTime)
            {
                TargetInfo.Status = TargetStatus.TimedOut;
                PhysicsObj.HandleUpdateTarget(new TargetInfo(TargetInfo));  // ref?
            }

            if (VoyeurTable != null)
                foreach (var voyeur in VoyeurTable.Values.ToList())
                    CheckAndUpdateVoyeur(voyeur);

            LastUpdateTime = PhysicsTimer.CurrentTime;
        }

        public void CheckAndUpdateVoyeur(TargettedVoyeurInfo voyeur)
        {
            var newPos = GetInterpolatedPosition(voyeur.Quantum);
            if (newPos != null)
            {
                if (newPos.Distance(voyeur.LastSentPosition) > voyeur.Radius)
                    SendVoyeurUpdate(voyeur, newPos, TargetStatus.OK);
            }
        }

        public Position GetInterpolatedPosition(double quantum)
        {
            if (PhysicsObj == null) return null;

            var pos = new Position(PhysicsObj.Position);
            pos.Frame.Origin += PhysicsObj.get_velocity() * (float)quantum;
            return pos;
        }

        public void ClearTarget()
        {
            if (TargetInfo == null)
                return;

            var targetObj = PhysicsObj.GetObjectA(TargetInfo.ObjectID);
            if (targetObj != null)
                targetObj.remove_voyeur(PhysicsObj.ID);

            if (TargetInfo != null)
                TargetInfo = null;
        }

        public void NotifyVoyeurOfEvent(TargetStatus status)
        {
            if (PhysicsObj == null || VoyeurTable == null) return;

            foreach (var voyeur in VoyeurTable.Values.ToList())
                SendVoyeurUpdate(voyeur, PhysicsObj.Position, status);
        }

        public void ReceiveUpdate(TargetInfo update)
        {
            if (PhysicsObj == null || TargetInfo == null || TargetInfo.ObjectID != update.ObjectID) return;

            TargetInfo = new TargetInfo(update);    // ref?
            TargetInfo.LastUpdateTime = PhysicsTimer.CurrentTime;
            TargetInfo.InterpolatedHeading = PhysicsObj.Position.GetOffset(TargetInfo.InterpolatedPosition);

            if (Vec.NormalizeCheckSmall(ref TargetInfo.InterpolatedHeading))
                TargetInfo.InterpolatedHeading = Vector3.UnitZ;

            PhysicsObj.HandleUpdateTarget(new TargetInfo(TargetInfo));  // ref?

            if (update.Status == TargetStatus.ExitWorld)
                ClearTarget();
        }

        public void AddVoyeur(uint objectID, float radius, double quantum)
        {
            if (VoyeurTable != null)
            {
                VoyeurTable.TryGetValue(objectID, out var existingInfo);
                if (existingInfo != null)
                {
                    existingInfo.Radius = radius;
                    existingInfo.Quantum = quantum;
                    return;
                }
            }
            else
                VoyeurTable = new Dictionary<uint, TargettedVoyeurInfo>();

            var info = new TargettedVoyeurInfo(objectID, radius, quantum);
            VoyeurTable.Add(objectID, info);

            SendVoyeurUpdate(info, PhysicsObj.Position, TargetStatus.OK);
        }

        public void SendVoyeurUpdate(TargettedVoyeurInfo voyeur, Position pos, TargetStatus status)
        {
            voyeur.LastSentPosition = new Position(pos);    // ref?

            var info = new TargetInfo(0, PhysicsObj.ID, voyeur.Radius, voyeur.Quantum);
            info.TargetPosition = PhysicsObj.Position;
            info.InterpolatedPosition = new Position(pos);    // ref?
            info.Velocity = PhysicsObj.get_velocity();
            info.Status = status;

            var voyeurObj = PhysicsObj.GetObjectA(voyeur.ObjectID);
            if (voyeurObj != null)
                voyeurObj.receive_target_update(info);
        }

        public bool RemoveVoyeur(uint objectID)
        {
            if (VoyeurTable == null) return false;

            return VoyeurTable.Remove(objectID);
        }
    }
}
