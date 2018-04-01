using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query
{
    public class QueryHandlerPaggedTestsBase
    {
        protected CustomerPageQuery _sort1Asc, _sort2Asc, _sort3Asc, _sort4Asc;
        protected CustomerPageQuery _sort1Dsc, _sort2Dsc, _sort3Dsc, _sort4Dsc;
        protected CustomerPageQuery _not1, _not2, _likeScreen1, _likeScreen2, _likeScreen3;
        protected CustomerPageQuery _sortNameFilter2RankAnd, _sortNameFilter2RankOr;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _sort1Asc = new CustomerPageQuery() { PageSize = 10 };
            _sort1Asc.AddSorting("Name")
                .AddFiltering("Name", "Thomas", CriterionPredicate.Eq);

            _sortNameFilter2RankAnd = new CustomerPageQuery() { PageSize = 10 };
            _sortNameFilter2RankAnd.AddSorting("Rank", true)
                .AddSorting("Rank", true)
                .AddFiltering("Rank", 10, CriterionPredicate.GtEq)
                .AddFiltering("Rank", 20, CriterionPredicate.LtEq)
                .Filtering.CombiningRule = FilteringCollection.CombiningFilters.And;

            _sortNameFilter2RankOr = new CustomerPageQuery() { PageSize = 10 };
            _sortNameFilter2RankOr.AddSorting("Rank", true)
                .AddSorting("Rank", true)
                .AddFiltering("Rank", 14, CriterionPredicate.Eq)
                .AddFiltering("Rank", 16, CriterionPredicate.Eq)
                .AddFiltering("Rank", 2, CriterionPredicate.Eq)
                .Filtering.CombiningRule = FilteringCollection.CombiningFilters.Or;

            _sort2Asc = new CustomerPageQuery() { PageSize = 10 };
            _sort2Asc.AddSorting("Name"); _sort2Asc.AddSorting("SurName")
                .AddFiltering("Name", "Thomas", CriterionPredicate.Eq)
                .AddFiltering("SurName", "Nikolas", CriterionPredicate.Eq);

            _sort3Asc = new CustomerPageQuery() { PageSize = 10 };
            _sort3Asc.AddSorting("Name")
                .AddSorting("SurName")
                .AddSorting("Rank")
                .AddFiltering("Name", "Thomas", CriterionPredicate.Eq)
                .AddFiltering("SurName", "Nikolas", CriterionPredicate.Eq)
                .AddFiltering("Rank", 7, CriterionPredicate.Eq);

            _sort4Asc = new CustomerPageQuery() { PageSize = 10 };
            _sort4Asc.AddSorting("Name")
                .AddSorting("SurName")
                .AddSorting("Rank")
                .AddSorting("Amt")
                .AddFiltering("Name", "Thomas", CriterionPredicate.Neq);


            _sort1Dsc = new CustomerPageQuery() { PageSize = 10 };
            _sort1Dsc.AddSorting("Name", true)
                .AddFiltering("Rank", 10, CriterionPredicate.Gt);

            _sort2Dsc = new CustomerPageQuery() { PageSize = 10 };
            _sort2Dsc.AddSorting("Name", true)
                .AddSorting("SurName", true)
                .AddFiltering("Name", "ck", CriterionPredicate.Contains);

            _sort3Dsc = new CustomerPageQuery() { PageSize = 10 };
            _sort3Dsc.AddSorting("Name", true)
                .AddSorting("SurName", true)
                .AddSorting("Rank", true)
                .AddFiltering("Name", "Ni", CriterionPredicate.StartsWith)
                .AddFiltering("Amt", 15.5f, CriterionPredicate.LtEq);

            _sort4Dsc = new CustomerPageQuery() { PageSize = 10 };
            _sort4Dsc.AddSorting("Name", true)
                .AddSorting("SurName", true)
                .AddSorting("Rank", true)
                .AddSorting("Amt", true)
                .AddFiltering("Name", "As", CriterionPredicate.EndsWith)
                .AddFiltering("Amt", 17.25f, CriterionPredicate.GtEq);



            _not1 = new CustomerPageQuery() { PageSize = 10 };
            _not1.AddFiltering("SurName", null, CriterionPredicate.Null)
                .AddFiltering("Amt", 1f, CriterionPredicate.Gt);

            _not2 = new CustomerPageQuery() { PageSize = 10 };
            _not2.AddFiltering("SurName", null, CriterionPredicate.NotNull)
                .AddFiltering("SurName", "Tomazo", CriterionPredicate.Neq);

            _likeScreen1 = new CustomerPageQuery() { PageSize = 10 };
            _likeScreen1.AddFiltering("Name", "abc'rgb_1", CriterionPredicate.Neq)
                .AddFiltering("SurName", "Tom'az'!", CriterionPredicate.Contains);

            _likeScreen2 = new CustomerPageQuery() { PageSize = 10 };
            _likeScreen2.AddFiltering("Name", "' [10]", CriterionPredicate.Contains);

            _likeScreen3 = new CustomerPageQuery() { PageSize = 10 };
            _likeScreen3.AddFiltering("Name", "abc'''bcb123%%111", CriterionPredicate.Contains);
        }
    }
}
