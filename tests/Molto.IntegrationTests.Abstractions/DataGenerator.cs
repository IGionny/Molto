using System;

namespace Molto.IntegrationTests.Abstractions
{
    public static class DataGenerator
    {
        public static Test WellKnownTest(Guid? id = null)
        {
            var item = new Test();
            item.Id = id ?? Guid.NewGuid();
            item.Name = "A Nice Test";
            item.Amount = 43434.43M;
            item.CreatedAt = new DateTime(2018, 06, 15, 15, 12, 23, DateTimeKind.Utc);
            item.Eta = 43324234;

            return item;
        }

        public static Alias WellKnownAlias(Guid? id = null)
        {
            var item = new Alias();
            item.Id = id ?? Guid.NewGuid();
            item.FullName = "A Nice Test";
            item.IgnoreMe = Guid.NewGuid().ToString();
            item.Money = 43434.43M;
            item.CreatedAt = new DateTime(2018, 06, 15, 15, 12, 23, DateTimeKind.Utc);
            item.Discount = 343242M;
            return item;
        }
    }
}