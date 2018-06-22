using System;
using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Validators;
using Molto.Abstractions;
using Molto.MsSql2014;
using Molto.Tests.Benchmark.Helpers;

namespace Molto.Tests.Benchmark
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [Config(typeof(Config))]
    public abstract class BenchmarkBase
    {
        public const int Iterations = 50;
        protected static readonly Random _rand = new Random();
        protected SqlConnection _connection;
        public static string ConnectionString { get; } = "Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;";
        protected int i;
        protected IDb _db;

       

        protected void Step()
        {
            i++;
            if (i > 5000) i = 1;
        }
    }

    public class Config : ManualConfig
    {
        public Config()
        {
            Add(ExecutionValidator.DontFailOnError);
            Add(new MemoryDiagnoser());
            Add(new ORMColum());
            Add(new ReturnColum());
            Add(JitOptimizationsValidator.DontFailOnError); // ALLOW NON-OPTIMIZED DLLS
            Add(Job.Default
                .WithUnrollFactor(BenchmarkBase.Iterations)
                //.WithIterationTime(new TimeInterval(500, TimeUnit.Millisecond))
                .WithLaunchCount(1)
                .WithWarmupCount(0)
                .WithTargetCount(5)
                .WithRemoveOutliers(true)
            );
        }
    }
}