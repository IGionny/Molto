using Molto.Abstractions;

namespace Molto
{
    public abstract class BaseDbFactory : IDbFactory
    {
        private readonly IDataReaderToPoco _dataReaderToPoco;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private readonly IDbValueConverter _dbValueConverter;
        private readonly ISqlQueryBuilder _sqlQueryBuilder;
        private readonly IEntityDatabaseMapProvider _entityDatabaseMapProvider;
        private readonly ISqlQueryCutter _sqlQueryCutter;

        public virtual IDb Db()
        {
            return new Db(_dbConnectionProvider, _dbValueConverter, _dataReaderToPoco, _sqlQueryBuilder);
        }
    }
}