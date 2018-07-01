using Molto.Abstractions;

namespace Molto.MsSql2014
{
    public class MsSql2014Factory : IDbFactory
    {
        private readonly IDataReaderToPoco _dataReaderToPoco;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDbValueConverter _dbValueConverter;
        private readonly ISqlQueryBuilder _sqlQueryBuilder;
        private readonly IEntityDatabaseMapProvider _entityDatabaseMapProvider;

        public MsSql2014Factory(string connectionString, IEntityMapper entityMapper = null)
        {
            _dbConnectionProvider = new InMemoryDbConnectionProvider();
            _dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(connectionString));
            _dbValueConverter = new StrategiesDbValueConverter();
            _entityDatabaseMapProvider  =
                new EntityDatabaseMapProvider(entityMapper ?? new DirectPropertyEntityMapper());
            _dataReaderToPoco = new DataReaderToPoco(_entityDatabaseMapProvider);
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            _sqlQueryBuilder = new MsSql2014SqlQueryBuilder(_entityDatabaseMapProvider, sqlQueryCutter);
        }

        public IEntityDatabaseMapProvider EntityDatabaseMapProvider => _entityDatabaseMapProvider;

        public IDb Db()
        {
            var db = new Db(_dbConnectionProvider, _dbValueConverter, _dataReaderToPoco, _sqlQueryBuilder);
            return db;
        }
    }
}