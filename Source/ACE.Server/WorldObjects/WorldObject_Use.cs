using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private double? useTimestamp;
        protected double? UseTimestamp
        {
            get { return useTimestamp; }
            set { useTimestamp = Time.GetUnixTime(); }
        }

        protected double? ResetInterval
        {
            get => GetProperty(PropertyFloat.ResetInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, value.Value); }
        }

        protected bool DefaultLocked { get; set; }

        protected bool DefaultOpen { get; set; }


        /// <summary>
        /// Used to determine how close you need to be to use an item.
        /// </summary>
        public bool IsWithinUseRadiusOf(WorldObject wo)
        {
            /*var matchIndoor = Location.Indoors == wo.Location.Indoors;
            var globalPos = matchIndoor ? Location.ToGlobal() : Location.Pos;
            var targetGlobalPos = matchIndoor ? wo.Location.ToGlobal() : wo.Location.Pos;

            var originDist = Vector3.Distance(globalPos, targetGlobalPos);
            var radSum = PhysicsObj.GetRadius() + wo.PhysicsObj.GetRadius();
            var radDist = originDist - radSum;*/
            var useRadius = wo.UseRadius ?? 0.6f;

            var cylDist = GetCylinderDistance(wo);

            // if (this is Player player)
            //    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"OriginDist: {originDist}, RadDist: {radDist}, MyRadius: {PhysicsObj.GetRadius()}, TargetRadius: {wo.PhysicsObj.GetRadius()}, MyUseRadius: {UseRadius ?? 0}, TargetUseRadius: {wo.UseRadius ?? 0}", ChatMessageType.System));

            return cylDist <= useRadius;
        }

        public float GetCylinderDistance(WorldObject wo)
        {
            return (float)Physics.Common.Position.CylinderDistance(PhysicsObj.GetRadius(), PhysicsObj.GetHeight(), PhysicsObj.Position,
                wo.PhysicsObj.GetRadius(), wo.PhysicsObj.GetHeight(), wo.PhysicsObj.Position);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public virtual void UseItem(Player player)
        {
            // Do Nothing by default

#if DEBUG
            var message = $"Default UseItem reached, this object ({Name}) of type ({WeenieType}) not programmed yet."; 
            player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
            log.Error(message);
#endif

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public virtual void ActOnUse(WorldObject worldObject)
        {
            // Do Nothing by default
            if (worldObject is Player player)
            {
#if DEBUG
                var message = $"Default ActOnUse reached, this object ({Name}) not programmed yet.";
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.System));
                log.Error(message);
#endif
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            }
        }
    }
}
