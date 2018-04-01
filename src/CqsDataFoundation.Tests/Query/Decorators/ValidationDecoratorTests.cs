using System;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    [TestClass]
    public sealed class ValidationDecoratorTests
    {                        
        IQueryHandler<ClientQuery, Client> _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new ValidationQueryHandlerDecorator<ClientQuery, Client>(null, new ClientQueryLinqHandler(new ClientDataContext(new Client[0])));
        }

        [TestMethod]
        public void HandleValid()
        {
            var q = new ClientQuery()
            {
                GivenName = "Paul",
                SurName = "Glenn",                
            };

            var res = _handler.Handle(q);

            Assert.AreEqual("Paul", res.GivenName);
            Assert.AreEqual("Glenn", res.SurName);
        }

        [TestMethod]
        public void ValidateInvalid()
        {
            var q = new ClientQuery()
            {                
                SurName = "Glenn"                
            };
            
            try
            {
                _handler.Handle(q);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1 || !(ex.InnerExceptions[0] is ValidationException) || !ex.InnerExceptions[0].Message.Contains("GivenName"))
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void ValidateObject_IsValid()
        {
            var q = new ClientQuery()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                ExtParams = new ExtParams() { RefererName = "Hanso" }
            };

            var res = _handler.Handle(q);

            Assert.AreEqual("Paul", res.GivenName);
            Assert.AreEqual("Glenn", res.SurName);
            
        }

        [TestMethod]
        public void ValidateObject_IsInvalid()
        {
            var q = new ClientQuery()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                ExtParams = new ExtParams() {}
            };
            
            try
            {
                _handler.Handle(q);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1 || !(ex.InnerExceptions[0] is ValidationException) || !ex.InnerExceptions[0].Message.Contains("RefererName"))
                    Assert.Fail();
            }
        }
    }
}
