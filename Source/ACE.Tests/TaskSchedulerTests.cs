using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Entity.Actions;

namespace ACE.Tests
{
    [TestClass]
    public class TaskSchedulerTests
    {
        string res1 = "Hello World";
        string res2 = "Goodbye World";
        bool here1 = false;

        private static readonly ACETaskScheduler scheduler = new ACETaskScheduler();

        private static readonly TaskFactory myTaskFactory = new TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.AttachedToParent,
            TaskContinuationOptions.AttachedToParent, scheduler);
        
        public async Task<string> Coroutine2(string input, double waitSecs = 0)
        {
            await Task.Delay((int)(waitSecs * 1000));

            return input;
        }

        public async Task<string> Coroutine1()
        {
            string s1 = await Coroutine2(res1);
            Assert.AreEqual(res1, s1);

            string s2 = await Coroutine2(res2);
            Assert.AreEqual(res2, s2);

            here1 = true;
            return s2;
        }

        [TestMethod]
        public void TestTaskScheduler()
        {
            Task<string> runMe = myTaskFactory.StartNew(Coroutine1).Unwrap();

            while (!runMe.IsCompleted)
            {
                scheduler.DoWork();
            }

            if (runMe.IsFaulted)
            {
                throw runMe.Exception;
            }

            Assert.IsTrue(runMe.IsCompletedSuccessfully);
            Assert.IsTrue(here1);
            Assert.AreEqual(res2, runMe.Result);
        }
    }
}
