using System;

namespace Molto.IntegrationTests.Abstractions
{
    public class Test
    {
        //Default Constructor
        public Test()
        {

        }

        //Constructor with an argument
        public Test(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsValid { get; set; }
        public long Eta { get; set; }
        public DateTime CreatedAt { get; set; }
        public Fruits Fruit { get; set; }

        //Null values
        public bool? PrivacyAccepted { get; set; }
        public decimal? Discount { get; set; }
        public long? Employees { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        //Protected properties
        protected string HiddenGem { get; set; }
    }

    public enum Fruits
    {
        Apple = 0,
        Banana = 1,
        Orange = 2,
        Lemon = 3
    }
}
