using System;
using System.Threading;
using CqsDataFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests
{    
    [TestClass]
    public class MemCacheTest
    {
        private IDataCache _cache;

        [TestInitialize]
        public void TestInitialize()
        {
            _cache = new MemCache("MyCache");
            _cache.Set("k1", "Test simple value 1");
            _cache.Set("k2", "Test simple value 2");
            _cache.Set("k3", "Test simple value 3", TimeSpan.FromSeconds(3), true);
        }
         
        [TestMethod]
        public void TestContains()
        {
            Assert.IsTrue(_cache.Contains("k1"));
            Assert.AreEqual("Test simple value 1", _cache.Get<string>("k1"));

            Assert.IsTrue(_cache.Contains("k2"));
            Assert.AreEqual("Test simple value 2", _cache.Get<string>("k2"));

            Assert.IsTrue(_cache.Contains("k3"));
            Assert.AreEqual("Test simple value 3", _cache.Get<string>("k3"));
        }

        [TestMethod]
        public void TestRemove()
        {
            _cache.Remove("k2");
            Assert.IsFalse(_cache.Contains("k2"));
        }

        [TestMethod]
        public void TestExpired()
        {
            Assert.IsTrue(_cache.Contains("k3"));
            Thread.Sleep(5 * 1000);
            Assert.IsFalse(_cache.Contains("k3"));
        }

        [TestMethod]
        public void TestSlidingExpiration()
        {
            _cache.Set("k2", "Test simple value 2", TimeSpan.FromSeconds(3), false);
            Assert.IsTrue(_cache.Contains("k2"));

            Thread.Sleep(1 * 1000);
            Assert.AreEqual("Test simple value 2", _cache.Get<string>("k2"));
            Thread.Sleep(2 * 1000);
            Assert.AreEqual("Test simple value 2", _cache.Get<string>("k2"));
            Thread.Sleep(1 * 1000);
            Assert.AreEqual("Test simple value 2", _cache.Get<string>("k2"));
            Thread.Sleep(2 * 1000);
            Assert.AreEqual("Test simple value 2", _cache.Get<string>("k2"));

            Thread.Sleep(5 * 1000);
            Assert.IsFalse(_cache.Contains("k3"));
        }
    }
}
