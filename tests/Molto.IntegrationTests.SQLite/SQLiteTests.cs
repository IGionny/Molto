using System;
using System.Data.SQLite;
using System.Linq;
using FluentAssertions;
using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.SQLite;
using Xunit;

namespace Molto.IntegrationTests.SQLite
{
    public class SQLiteTests
    {

        protected IDb MakeDb()
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");

            string connectionString = "Data Source=MyDatabase.sqlite;Version=3;";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new SQLiteConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
            return db;
        }

        [Fact]
        public void Query_CreateTable()
        {
            //Arrange
            using (var db = MakeDb())
            {
                string sql = "CREATE TABLE test (id uniqueidentifier not null, name varchar(255), amount decimal(18,5), isvalid bit, eta bigint, createdat timestamp) ";

                //Act
                var result = db.Execute(sql);

                //Assert
                result.Should().Be(0);
            }
        }

        [Fact]
        public void Query_EmptySql()
        {
            //Arrange
            using (var db = MakeDb())
            {

                //Act
                var result = db.Query<Test>("");

                //Assert
                result.Should().NotBeEmpty();
            }
        }

        [Fact]
        public void Insert()
        {
            //Arrange
            using (var db = MakeDb())
            {
                var item = new Test();
                item.Id = Guid.NewGuid();
                item.Name = Guid.NewGuid().ToString();
                item.Amount = 43434.43M;
                item.CreatedAt = DateTime.UtcNow;
                item.Eta = 43324234;

                //Act
                var result = db.Insert(item);

                //Assert
                result.Should().Be(1);
                var resultQuery = db.Query<Test>("WHERE id = @0", item.Id).ToList();
                resultQuery.Should().HaveCount(1);
                resultQuery[0].Id.Should().Be(item.Id);
                resultQuery[0].Name.Should().Be(item.Name);
                resultQuery[0].Amount.Should().Be(item.Amount);
                resultQuery[0].CreatedAt.Year.Should().Be(item.CreatedAt.Year);
                resultQuery[0].CreatedAt.Month.Should().Be(item.CreatedAt.Month);
                resultQuery[0].CreatedAt.Day.Should().Be(item.CreatedAt.Day);
                resultQuery[0].CreatedAt.Hour.Should().Be(item.CreatedAt.Hour);
                resultQuery[0].CreatedAt.Minute.Should().Be(item.CreatedAt.Minute);
                resultQuery[0].CreatedAt.Second.Should().Be(item.CreatedAt.Second);
                //resultQuery[0].CreatedAt.Millisecond.Should().Be(item.CreatedAt.Millisecond); //! this can be different
                //resultQuery[0].CreatedAt.Kind.Should().Be(item.CreatedAt.Kind); //! Mssql return Unspecified
                resultQuery[0].Eta.Should().Be(item.Eta);

            }
        }

    }
}
