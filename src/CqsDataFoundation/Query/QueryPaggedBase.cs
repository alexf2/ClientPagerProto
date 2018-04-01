using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Runtime.Serialization;
using CqsDataFoundation.Query.Filtering;
using CqsDataFoundation.Query.Sorting;


namespace CqsDataFoundation.Query
{
    [Serializable]
    public class QueryPaggedBase<TItem, TResult> : QueryBase<TResult>, IQueryPagged<TResult>
    {        
        [DataMember(Order = 1, Name = "SortingOrder")]
        protected SortingCollection _sorting;
        [DataMember(Order = 2, Name = "FilteringCriterions")]
        protected FilteringCollection _filtering;

        public QueryPaggedBase()
            : this(Constants.PageSizeNoPagging, 1)
        {            
        }

        public QueryPaggedBase(int size, int pageNumber)
        {
            ReturnTotalRecods = true;
            PageSize = size;
            PageNumber = pageNumber;
        }

        #region IQuery
        public override string CacheKey
        {
            get
            {
                var bld = new StringBuilder();
                bld.AppendFormat("{0}_{1}", PageSize, PageNumber);
                
                bld.Append('_');
                GetFilteringDescription(bld);
                
                bld.Append('_');
                GetSortingDescription(bld);
                
                return bld.ToString();
            }
        }        
        #endregion IQuery

        #region IQueryPagged
        [Range(Constants.PageSizeNoPagging, 1000, ErrorMessage = "PageSize should be in ragne 0 - 1000. -1 means turning pagging off.")]
        [DataMember(Order = 3)]
        [DefaultValue(Constants.PageSizeNoPagging)] //no pagging
        public int PageSize
        {
            get;
            set;
        }

        [Range(1, int.MaxValue, ErrorMessage = "PageNumber should be greater, than 0.")]
        [DataMember(Order = 4)]
        [DefaultValue(1)]
        public int PageNumber
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        [DefaultValue(true)]
        public bool ReturnTotalRecods
        {
            get; 
            set;
        }

        
        public bool HasPaging
        {
            get { return PageSize > Constants.PageSizeNoPagging; }
        }
        
        public bool HasSorting
        {
            get { return _sorting != null && _sorting.Count > 0; }
        }
        
        public bool HasFiltering
        {
            get { return _filtering != null && _filtering.Count > 0; }
        }

        public SortingCollection Sorting
        {
            get { return _sorting; }
        }

        public FilteringCollection Filtering
        {
            get { return _filtering; }
        }        
        #endregion IQueryPagged

        #region Building
        public QueryPaggedBase<TItem, TResult> AddSorting(string field, bool desc = false)
        {
            if (_sorting == null)
                _sorting = new SortingCollection();
            _sorting.Add(new SortingDescriptor() { Field = field, Desc = desc });

            return this;
        }

        public QueryPaggedBase<TItem, TResult> AddSorting(Expression<Func<TItem, object>> memberSelector, bool desc = false)
        {
            if (_sorting == null)
                _sorting = new SortingCollection();
            _sorting.Add(SortingDescriptor.Get(memberSelector, desc));

            return this;
        }

        public QueryPaggedBase<TItem, TResult> AddFiltering(string field, object criterion, CriterionPredicate predicate = CriterionPredicate.Eq, bool ignoreCase = false)
        {
            if (_filtering == null)
                _filtering = new FilteringCollection();
            _filtering.Add(new FilteringDescriptor() { Field = field, Predicate = predicate, Criterion = criterion, IgnoreCase = ignoreCase});

            return this;
        }

        public QueryPaggedBase<TItem, TResult> AddFiltering(Expression<Func<TItem, object>> memberSelector, object criterion, CriterionPredicate predicate = CriterionPredicate.Eq, bool ignoreCase = false)
        {
            if (_filtering == null)
                _filtering = new FilteringCollection();
            _filtering.Add( FilteringDescriptor.Get(memberSelector, criterion, predicate, ignoreCase) );

            return this;
        }        

        public void SetFilteringCombieRule(FilteringCollection.CombiningFilters cf)
        {
            if (_filtering == null)
                _filtering = new FilteringCollection();
            _filtering.CombiningRule = cf;
        }

        public void ClearFilterig()
        {
            if (_filtering != null)
                _filtering.Clear();
        }
        public void ClearSorting()
        {
            if (_sorting != null)
                _sorting.Clear();
        }
        #endregion Building

        void GetSortingDescription(StringBuilder bld)
        {
            if (!HasSorting)
            {
                bld.Append("unsorted");
                return;
            }
            _sorting.GetDescription(bld);            
        }
        void GetFilteringDescription(StringBuilder bld)
        {
            if (!HasFiltering)
            {
                bld.Append("unfiltered");
                return;
            }

            _filtering.GetDescription(bld);            
        }
    }
}
