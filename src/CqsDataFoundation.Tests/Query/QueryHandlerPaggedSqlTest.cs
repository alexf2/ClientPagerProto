using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query
{
    [TestClass]
    public class QueryHandlerPaggedSqlTest : QueryHandlerPaggedTestsBase
    {
        CustomerPageQuerySqlHandler _hndlSql;
        

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            _hndlSql = new CustomerPageQuerySqlHandler(new CustomersDataContext(Customer.GetTestdata()));
        }

        [TestMethod]
        public void TestSqlSort()
        {
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort1Asc), "[Name]");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort2Asc), "[Name], [SurName]");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort3Asc), "[Name], [SurName], [Rank]");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort4Asc), "[Name], [SurName], [Rank], [Amt]");

            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort1Dsc), "[Name] desc");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort2Dsc), "[Name] desc, [SurName] desc");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort3Dsc), "[Name] desc, [SurName] desc, [Rank] desc");
            Assert.AreEqual(_hndlSql.GetSortSqlExpressionTest(_sort4Dsc), "[Name] desc, [SurName] desc, [Rank] desc, [Amt] desc");
        }

        [TestMethod]
        public void TestSqlFilter()
        {
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort1Asc), "[Name] = 'Thomas'");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort2Asc), "[Name] = 'Thomas' and [SurName] = 'Nikolas'");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort3Asc), "[Name] = 'Thomas' and [SurName] = 'Nikolas' and [Rank] = 7");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort4Asc), "[Name] <> 'Thomas'");

            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort1Dsc), "[Rank] > 10");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort2Dsc), "[Name] like '%ck%'");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort3Dsc), "[Name] like 'Ni%' and [Amt] <= 15.5");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sort4Dsc), "[Name] like '%As' and [Amt] >= 17.25");

            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_not1), "[SurName] is null and [Amt] > 1");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_not2), "[SurName] is not null and [SurName] <> 'Tomazo'");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_likeScreen1), "[Name] <> 'abc''rgb_1' and [SurName] like '%Tom''az''!%'");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_likeScreen2), "[Name] like '%'' ![10!]%' escape '!'");

            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_likeScreen3), "[Name] like '%abc''''''bcb123!%!%111%' escape '!'");
        }

        [TestMethod]
        public void TestSqlFilterMultiple()
        {
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sortNameFilter2RankAnd), "[Rank] >= 10 and [Rank] <= 20");
            Assert.AreEqual(_hndlSql.GetFilterExpressionTest(_sortNameFilter2RankOr), "[Rank] = 14 or [Rank] = 16 or [Rank] = 2");
        }
    }
}
