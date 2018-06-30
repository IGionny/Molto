using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.SQLite;

namespace Molto.IntegrationTests.SQLite
{
    public class SQLiteTests : BaseCrudTests
    {
        private readonly string _createTableTestSql = @"CREATE TABLE Test 
                            (id uniqueidentifier not null,
                            name varchar(255),
                            amount decimal(19,5),
                            isvalid bit,
                            eta bigint,
                            createdat datetime,
                            privacyaccepted bit NULL,
                            discount decimal(19,5) NULL,
                            employees bigint NULL,
                            confirmedat datetime null,
                            fruit smallint null
                            ); ";

        private string _dropTableTestSql = "DROP TABLE IF EXISTS Test";

        protected override IDb MakeDb()
        {
//            SQLiteConnection.CreateFile("MyDatabase.sqlite");

            var connectionString = "Data Source=:memory:;Version=3;";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new SQLiteConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new SQLiteSqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);

            db.Execute(_createTableTestSql);

            return db;
        }
    }
}