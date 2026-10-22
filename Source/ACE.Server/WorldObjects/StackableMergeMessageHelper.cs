using System;

namespace ACE.Server.WorldObjects
{
    internal static class StackableMergeMessageHelper
    {
        internal static void EnqueueCreateObjectForRemainingSourceStack(
            WorldObject sourceStack,
            bool sourceIsPlayerOwned,
            Action<WorldObject> enqueueCreateObject)
        {
            // A full merge destroys the source but leaves its pre-merge StackSize intact.
            if (!sourceIsPlayerOwned && !sourceStack.IsDestroyed && sourceStack.StackSize > 0)
                enqueueCreateObject(sourceStack);
        }
    }
}
