using System;

namespace Molto.IntegrationTests.Abstractions
{
    public class Test
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsValid { get; set; }
        public long Eta { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
