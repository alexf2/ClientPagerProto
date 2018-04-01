using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CqsDataFoundation.Tests.Query
{
    [TestClass]
    public class FilteringDescriptorTests
    {        
        [TestMethod]
        public void TestLinqWIthConversion()
        {
            var f1 = new FilteringDescriptor() { Field = "f1", Predicate = CriterionPredicate.Gt, Criterion = "sss"};
            var f2 = new FilteringDescriptor() { Field = "f1", Predicate = CriterionPredicate.Gt, Criterion = "sss" };
            var f3 = new FilteringDescriptor() { Predicate = CriterionPredicate.Gt, Criterion = "sss" };
            var f4 = new FilteringDescriptor() { Field = "f1", Predicate = CriterionPredicate.Gt};
            var f5 = new FilteringDescriptor() { Field = "f1", Predicate = CriterionPredicate.Lt, Criterion = "sss" };
            var f6 = new FilteringDescriptor() { Field = "f2", Predicate = CriterionPredicate.Gt, Criterion = "sss" };
            var f7 = new FilteringDescriptor() { Field = "f1", Predicate = CriterionPredicate.Gt, Criterion = "sss2" };

            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 == f3);
            Assert.IsFalse(f1 == f4);
            Assert.IsFalse(f1 == f5);
            Assert.IsFalse(f1 == f6);
            Assert.IsFalse(f1 == f7);

            Assert.IsFalse(f1 < f2);
            Assert.IsFalse(f1 < f3);
            Assert.IsFalse(f1 < f4);
            Assert.IsFalse(f1 < f5);
            Assert.IsTrue(f1 < f6);
            Assert.IsTrue(f1 < f7);            
        }
    }
}
