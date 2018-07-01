using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Molto.IntegrationTests.Abstractions
{
    public abstract class BaseCrudTestsOnAlias : CrudTestsOnT<Alias, Guid>
    {
        protected override string SingleFieldQuery => "SELECT name from Test";

        protected override void SingleFieldAssertion(IList<string> result, Alias persisted)
        {
            result.Should().Contain(persisted.FullName);
        }

        protected override Alias WellknownData()
        {
            return DataGenerator.WellKnownAlias();
        }

        [Fact]
        public override void Update()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var oldName = Guid.NewGuid();
                var item = WellknownData();
                item.FullName = oldName.ToString();
                db.Insert(item);

                //Act
                item.FullName = Guid.NewGuid().ToString();
                db.Update(item);

                //Act
                var resultQuery = db.Query<Test>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(1);
                resultQuery[0].Name.Should().Be(item.FullName);
            }

        }

        protected override void CompareItem(Alias item1, Alias item2)
        {
            item1.Id.Should().Be(item2.Id);
            item1.FullName.Should().Be(item2.FullName);
        }
    }
}