using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MsSql2014;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014Tests : BaseCrudTests
    {
        private string _createTableSql =
            @"CREATE TABLE [Test](
                [Id] [uniqueidentifier] NOT NULL,
                [Name] [nvarchar](255) NOT NULL,
                [Amount] [decimal](19, 5) NOT NULL,
                [IsValid] [bit] NOT NULL,
                [Eta] [bigint] NOT NULL,
                [CreatedAt] [datetime] NOT NULL,
                [PrivacyAccepted] [bit] NULL,
                [Discount] [decimal](19, 5) NULL,
                [Employees] [bigint] NULL,
                [ConfirmedAt] [datetime] NULL,
                [Fruit] [smallint] NULL,
                CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED ([Id] ASC)";

        protected override IDb MakeDb()
        {
            var connectionString =
                "Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;";
            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(connectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
            return db;
        }
    }
}