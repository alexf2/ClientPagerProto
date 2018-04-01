using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Tests.Query.Decorators;
using CqsDataFoundation.Command;
using CqsDataFoundation.Command.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Command.Decorators
{
    [TestClass]
    public sealed class ValidationCommandTests
    {    
        private ICommandHandler<AddClientCommand> _handler;

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = new ValidationCommandHandlerDecorator<AddClientCommand>(null, new AddClientCommandHandler(new ClientDataContext(new List<Client>())));
        }

        [TestMethod]
        public void HandleValid()
        {
            var cmd = new AddClientCommand()
            {
                GivenName = "Paul",
                SurName = "Glenn",
            };

            _handler.Handle(cmd);            
        }

        [TestMethod]
        public void ValidateInvalid()
        {
            var cmd = new AddClientCommand()
            {
                SurName = "Glenn"
            };

            try
            {
                _handler.Handle(cmd);
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
            var cmd = new AddClientCommand()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                Address = new Address2() { City = "London", Street = "Padington", House = 7, Flat = 11 }
            };

            _handler.Handle(cmd);
        }

        [TestMethod]
        public void ValidateObject_IsInvalid()
        {
            var q = new AddClientCommand()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                Address = new Address2() { City = "London", Street = "Padington", House = 7}
            };

            try
            {
                _handler.Handle(q);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1 || !(ex.InnerExceptions[0] is ValidationException) || !ex.InnerExceptions[0].Message.Contains("Flat"))
                    Assert.Fail();
            }
        }
    }
}
