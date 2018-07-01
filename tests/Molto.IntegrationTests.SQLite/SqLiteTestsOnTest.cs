using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.SQLite;

namespace Molto.IntegrationTests.SQLite
{
    public class SqLiteTestsOnTest : BaseCrudTestsOnTest
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
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider(new DirectPropertyEntityMapper());
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new SQLiteSqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);

            db.Execute(_createTableTestSql);

            return db;
        }

        public override void CleanupTable()
        {
            var map = _mapProvider.Get<Test>();
            using (var db = MakeDb())
            {
                //SqlLite does not provide TRUNCATE

                db.Execute(Sql.Delete + " FROM " + map.Table);
            }
        }

        protected override IEntityDatabaseMapProvider _mapProvider => new EntityDatabaseMapProvider(new DirectPropertyEntityMapper());
    }
}