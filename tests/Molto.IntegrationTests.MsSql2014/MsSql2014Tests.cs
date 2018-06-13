using System;
using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MsSql2014;
using Xunit;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014Tests
    {

        [Fact]
        public void Query_EmptySql()
        {
            //Arrange
            string connectionString = "";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IDataReaderToPoco dataReaderToPoco = null; //to finish
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);

            //Act
            var result = db.Query<Test>("");

            //Assert
        }

    }
}
