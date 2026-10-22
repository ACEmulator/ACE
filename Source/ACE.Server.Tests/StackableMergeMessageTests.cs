using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Models;
using ACE.Server.WorldObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.Server.Tests
{
    [TestClass]
    public class StackableMergeMessageTests
    {
        [TestMethod]
        public void DoesNotEnqueueCreateObjectForDestroyedForeignSourceStack()
        {
            var sourceStack = CreateStack(5);
            sourceStack.Destroy(false);
            Assert.IsTrue(sourceStack.IsDestroyed);
            Assert.AreEqual(5, sourceStack.StackSize ?? -1);
            var sentObjects = new List<WorldObject>();

            StackableMergeMessageHelper.EnqueueCreateObjectForRemainingSourceStack(
                sourceStack,
                sourceIsPlayerOwned: false,
                sentObjects.Add);

            Assert.AreEqual(0, sentObjects.Count);
        }

        [TestMethod]
        public void EnqueuesCreateObjectForRemainingForeignSourceStack()
        {
            var sourceStack = CreateStack(3);
            var sentObjects = new List<WorldObject>();

            StackableMergeMessageHelper.EnqueueCreateObjectForRemainingSourceStack(
                sourceStack,
                sourceIsPlayerOwned: false,
                sentObjects.Add);

            Assert.AreEqual(1, sentObjects.Count);
            Assert.AreSame(sourceStack, sentObjects[0]);
        }

        private static Stackable CreateStack(int stackSize)
        {
            var stack = new Stackable(new Weenie(), new ObjectGuid(1));
            stack.StackSize = stackSize;
            return stack;
        }
    }
}
