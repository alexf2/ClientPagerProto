using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation;
using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    [TestClass]
    public class CacheDecoratorTests
    {
        class ClientQueryNoCacahe : ClientQuery
        {
            public override string CacheKey
            {
                get { return null; }
            }
        }

        IQueryHandler<ClientQuery, Client> _handler;
        IDataCache _cache;

        [TestInitialize]
        public void TestInitialize()
        {
            _cache = new MemCache("MyCache");
            _handler = new CacheQueryHandlerDecorator<ClientQuery, Client>(new ClientQueryLinqHandler(new ClientDataContext(new Client[0])), _cache);
        }

        [TestMethod]
        public void CachedQueryTest()
        {
            var cli = new Client() {GivenName = "Hans", MiddleName = "P.", SurName = "Anderson"};

            var q = new ClientQuery() { GivenName = "Hans", SurName = "Anderson" };

            _cache.Set(q.CacheKey, cli);
            var res = _handler.Handle(q);

            Assert.AreSame(cli, res);
        }

        [TestMethod]
        public void RepeatQueryTest()
        {            
            var q = new ClientQuery() { GivenName = "Hans", SurName = "Anderson" };            
            var res = _handler.Handle(q);

            Assert.AreEqual("Hans", res.GivenName);
            Assert.AreEqual("Anderson", res.SurName);

            var res2 = _handler.Handle(q);

            Assert.AreEqual("Hans", res2.GivenName);
            Assert.AreEqual("Anderson", res2.SurName);

            Assert.AreSame(res, res2);
        }

        [TestMethod]
        public void RepeatQueryNonCachedTest()
        {
            var q = new ClientQueryNoCacahe() { GivenName = "Hans", SurName = "Anderson" };
            var res = _handler.Handle(q);

            Assert.AreEqual("Hans", res.GivenName);
            Assert.AreEqual("Anderson", res.SurName);

            var res2 = _handler.Handle(q);

            Assert.AreEqual("Hans", res2.GivenName);
            Assert.AreEqual("Anderson", res2.SurName);

            Assert.AreNotSame(res, res2);
        }

    }
}
