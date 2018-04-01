using CqsDataFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CM = System.ComponentModel;

namespace CqsDataFoundation.Tests
{
    [TestClass]
    public class DescribedEnumTests
    {
        enum Color
        {
            [CM.Description("Red description")]
            Red,

            [CM.Description("Green description")]
            Green,

            [CM.Description("Blue description")]
            Blue
        }

        [TestMethod]
        public void TesDescribe()
        {
            var descriptors = DescribedEnumItem<Color>.GetItems();

            Assert.AreEqual(3, descriptors.Length);
            Assert.AreEqual(Color.Red, descriptors[0].Value);
            Assert.AreEqual(Color.Green, descriptors[1].Value);
            Assert.AreEqual(Color.Blue, descriptors[2].Value);

            Assert.AreEqual("Red description", descriptors[0].ToString());
            Assert.AreEqual("Green description", descriptors[1].ToString());
            Assert.AreEqual("Blue description", descriptors[2].ToString());
        }
    }
}
