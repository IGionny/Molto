﻿using System;
using System.Data.SqlClient;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using Molto.Abstractions;
using Molto.Tests.Benchmark.Helpers;

namespace Molto.Tests.Benchmark
{
    //[OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [Config(typeof(Config))]
    public abstract class BenchmarkBase
    {
        public const int Iterations = 50;
        protected static readonly Random _rand = new();
        protected SqlConnection _connection;
        public static string ConnectionString { get; } = ""; //"Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;";
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
            AddLogger(ConsoleLogger.Default);

            AddExporter(CsvExporter.Default);
            AddExporter(MarkdownExporter.GitHub);
            AddExporter(HtmlExporter.Default);

            var md = MemoryDiagnoser.Default;
            AddDiagnoser(md);
            AddColumn(new ORMColumn());
            AddColumn(TargetMethodColumn.Method);
            AddColumn(new ReturnColumn());
            AddColumn(StatisticColumn.Mean);
            AddColumn(StatisticColumn.StdDev);
            AddColumn(StatisticColumn.Error);
            AddColumn(BaselineRatioColumn.RatioMean);
            AddColumnProvider(DefaultColumnProviders.Metrics);

            AddJob(Job.ShortRun
                .WithLaunchCount(1)
                .WithWarmupCount(2)
                .WithUnrollFactor(BenchmarkBase.Iterations)
                .WithIterationCount(10)
            );
            Orderer = new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest);
            Options |= ConfigOptions.JoinSummary;
        }
    }
}