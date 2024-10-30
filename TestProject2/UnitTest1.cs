using WavChunkReader;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.ThrowsException<ArgumentException>(() => { new RiffReader(string.Empty); });
        }
    }
}