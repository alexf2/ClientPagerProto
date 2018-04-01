using System.Collections.Generic;
using System.Linq;
using CqsDataFoundation.Query;

namespace ClientPagerProto.PaggedList
{
    public abstract class PagedListDataProviderBase<TQuery, TItem, TResult> : IPagedListDataProvider 
        where TQuery : QueryPaggedBase<TItem, TResult>, new()
        where TResult : DataPage<TItem>
    {
        public DataPage<PagedListItem> FetchPage(PagedListConfigModel model)
        {
            var q = new TQuery();

            ConfigureQuery(q, model);
            TResult pageOfData = QueryData(q);

            var staticItems = GetStaticItems(model, q); ;                
            var res = pageOfData.Data.Select(ListItemFromDataItem);
            
            return new DataPage<PagedListItem>()
            {
                Data = (staticItems != null && staticItems.Count > 0 ? staticItems.Union(res) : res).ToList(),
                TotalRecordsCount = pageOfData.TotalRecordsCount,
                PageNumber = pageOfData.PageNumber,
                TotalPages = pageOfData.TotalPages
            };
        }

        protected virtual void ConfigureQuery(TQuery query, PagedListConfigModel model)
        {
            query.PageSize = model.Pagging.PageSize;
            query.PageNumber = model.Pagging.RequestedPageNumber;

            if (model.Sorting.HasSorting)
                query.AddSorting(model.Sorting.SortByValue ? ValueFieldName : DescriptionFieldName, model.Sorting.SortingOrder == PagedListSortOrder.Desc);


            if (model.Filtering.HasFiltering && model.Filtering.FilterValue != null)
            {
                var strVal = model.Filtering.FilterValue as string;

                if (strVal == null || strVal.Length >= model.Filtering.MinFilterSize)
                    query.AddFiltering(
                        model.Filtering.FilterByValue ? ValueFieldName : DescriptionFieldName, 
                        model.Filtering.FilterValue, ConvertCriterion(model.Filtering.FilteringMode), 
                        !model.Filtering.FilterCaseSensitive
                 );
            }
        }

        protected abstract string ValueFieldName { get; }
        protected abstract string DescriptionFieldName { get; }
        protected abstract PagedListItem ListItemFromDataItem(TItem data);

        protected virtual IList<PagedListItem> GetStaticItems(PagedListConfigModel m, TQuery q)
        {
            return null;
        }

        protected abstract TResult QueryData(TQuery q);

        static CriterionPredicate ConvertCriterion(PagedListFilteringMode fmode)
        {
            switch (fmode)
            {
                case PagedListFilteringMode.Contains:
                    return CriterionPredicate.Contains;

                case PagedListFilteringMode.StartsWith:
                    return CriterionPredicate.StartsWith;

                case PagedListFilteringMode.EndsWith:
                    return CriterionPredicate.EndsWith;

                default:
                    return CriterionPredicate.Contains;
            }
        }
    }
}