using System;
using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.PostgreSql;

namespace Molto.IntegrationTests.PostgreSql
{
    public class PostgreSqlTests : BaseCrudTests
    {
        private string _createTableSql =
            @"CREATE TABLE IF NOT EXISTS test (
	            id UUID NOT NULL PRIMARY KEY,
	            name varchar(255) NOT NULL,
	            amount decimal(10, 5) NOT NULL,
	            isvalid boolean NOT NULL,
	            eta bigint NOT NULL,
	            createdat timestamp NOT NULL,
	            privacyaccepted smallint NULL,
	            discount decimal(10,5) NULL,
	            employees bigint NULL,
	            confirmedat timestamp NULL,
	            fruit smallint NULL
            );";

        protected override IDb MakeDb()
        {
            var connectionString =
                "host=192.168.1.11;user id=postgres;password=Tests;Application Name=MoltoTests;database=Test";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new PostgreSqlConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new PostgreSqlDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new PostgreSqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
            return db;
        }
    }
}