using System;
using System.Linq.Expressions;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests
{
    [TestClass]
    public class ExpressionHelperTests
    {                       
        [TestMethod]
        public void GetMembersChain()
        {
            Assert.AreEqual("Referer.Address.Zip", LinqExpressionHelper.GetMembersChain((Client c) => c.Referer.Address.Zip) );
            Assert.AreEqual("Referer.Address.Zip", LinqExpressionHelper.GetMembersChain<Client>(c => c.Referer.Address.Zip));            
        }

        [TestMethod]
        public void GetMemberChainExpression()
        {
            var param = Expression.Parameter(typeof(Client), "parm");
            var expr = LinqExpressionHelper.GetMemberChainExpression(param, "Referer.Address.Zip");

            var c = new Client() {Referer = new Referer() {Address = new Address2(){Zip = 121108}}};
            Assert.AreEqual(121108, Expression.Lambda<Func<Client, int>>(expr, param).Compile()(c) );
        }
    }
}
