using System;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public override float GetRotateDelay(float angle)
        {
            // calculate time to complete the rotation
            // TODO: account for walking speed (shift key)
            var rotateTime = Math.PI / (360.0f / angle);
            return (float)rotateTime;
        }
    }
}
