
namespace CqsDataFoundation.Tests.Query
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public static Person[] GetTestData()
        {
            return new[]
                   {
                       new Person {Id = 1, Name = "Ivan", Age = 36, Address = new Address {City = "Yaroslavl"}},
                       new Person {Id = 2, Name = "John", Age = 12, Address = new Address {City = "New York"}},
                       new Person {Id = 3, Name = "Amily", Age = 42, Address = new Address {City = "London"}},
                       new Person {Id = 4, Name = "Jim", Age = 24, Address = new Address {City = "Berlin"}},
                   };
        }
    }

    public class Address
    {
        public string City { get; set; }
    }
}
