using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CqsDataFoundation
{
    internal static class LinqExpressionHelper
    {
        public static string GetMembersChain<TEntity>(Expression<Func<TEntity, object>> memberSelector)
        {
            var expression = memberSelector.Body;

            var builder = new StringBuilder();
            if (expression.NodeType == ExpressionType.Convert)
                expression = ((UnaryExpression) expression).Operand;

            while (expression.NodeType == ExpressionType.MemberAccess)
            {
                var memberAccessExpression = (MemberExpression)expression;

                if (builder.Length != 0)
                {
                    builder.Insert(0, ".");
                }

                builder.Insert(0, memberAccessExpression.Member.Name);

                expression = memberAccessExpression.Expression;
            }

            if (builder.Length == 0)
            {
                throw new ArgumentException("Input expression must contain only access to properties or fields", "memberSelector");
            }

            return builder.ToString();
        }

        public static Expression GetMemberChainExpression(ParameterExpression parameter, string memberChain)
        {
            Expression expression = parameter;

            var type = parameter.Type;
            var members = memberChain.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var member in members)
            {
                var memberInfo = (MemberInfo)type.GetProperty(member) ?? type.GetField(member);

                if (memberInfo == null)
                    throw new InvalidOperationException(string.Format("Unable to find member '{0}' of type '{1}'", member, type));

                expression = Expression.MakeMemberAccess(expression, memberInfo);
                type = expression.Type;
            }

            return expression;
        }
    }
}
