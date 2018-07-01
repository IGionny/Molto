using System;
using Molto.MapByAttributes;

namespace Molto.IntegrationTests.Abstractions
{
    [TableName("Test")]
    public class Alias :IEntity<Guid>
    {
        public Guid Id { get; set; }

        [Column("Name")]
        public string FullName {get; set; }

        [Column(ignore:true)]
        public string IgnoreMe { get; set; }

        [Column("Amount")]
        public decimal Money { get; set; }
        
        public bool IsValid { get; set; }
        public long Eta { get; set; }
        public DateTime CreatedAt { get; set; }
        public Fruits Fruit { get; set; }

        [Column(ignore: true)]
        public decimal? Discount { get; set; }
    }
}