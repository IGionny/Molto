using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using Dapper.Contrib.Extensions;
using System.Linq;
using Dapper;

namespace Molto.Tests.Benchmark
{
    public class DapperBenchmarks : BenchmarkBase
    {
        [GlobalSetup]
        public void Setup()
        {
            BaseSetup();
        }

        protected void BaseSetup()
        {
            i = 0;
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
        }

        // [Benchmark(Description = "Query<T> (buffered)")]
        [Benchmark(Description = "Query<T>")]
        public Post QueryBuffered()
        {
            Step();
            return _connection.Query<Post>("select * from Posts where Id = @Id", new { Id = i }, buffered: true).First();
        }

        //[Benchmark(Description = "Query<dynamic> (buffered)")]
        //public dynamic QueryBufferedDynamic()
        //{
        //    Step();
        //    return _connection.Query("select * from Posts where Id = @Id", new { Id = i }, buffered: true).First();
        //}

        //[Benchmark(Description = "Query<T> (unbuffered)")]
        //public Post QueryUnbuffered()
        //{
        //    Step();
        //    return _connection.Query<Post>("select * from Posts where Id = @Id", new { Id = i }, buffered: false).First();
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
        //    return _connection.Get<Post>(i);
        //}
    }
}