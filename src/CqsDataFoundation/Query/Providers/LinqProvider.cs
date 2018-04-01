using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CqsDataFoundation.Query.Filtering;
using CqsDataFoundation.Query.Sorting;

namespace CqsDataFoundation.Query.Providers
{    
    internal sealed class LinqProvider<T> : IQueryVisitor
    {
        delegate BinaryExpression Predicate(Expression left, Expression right);

        readonly Type _t;
        readonly ParameterExpression _param;

        IQueryable<T> _query;
        bool _isFirstItem;
        Expression _expression;

        public LinqProvider()
        {
            _t = typeof (T);
            _param = Expression.Parameter(_t, "parm");
            Validator = LinqProviderStatic.DefaultPropValidator;
        }

        public PropertyValidator Validator { get; set; }

        public IQueryable<T> ApplySorting(IQueryable<T> query, SortingCollection coll)
        {
            _query = query;            
            coll.Accept(this);

            var res = _query;
            _query = null;

            return res;
        }

        public IQueryable<T> ApplyFiltering(IQueryable<T> query, FilteringCollection coll)
        {
            _query = query;
            _expression = null;
            coll.Accept(this);            
            _query = _query.Where( Expression.Lambda<Func<T, bool>>(_expression, _param) );

            var res = _query;
            _query = null;

            return res;
        }

        public IQueryable<T> ApplyPaging(IQueryable<T> query, int pageSize, int pageNumber)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        #region IQueryVisitor
        void IQueryVisitor.Visit(SortingCollection sorting)
        {
            _isFirstItem = true;
            foreach (var sd in sorting)
                sd.Accept(this);
        }
        void IQueryVisitor.Visit(SortingDescriptor sd)
        {
            Validator.ValidatePropertyName<T>(sd.Field);

            string ordAsc = _isFirstItem ? "OrderBy" : "ThenBy";
            string ordDesc = _isFirstItem ? "OrderByDescending" : "ThenByDescending";
            _isFirstItem = false;
                                   
            Expression propertyAccess = LinqExpressionHelper.GetMemberChainExpression(_param, sd.Field);
            LambdaExpression orderByExp = Expression.Lambda(propertyAccess, _param);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), sd.Desc ? ordDesc : ordAsc, new[] { _t, propertyAccess.Type },
                                        _query.Expression, Expression.Quote(orderByExp));

            _query = _query.Provider.CreateQuery<T>(resultExp);
        }


        void IQueryVisitor.Visit(FilteringCollection filtering)
        {                        
            Predicate predicate = filtering.CombiningRule == FilteringCollection.CombiningFilters.Or
                ? (Predicate)Expression.OrElse
                : Expression.AndAlso;

            filtering[ 0 ].Accept(this);
            var expression = _expression;

            for (var i = 1; i < filtering.Count; ++i)
            {
                var filterDescriptor = filtering[i];

                filterDescriptor.Accept(this);
                if (_expression == null)
                    continue; 
                var descriptorExpression = _expression;
                expression = predicate(expression, descriptorExpression);
            }

            _expression = expression;
        }

        void IQueryVisitor.Visit(FilteringDescriptor filter)
        {
            // The member you want to evaluate (x => x.FirstName)            
            Expression member = LinqExpressionHelper.GetMemberChainExpression(_param, filter.Field);

            // The value you want to evaluate
            Expression constant = Expression.Constant(filter.Criterion);

            if (filter.IgnoreCase && filter.Criterion is string)
            {
                member = Expression.Call(Expression.Coalesce(member, LinqProviderStatic.EmptyStr), LinqProviderStatic.ToUpper);
                constant = Expression.Constant(string.IsNullOrEmpty((string)filter.Criterion) ? string.Empty : (string)filter.Criterion);
                constant = Expression.Call(constant, LinqProviderStatic.ToUpper);
            }


            // Determine how we want to apply the expression
            switch (filter.Predicate)
            {
                case CriterionPredicate.Eq:
                case CriterionPredicate.Null:
                    _expression = Expression.Equal(member, constant);
                    break;

                case CriterionPredicate.Neq:
                case CriterionPredicate.NotNull:
                    _expression = Expression.NotEqual(member, constant);
                    break;

                case CriterionPredicate.Gt:
                    _expression = Expression.GreaterThan(member, constant);
                    break;

                case CriterionPredicate.GtEq:
                    _expression = Expression.GreaterThanOrEqual(member, constant);
                    break;

                case CriterionPredicate.Lt:
                    _expression = Expression.LessThan(member, constant);
                    break;

                case CriterionPredicate.LtEq:
                    _expression = Expression.LessThanOrEqual(member, constant);
                    break;

                case CriterionPredicate.Contains:
                    //return Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null)),
                    //Expression.Call(member, PaggedLinqBaseStatics.ContainsMethod, constant));
                    _expression = Expression.Call(Expression.Coalesce(member, LinqProviderStatic.EmptyStr), LinqProviderStatic.ContainsMethod, constant);
                    break;

                case CriterionPredicate.StartsWith:
                    _expression = Expression.Call(Expression.Coalesce(member, LinqProviderStatic.EmptyStr), LinqProviderStatic.StartsWithMethod, constant);
                    break;

                case CriterionPredicate.EndsWith:
                    _expression = Expression.Call(Expression.Coalesce(member, LinqProviderStatic.EmptyStr), LinqProviderStatic.EndsWithMethod, constant);
                    break;
            }            
        }
        #endregion IQueryVisitor        
    }

    internal static class LinqProviderStatic
    {
        internal static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        internal static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        internal static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
        internal static readonly MethodInfo ToUpper = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
        internal static readonly Expression EmptyStr = Expression.Constant(string.Empty);
        internal static readonly PropertyValidator DefaultPropValidator = new PropertyValidator();
    };
}
