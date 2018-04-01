using System;
using System.IO;
using System.Text;
using CqsDataFoundation.Query;
using CqsDataFoundation.Query.Decorators;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace CqsDataFoundation.Tests.Query
{
    [TestClass]
    public class QueryHandlerPaggedLinqTest : QueryHandlerPaggedTestsBase
    {
        CustomerPageQuery _defaultQuery;        
        CustomerPageQuery _sortSurname;
        
        
        CustomerPageQueryLinqHandler _hndlLinq;
        private CustomerRefPageQueryLinqHandler _hndlLinqRef;

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();

            _defaultQuery = new CustomerPageQuery();

            _sortSurname = new CustomerPageQuery() { PageSize = 10 };
            _sortSurname.AddSorting("SurName");


            _hndlLinq = new CustomerPageQueryLinqHandler(new CustomersDataContext(Customer.GetTestdata()));
            _hndlLinqRef = new CustomerRefPageQueryLinqHandler(new CustomersDataContext(Customer.GetTestdata()));
        }


        [TestMethod]
        public void TestLinqWIthConversion()
        {
            var q1 = new CustomerRefPageQuery() { PageSize = Constants.PageSizeNoPagging };
            var pg = _hndlLinqRef.Handle(q1);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 17);

            Assert.AreEqual("Frederik", pg.Data[0].Name);
            Assert.AreEqual("Jons", pg.Data[0].SurName);
        }

        [TestMethod]
        public void TestLinqPagging()
        {
            var q1 = new CustomerPageQuery(){PageSize = Constants.PageSizeNoPagging};
            var pg = _hndlLinq.Handle(q1);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 17);
            

            q1.PageSize = 10;
            q1.PageNumber = 1;
            pg = _hndlLinq.Handle(q1);
            
            Assert.IsTrue(pg.Data.Count == 10);
            Assert.IsTrue(pg.Data[0].Name == "Frederik");
            Assert.IsTrue(pg.Data[9].Name == "Nick 3");

            q1.PageNumber = 2;
            pg = _hndlLinq.Handle(q1);
            
            Assert.IsTrue(pg.Data.Count == 7);
            Assert.IsTrue(pg.Data[0].Name == "Nick 4");
            Assert.IsTrue(pg.Data[6].Name == "Nick' [10]");

            q1.PageNumber = 100;
            pg = _hndlLinq.Handle(q1);
            Assert.IsTrue(pg.PageNumber == 2);
            Assert.IsTrue(pg.TotalPages == 2);
            Assert.IsTrue(pg.Data[0].Name == "Nick 4");
            Assert.IsTrue(pg.Data[6].Name == "Nick' [10]");
            //Dumper.Dump(pg);
        }

        [TestMethod]
        public void TestLinqDefault()
        {
            var data = _hndlLinq.Handle(_defaultQuery);
            var srcdata = Customer.GetTestdata();

            Assert.IsTrue(data.PageNumber == 1);
            Assert.IsTrue(data.TotalPages == 1);
            Assert.IsTrue(data.TotalRecordsCount == srcdata.Length);
            Assert.IsTrue(data.Data.Count == data.TotalRecordsCount);
            Assert.IsTrue(data.Data[0].Name == srcdata[0].Name);
            Assert.IsTrue(data.Data[2].Name == srcdata[2].Name);
        }

        [TestMethod]
        public void TestLinqFilterMultiple()
        {
            var pg = _hndlLinq.Handle(_sortNameFilter2RankAnd);

            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 4);
            Assert.IsTrue(pg.Data.Count == 4);
            Assert.IsTrue(pg.Data[0].Rank == 19);
            Assert.IsTrue(pg.Data[2].Rank == 14);
            Assert.IsTrue(pg.Data[3].Rank == 10);

            pg = _hndlLinq.Handle(_sortNameFilter2RankOr);

            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 3);
            Assert.IsTrue(pg.Data.Count == 3);
            Assert.IsTrue(pg.Data[0].Rank == 16);
            Assert.IsTrue(pg.Data[2].Rank == 2);
        }

        [TestMethod]
        public void TestLinqSorting()
        {
            var pg = _hndlLinq.Handle(_sort1Asc);

            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 3);
            Assert.IsTrue(pg.Data[0].Name == "Thomas");
            Assert.IsTrue(pg.Data[2].Name == "Thomas");
            

            pg = _hndlLinq.Handle(_sort2Asc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 1);
            Assert.IsTrue(pg.Data[0].Name == "Thomas");

            pg = _hndlLinq.Handle(_sort3Asc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 1);
            Assert.IsTrue(pg.Data[0].Name == "Thomas");

            pg = _hndlLinq.Handle(_sort4Asc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 2);
            Assert.IsTrue(pg.TotalRecordsCount == 14);
            Assert.IsTrue(pg.Data[0].Name == "Ariel");
            Assert.IsTrue(pg.Data[9].Name == "Nick 5");

            pg = _hndlLinq.Handle(_sort1Dsc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 2);
            Assert.IsTrue(pg.TotalRecordsCount == 11);
            Assert.IsTrue(pg.Data[0].Name == "Nick 9");
            Assert.IsTrue(pg.Data[9].Name == "Nick' [10]");

            pg = _hndlLinq.Handle(_sort2Dsc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 10);
            Assert.IsTrue(pg.Data[0].Name == "Nick 9");
            Assert.IsTrue(pg.Data[9].Name == "Nick' [10]");

            pg = _hndlLinq.Handle(_sort3Dsc);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 4);
            Assert.IsTrue(pg.Data[0].Name == "Nick 6");
            Assert.IsTrue(pg.Data[2].Name == "Nick 2");

            pg = _hndlLinq.Handle(_sort4Dsc);
            Assert.IsTrue(pg.PageNumber == 0);
            Assert.IsTrue(pg.TotalPages == 0);
            Assert.IsTrue(pg.TotalRecordsCount == 0);

            pg = _hndlLinq.Handle(_not1);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 1);
            Assert.IsTrue(pg.Data[0].Name == "Nick 6");

            pg = _hndlLinq.Handle(_not2);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 7);
            Assert.IsTrue(pg.Data[0].Name == "Frederik");
            Assert.IsTrue(pg.Data[6].Name == "Thomas");

            pg = _hndlLinq.Handle(_sortSurname);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 2);
            Assert.IsTrue(pg.TotalRecordsCount == 17);
            Assert.IsTrue(pg.Data[0].Name == "Nick 6");
            Assert.IsTrue(pg.Data[9].Name == "Nick 4");

            pg = _hndlLinq.Handle(_likeScreen1);
            Assert.IsTrue(pg.PageNumber == 0);
            Assert.IsTrue(pg.TotalPages == 0);
            Assert.IsTrue(pg.TotalRecordsCount == 0);

            pg = _hndlLinq.Handle(_likeScreen2);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 1);
            Assert.IsTrue(pg.Data[0].Name == "Nick' [10]");

            pg = _hndlLinq.Handle(_likeScreen3);
            Assert.IsTrue(pg.PageNumber == 0);
            Assert.IsTrue(pg.TotalPages == 0);
            Assert.IsTrue(pg.TotalRecordsCount == 0);

            var qCase = new CustomerPageQuery() { PageSize = 10 };            
            qCase.AddFiltering("Name", "thOmas", CriterionPredicate.Eq, true);
            qCase.AddFiltering("SurName", "Frank", CriterionPredicate.Neq);
            qCase.AddFiltering("Rank", 5, CriterionPredicate.LtEq);

            pg = _hndlLinq.Handle(qCase);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 2);
            Assert.IsTrue(pg.Data[0].Name == "Thomas");
            Assert.IsTrue(pg.Data[1].Name == "Thomas");

            qCase = new CustomerPageQuery() { PageSize = 10 };
            qCase.AddFiltering("Name", "thOmas", CriterionPredicate.Eq, false);
            qCase.AddFiltering("SurName", "Frank", CriterionPredicate.Neq);
            qCase.AddFiltering("Rank", 5, CriterionPredicate.LtEq);

            pg = _hndlLinq.Handle(qCase);
            Assert.IsTrue(pg.PageNumber == 0);
            Assert.IsTrue(pg.TotalPages == 0);
            Assert.IsTrue(pg.TotalRecordsCount == 0);

            qCase = new CustomerPageQuery() { PageSize = 10 };
            qCase.AddFiltering("Name", "thOmas", CriterionPredicate.StartsWith, true);
            qCase.AddFiltering("SurName", "Frank", CriterionPredicate.Neq);
            qCase.AddFiltering("Rank", 5, CriterionPredicate.LtEq);

            pg = _hndlLinq.Handle(qCase);
            Assert.IsTrue(pg.PageNumber == 1);
            Assert.IsTrue(pg.TotalPages == 1);
            Assert.IsTrue(pg.TotalRecordsCount == 2);
            Assert.IsTrue(pg.Data[0].Name == "Thomas");
            Assert.IsTrue(pg.Data[1].Name == "Thomas");

            qCase = new CustomerPageQuery() { PageSize = 10 };
            qCase.AddFiltering("Name", "thOmas", CriterionPredicate.StartsWith, false);            
            qCase.AddFiltering("Rank", 5, CriterionPredicate.LtEq);

            pg = _hndlLinq.Handle(qCase);
            Assert.IsTrue(pg.PageNumber == 0);
            Assert.IsTrue(pg.TotalPages == 0);
            Assert.IsTrue(pg.TotalRecordsCount == 0);

            //Dumper.Dump(pg);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void QueryValidation()
        {
            var q1 = new CustomerPageQuery() { PageSize = 10, PageNumber = 0 };
            new ValidationQueryHandlerDecorator<CustomerPageQuery, DataPage<Customer>>(null, _hndlLinq).Handle(q1);
        }
        
    }

    public static class Dumper
    {
        public static void Dump<T>(T obj)
        {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            Console.WriteLine(Encoding.Default.GetString(ms.ToArray()));
        }
    }
}
