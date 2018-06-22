using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Molto.IntegrationTests.Abstractions
{
    public abstract class BaseCrudTests
    {
        protected abstract IDb MakeDb();

        [Fact]
        public void Query()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = DataGenerator.WellKnownTest();
                db.Insert(item);

                //Act
                var result = db.Query<Test>("");

                //Assert
                result.Should().NotBeEmpty();
                var persistedItem = result.SingleOrDefault(x => x.Id == item.Id);
                CompareTestItem(persistedItem, item);

            }
        }

        [Fact]
        public void Delete()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = DataGenerator.WellKnownTest();
                db.Insert(item);

                //Act
                db.Delete(item);

                //Act
                var resultQuery = db.Query<Test>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(0);
            }
        }

        [Fact]
        public void Update()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var oldName = Guid.NewGuid();
                var item = DataGenerator.WellKnownTest();
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

        [Fact]
        public void Insert()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = DataGenerator.WellKnownTest();

                //Act
                var result = db.Insert(item);

                //Assert
                result.Should().Be(1);
                var resultQuery = db.Query<Test>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(1);
                CompareTestItem(resultQuery[0], item);
            }
        }

        [Fact]
        public void Count()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = DataGenerator.WellKnownTest();
                db.Insert(item);

                //Act
                var result = db.Count<Test>();

                //Assert
                result.Should().BeGreaterOrEqualTo(1);
            }
        }

        [Fact]
        public async Task Paged()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var items = new Test[10];
                for (var i = 0; i < 10; i++)
                {
                    var item = DataGenerator.WellKnownTest();
                    db.Insert(item);
                    items.Append(item);
                }
                
                //Act
                var result = await db.PageAsync<Test>(1, 5).ConfigureAwait(false);

                //Assert
                result.TotalItems.Should().Be(10);
                result.Items.Should().HaveCount(5);
                result.CurrentPage.Should().Be(1);
                result.ItemsPerPage.Should().Be(5);
                result.TotalPages.Should().Be(2);
            }
        }

        protected void CompareTestItem(Test item1, Test item2)
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