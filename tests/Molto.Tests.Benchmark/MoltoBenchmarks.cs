﻿using System.Linq;
using BenchmarkDotNet.Attributes;
using Molto.Abstractions;
using Molto.MsSql2014;
using Molto.PostgreSql;

namespace Molto.Tests.Benchmark
{
    [RPlotExporter, RankColumn]
    public class MoltoBenchmarks : BenchmarkBase
    {
        [GlobalSetup]
        public void Setup()
        {
            BaseSetup();
        }

        protected void BaseSetup()
        {
            i = 0;

            IDbConnectionProvider dbConnectionProvider = new InMemoryDbConnectionProvider();
            //dbConnectionProvider.AddConnectionFactory("default", new MsSql2014ConnectionMaker(ConnectionString));
            dbConnectionProvider.AddConnectionFactory("default", new PostgreSqlConnectionMaker(ConnectionString));
            IDbValueConverter dbValueConverter = new StrategiesDbValueConverter();
            IEntityDatabaseMapProvider entityDatabaseMapProvider = new EntityDatabaseMapProvider(new DirectPropertyEntityMapper());
            IDataReaderToPoco dataReaderToPoco = new DataReaderToPoco(entityDatabaseMapProvider);
            entityDatabaseMapProvider.AddMap<Post>();
            ISqlQueryCutter sqlQueryCutter = new SqlQueryCutter();
            ISqlQueryBuilder sqlQueryBuilder = new SqlQueryBuilder(entityDatabaseMapProvider, sqlQueryCutter);
            _db = new Db(dbConnectionProvider, dbValueConverter, dataReaderToPoco, sqlQueryBuilder);

        }

        [Benchmark(Description = "Query<T>")]
        public Post QueryBuffered()
        {
            Step();
            return _db.Query<Post>("select * from Posts where Id = @0",  i).First();
        }

        //[Benchmark(Description = "Query<dynamic> (buffered)")]
        //public dynamic QueryBufferedDynamic()
        //{
        //    Step();
        //    return _db.Query("select * from Posts where Id = @Id", new { Id = i }).First();
        //}

        //[Benchmark(Description = "Query<T> (unbuffered)")]
        //public Post QueryUnbuffered()
        //{
        //    Step();
        //    return _db.Query<Post>("select * from Posts where Id = @Id", new { Id = i }, buffered: false).First();
        //}

        //[Benchmark(Description = "Query<dynamic> (unbuffered)")]
        //public dynamic QueryUnbufferedDynamic()
        //{
        //    Step();
        //    return _connection.Query("select * from Posts where Id = @Id", new { Id = i }, buffered: false).First();
        //}

        //[Benchmark(Description = "QueryFirstOrDefault<T>")]
        //public Post QueryFirstOrDefault()
        //{
        //    Step();
        //    return _connection.QueryFirstOrDefault<Post>("select * from Posts where Id = @Id", new { Id = i });
        //}

        //[Benchmark(Description = "QueryFirstOrDefault<dynamic>")]
        //public dynamic QueryFirstOrDefaultDynamic()
        //{
        //    Step();
        //    return _connection.QueryFirstOrDefault("select * from Posts where Id = @Id", new { Id = i }).First();
        //}

        //[Benchmark(Description = "Contrib Get<T>")]
        //public Post ContribGet()
        //{
        //    Step();
        //    return _db.Get<Post>(i);
        //}
    }
}