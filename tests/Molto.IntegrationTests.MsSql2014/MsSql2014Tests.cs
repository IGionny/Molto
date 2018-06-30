using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MsSql2014;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014Factory
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDbValueConverter _dbValueConverter;
        private readonly IDataReaderToPoco _dataReaderToPoco;
        private readonly ISqlQueryBuilder _sqlQueryBuilder;

        public MsSql2014Factory(string connectionString)
        {
            _dbConnectionProvider = new InMemoryDbConnectionProvider();
            _dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(connectionString));
            _dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider();
            _dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Test>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            _sqlQueryBuilder = new MsSql2014SqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
        }

        public IDb Db()
        {
            var db = new Db(_dbConnectionProvider, _dbValueConverter, _dataReaderToPoco, _sqlQueryBuilder);
            return db;
        }
    }

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

        private MsSql2014Factory _factory;

        public MsSql2014Factory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new MsSql2014Factory("Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;");
                }

                return _factory;
            }
        }


        protected override IDb MakeDb()
        {
            return Factory.Db();
        }
    }
}