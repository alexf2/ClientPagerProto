using System.Linq;
using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query
{
    [TestClass]
    public class ComplexFilteringSortingAndPagingTest
    {
        private PersonPageQueryLinqHandler _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new PersonPageQueryLinqHandler(new PersonDataContext(Person.GetTestData()));
        }

        [TestMethod]
        public void FilterByChainOfProperties()
        {
            var query = new PersonPageQuery();
            query.AddFiltering(p => p.Address.City, "n", CriterionPredicate.EndsWith);

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(2, page.Data.Count);
            CollectionAssert.AreEquivalent(new[] { "Amily", "Jim" }, page.Data.Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void FilterWithAnd()
        {
            var query = new PersonPageQuery();
            query.AddFiltering(p => p.Address.City, "n", CriterionPredicate.EndsWith);
            query.AddFiltering(p => p.Name, "a", CriterionPredicate.StartsWith, ignoreCase: true);

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.Data.Count);
            CollectionAssert.AreEquivalent(new[] { "Amily" }, page.Data.Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void FilterWithOr()
        {
            var query = new PersonPageQuery();
            query.AddFiltering(p => p.Address.City, "n", CriterionPredicate.EndsWith);
            query.AddFiltering(p => p.Name, "i", CriterionPredicate.StartsWith, ignoreCase: true);
            query.Filtering.CombiningRule = FilteringCollection.CombiningFilters.Or;

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(3, page.Data.Count);
            CollectionAssert.AreEquivalent(new[] { "Amily", "Jim", "Ivan" }, page.Data.Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SortingByChainOfPropertiesAsc()
        {
            var query = new PersonPageQuery();
            query.AddSorting(p => p.Address.City, desc: false);

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(4, page.Data.Count);
            CollectionAssert.AreEqual(new[] { "Jim", "Amily", "John", "Ivan" }, page.Data.Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void SortingByChainOfPropertiesDesc()
        {
            var query = new PersonPageQuery();
            query.AddSorting(p => p.Address.City, desc: true);

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(4, page.Data.Count);
            CollectionAssert.AreEqual(new[] { "Ivan", "John", "Amily", "Jim" }, page.Data.Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void PagingWithTotal()
        {
            var query = new PersonPageQuery { PageSize = 2, PageNumber = 1 };

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(2, page.Data.Count);
            CollectionAssert.AreEqual(new[] { "Ivan", "John" }, page.Data.Select(p => p.Name).ToArray());

            Assert.AreEqual(4, page.TotalRecordsCount);
            Assert.AreEqual(2, page.TotalPages);
            Assert.AreEqual(1, page.PageNumber);
        }

        [TestMethod]
        public void PagingWithoutTotal()
        {
            var query = new PersonPageQuery { PageSize = 2, PageNumber = 1, ReturnTotalRecods = false };

            var page = _handler.Handle(query);

            Assert.IsNotNull(page);
            Assert.AreEqual(2, page.Data.Count);
            CollectionAssert.AreEqual(new[] { "Ivan", "John" }, page.Data.Select(p => p.Name).ToArray());

            Assert.AreEqual(0, page.TotalRecordsCount);
            Assert.AreEqual(0, page.TotalPages);
            Assert.AreEqual(0, page.PageNumber);
        }
    }
}
