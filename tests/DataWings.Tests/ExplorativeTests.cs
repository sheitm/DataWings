using System.Diagnostics;
using NUnit.Framework;

namespace DataWings.Tests
{
    [TestFixture]
    public class ExplorativeTests
    {
        [Test]
        public void HowDoesStackTraceWork()
        {
            var callStack = new StackTrace();
            int index = 0;
            while (index < callStack.FrameCount)
            {
                var frame = callStack.GetFrame(index);
                var meth = frame.GetMethod();
                index++;
            }
        }
    }
}
