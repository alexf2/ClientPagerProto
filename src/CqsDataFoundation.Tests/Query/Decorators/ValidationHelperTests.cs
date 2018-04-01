using System;
using System.ComponentModel.DataAnnotations;
using CqsDataFoundation.Tests.Dto;
using CqsDataFoundation.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query.Decorators
{
    [TestClass]
    public sealed class ValidationHelperTests
    {        
        [TestMethod]
        public void ValidateValid()
        {
            var client = new Client()
            {
                GivenName = "Paul",
                SurName = "Glenn",

                Referer = new Referer() { Code = "12786" }
            };
            var context = new ValidationContext(client, null, null);
            ValidationHelper.Validate(client, context);
        }

        [TestMethod]
        public void ValidateInvalid()
        {
            var client = new Client()
            {
                GivenName = "Paul",
                SurName = "Glenn",

                Referer = new Referer() { Code = "1278" }
            };
            var context = new ValidationContext(client, null, null);
            try
            {
                ValidationHelper.Validate(client, context);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1 || !(ex.InnerExceptions[0] is ValidationException) || !ex.InnerExceptions[0].Message.Contains("Code"))
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void ValidateObject_IsValid()
        {
            var client = new Client()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                Address = new Address2() { Zip = 127865, City = "Auklend", Street = "Padington", House = 12, Flat  = 15},

                Referer = new Referer() { Code = "12786" }
            };
            var context = new ValidationContext(client, null, null);
            ValidationHelper.Validate(client, context);
        }

        [TestMethod]
        public void ValidateObject_IsInvalid()
        {
            var client = new Client()
            {
                GivenName = "Paul",
                SurName = "Glenn",
                Address = new Address2() { Zip = 127865, Street = "Padington", House = 12, Flat = 15 },

                Referer = new Referer() { Code = "12786" }
            };
            var context = new ValidationContext(client, null, null);
            try
            {
                ValidationHelper.Validate(client, context);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count != 1 || !(ex.InnerExceptions[0] is ValidationException) || !ex.InnerExceptions[0].Message.Contains("City"))
                    Assert.Fail();
            }
        }
    }
}
