
namespace CqsDataFoundation.Tests.Query
{
    public sealed class Customer
    {
        public string Name;
        public string SurName;
        public int Rank;
        public float Amt;
        public bool Active;

        public static Customer[] GetTestdata()
        {
            return new[]
            {
                new Customer() {Name = "Frederik", SurName = "Jons", Rank = 10, Amt = 12.5f, Active = true}, //0
                new Customer() {Name = "maria", SurName = "Anne", Rank = 2, Amt = 15.1f, Active = true}, //1
                new Customer() {Name = "Thomas", SurName = "Cruz", Rank = 5, Amt = 16.5f, Active = true}, //2
                new Customer() {Name = "Michael", SurName = "Uitni", Rank = 19, Amt = 22.5f, Active = true}, //3
                new Customer() {Name = "Thomas", SurName = "Nikolas", Rank = 7, Amt = 122.5f, Active = true}, //4
                new Customer() {Name = "Ariel", SurName = "Yoder", Rank = 9, Amt = 222.5f, Active = true}, //5
                new Customer() {Name = "Thomas", SurName = "Cruz", Rank = 4, Amt = 127.5f, Active = true}, //6

                new Customer() {Name = "Nick 1", SurName = "Tomazo", Rank = 14, Amt = 19.99f, Active = true},
                new Customer() {Name = "Nick 2", SurName = "Tomazo", Rank = 16, Amt = 12.27f, Active = true},
                new Customer() {Name = "Nick 3", SurName = "Tomazo", Rank = 44, Amt = 13.2f, Active = true},
                new Customer() {Name = "Nick 4", SurName = "Tomazo", Rank = 31, Amt = 17.6f, Active = false},
                new Customer() {Name = "Nick 5", SurName = "Tomazo", Rank = 30, Amt = 18.5785f, Active = true},
                new Customer() {Name = "Nick 6", SurName = null, Rank = 43, Amt = 15.323f, Active = true},
                new Customer() {Name = "Nick 7", SurName = "Tomazo", Rank = 42, Amt = 122.434f, Active = false},
                new Customer() {Name = "Nick 8", SurName = "Tomazo", Rank = 42, Amt = 181.2323f, Active = true},
                new Customer() {Name = "Nick 9", SurName = "Tomazo", Rank = 42, Amt = 987.2323f, Active = false},
                new Customer() {Name = "Nick' [10]", SurName = "Tomazo", Rank = 42, Amt = 9.23f, Active = true}
            };
        }
    };

    public sealed class CustomerRef
    {
        public string Name;
        public string SurName;
    }
}
