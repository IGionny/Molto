using BenchmarkDotNet.Running;
using System;
using System.Data.SqlClient;
using static System.Console;

namespace Molto.Tests.Benchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            WriteLineColor("Warning: DEBUG configuration; performance may be impacted!", ConsoleColor.Red);
            WriteLine();
#endif
            WriteLine(
                "Welcome to Molto performance benchmark suite, based on BenchmarkDotNet - And Dapper Benchmarks.");
            WriteLine();

            WriteLine("Using ConnectionString: " + BenchmarkBase.ConnectionString);
            EnsureDBSetup();
            WriteLine("Database setup complete.");

            WriteLine("Iterations: " + BenchmarkBase.Iterations);
            new BenchmarkSwitcher(typeof(BenchmarkBase).Assembly).Run(args, new Config());

            Console.ReadLine();
        }

        private static void EnsureDBSetup()
        {
            using (var cnn = new SqlConnection(BenchmarkBase.ConnectionString))
            {
                cnn.Open();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = @"
If (Object_Id('Posts') Is Null)
Begin
	Create Table Posts
	(
		Id int identity primary key, 
        --Id UUID primary key, 
		[Text] varchar(max) not null, 
		CreationDate datetime not null, 
		LastChangeDate datetime not null,
		Counter1 int,
		Counter2 int,
		Counter3 int,
		Counter4 int,
		Counter5 int,
		Counter6 int,
		Counter7 int,
		Counter8 int,
		Counter9 int
	);
	   
	Set NoCount On;
	Declare @i int = 0;
	While @i <= 5001
	Begin
		--Insert Posts (Id, [Text],CreationDate, LastChangeDate) values (NEWID(), replicate('x', 2000), GETDATE(), GETDATE());
        Insert Posts ([Text],CreationDate, LastChangeDate) values (replicate('x', 2000), GETDATE(), GETDATE());
		Set @i = @i + 1;
	End
End
";
                cmd.Connection = cnn;
                cmd.ExecuteNonQuery();
            }
        }

        public static void WriteLineColor(string message, ConsoleColor color)
        {
            var orig = ForegroundColor;
            ForegroundColor = color;
            WriteLine(message);
            ForegroundColor = orig;
        }

        public static void WriteColor(string message, ConsoleColor color)
        {
            var orig = ForegroundColor;
            ForegroundColor = color;
            Write(message);
            ForegroundColor = orig;
        }
    }
}