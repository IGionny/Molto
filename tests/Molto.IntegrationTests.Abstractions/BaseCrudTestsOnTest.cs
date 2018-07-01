using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Molto.IntegrationTests.Abstractions
{
    public abstract class BaseCrudTestsOnTest : CrudTestsOnT<Test, Guid>
    {
        protected override string SingleFieldQuery => "SELECT name from Test";

        protected override void SingleFieldAssertion(IList<string> result, Test persisted)
        {
            result.Should().Contain(persisted.Name);
        }

        protected override Test WellknownData()
        {
            return DataGenerator.WellKnownTest();
        }

        [Fact]
        public override void Update()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var oldName = Guid.NewGuid();
                var item = WellknownData();
                item.Name = oldName.ToString();
                db.Insert(item);

                //Act
                item.Name = Guid.NewGuid().ToString();
                db.Update(item);

                //Act
                var resultQuery = db.Query<Test>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(1);
                resultQuery[0].Name.Should().Be(item.Name);
            }

        }

        protected override void CompareItem(Test item1, Test item2)
        {
            item1.Id.Should().Be(item2.Id);
            item1.Name.Should().Be(item2.Name);
            item1.Amount.Should().Be(item2.Amount);
            item1.CreatedAt.Year.Should().Be(item2.CreatedAt.Year);
            item1.CreatedAt.Month.Should().Be(item2.CreatedAt.Month);
            item1.CreatedAt.Day.Should().Be(item2.CreatedAt.Day);
            item1.CreatedAt.Hour.Should().Be(item2.CreatedAt.Hour);
            item1.CreatedAt.Minute.Should().Be(item2.CreatedAt.Minute);
            item1.CreatedAt.Second.Should().Be(item2.CreatedAt.Second);
            //item1.CreatedAt.Millisecond.Should().Be(item2.CreatedAt.Millisecond); //! this can be different
            item1.CreatedAt.Kind.Should().Be(item2.CreatedAt.Kind); //! Mssql return Unspecified
            item1.Eta.Should().Be(item2.Eta);
            item1.PrivacyAccepted.Should().Be(item2.PrivacyAccepted);
            item1.Fruit.Should().Be(item2.Fruit);
        }
    }
}