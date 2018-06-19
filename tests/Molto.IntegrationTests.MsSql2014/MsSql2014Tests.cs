using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MsSql2014;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014Tests : BaseCrudTests
    {
        private string _createTableSql =
            "CREATE TABLE [dbo].[Test](\r\n\t[Id] [uniqueidentifier] NOT NULL,\r\n\t[Name] [nvarchar](255) NOT NULL,\r\n\t[Amount] [decimal](19, 5) NOT NULL,\r\n\t[IsValid] [bit] NOT NULL,\r\n\t[Eta] [bigint] NOT NULL,\r\n\t[CreatedAt] [datetime] NOT NULL,\r\n\t[PrivacyAccepted] [bit] NULL,\r\n\t[Discount] [decimal](19, 5) NULL,\r\n\t[Employees] [bigint] NULL,\r\n\t[ConfirmedAt] [datetime] NULL,\r\n CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n)";

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
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider);
            var db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);
            return db;
        }
    }
}