﻿using System;
using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.PostgreSql;

namespace Molto.IntegrationTests.PostgreSql
{
    public class PostgreSqlTestsOnTest : BaseCrudTestsOnTest
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
                "host=192.168.170.21;port=7432;user id=postgres;password=Test;Application Name=MoltoTests;database=Test";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new PostgreSqlConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new PostgreSqlDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider(new DirectPropertyEntityMapper());
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new PostgreSqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            return new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
        }

        protected override IEntityDatabaseMapProvider _mapProvider => new EntityDatabaseMapProvider(new DirectPropertyEntityMapper());
    }
}