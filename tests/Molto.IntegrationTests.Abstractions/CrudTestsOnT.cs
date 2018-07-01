using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Molto.Abstractions;
using Xunit;

namespace Molto.IntegrationTests.Abstractions
{
    public abstract class CrudTestsOnT<T,TId> where T: IEntity<TId>
    {
        protected abstract IDb MakeDb();
        protected abstract IEntityDatabaseMapProvider _mapProvider { get; }



        [Fact]
        public void ReconnectConnection_OnClose()
        {
            //Arrange
            using (var db = (Db)MakeDb())
            {
                db.Connection.State.Should().Be(ConnectionState.Open);
                var item = WellknownData();
                db.Insert(item);

                db.Connection.Close();
                
                //Act
                db.Query<T>("");
                db.Connection.State.Should().Be(ConnectionState.Open);

                //Assert
            }
        }

        [Fact]
        public void Query()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();
                db.Insert(item);

                //Act
                var result = db.Query<T>("");

                //Assert
                result.Should().NotBeEmpty();
                var persistedItem = result.SingleOrDefault(x => x.Id.Equals(item.Id));
                CompareItem(persistedItem, item);

            }
        }

        [Fact]
        public async Task QueryAsync()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();
                db.Insert(item);

                //Act
                var result = await db.QueryAsync<T>(null).ConfigureAwait(false);

                //Assert
                result.Should().NotBeEmpty();
                var persistedItem = result.SingleOrDefault(x => x.Id.Equals(item.Id));
                CompareItem(persistedItem, item);

            }
        }

        protected abstract string SingleFieldQuery { get; }
        protected abstract void SingleFieldAssertion(IList<string> result, T persisted);

        [Fact]
        public void Query_SingleField()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();
                db.Insert(item);

                //Act
                var result = db.Query<string>(SingleFieldQuery);

                //Assert
                result.Should().NotBeEmpty();
                SingleFieldAssertion(result.ToList(), item);
                
            }
        }

        [Fact]
        public void Delete()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();
                db.Insert(item);

                //Act
                db.Delete(item);

                //Act
                var resultQuery = db.Query<T>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(0);
            }
        }

        [Fact]
        public abstract void Update();

        [Fact]
        public void Insert()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();

                //Act
                var result = db.Insert(item);

                //Assert
                result.Should().Be(1);
                var resultQuery = db.Query<T>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(1);
                CompareItem(resultQuery[0], item);
            }
        }

        public virtual void CleanupTable()
        {
            var map = _mapProvider.Get<T>();
            using (var db = MakeDb())
            {
                db.Execute(Sql.Truncate + " " + map.Table);
            }
        }

        [Fact]
        public void Insert_Massive()
        {
            int elements = 1000;
            var prepareItems = new T[elements];
            for (var i = 0; i < elements; i++)
            {
                prepareItems[i] = WellknownData();
            }

            //Cleanup
            CleanupTable();

            //Arrange
            using (var db = MakeDb())
            {
                foreach (var prepareItem in prepareItems)
                {
                    db.Insert(prepareItem);
                }
            }

            using (var db = MakeDb())
            {
                db.Count<T>().Should().Be(elements);
            }
        }

        [Fact]
        public void Count()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = WellknownData();
                db.Insert(item);

                //Act
                var result = db.Count<T>();

                //Assert
                result.Should().BeGreaterOrEqualTo(1);
            }
        }


        protected abstract T WellknownData();

        [Fact]
        public void Paged()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var items = new T[10];
                for (var i = 0; i < 10; i++)
                {
                    var item = WellknownData(); 
                    db.Insert(item);
                    items[i] = item;
                }

                //Act
                var result = db.Page<T>(1, 5, "ORDER BY Id");

                //Assert
                result.TotalItems.Should().BeGreaterOrEqualTo(10);
                result.Items.Should().HaveCount(5);
                result.CurrentPage.Should().Be(1);
                result.ItemsPerPage.Should().Be(5);
                result.TotalPages.Should().BeGreaterOrEqualTo(2);
            }
        }

        [Fact]
        public void Single()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var items = new T[10];
                for (var i = 0; i < 10; i++)
                {
                    var item = WellknownData();
                    db.Insert(item);
                    items[i] = item;
                }

                //Act
                var result = db.Single<T>(Sql.Where + " id = @0 ", items[0].Id);

                //Assert
                result.Should().NotBeNull();
            }
        }

        protected abstract void CompareItem(T item1, T item2);
    }
}