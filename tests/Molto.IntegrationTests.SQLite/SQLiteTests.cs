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
    public class SQLiteTests : BaseCrudTests
    {

        private string _createTableTestSql = "CREATE TABLE Test (id uniqueidentifier not null, name varchar(255), amount decimal(18,5), isvalid bit, eta bigint, createdat datetime); ";
        private string _dropTableTestSql = "DROP TABLE IF EXISTS Test";

        public SQLiteTests()
        {

        }

        protected override IDb MakeDb()
        {
//            SQLiteConnection.CreateFile("MyDatabase.sqlite");

            string connectionString = "Data Source=:memory:;Version=3;";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new SQLiteConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);

            db.Execute(_createTableTestSql);

            return db;
        }

        
    }
}
