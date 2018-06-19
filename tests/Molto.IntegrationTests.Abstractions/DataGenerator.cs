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
    }
}