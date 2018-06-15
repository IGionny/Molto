using System;
using System.Linq;
using FluentAssertions;
using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MsSql2014;
using Xunit;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014Tests : BaseCrudTests
    {

        protected override IDb MakeDb()
        {
            string connectionString = "Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
            return db;
        }
        

    }
}
