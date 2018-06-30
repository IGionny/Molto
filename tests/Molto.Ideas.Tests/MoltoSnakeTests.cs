using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Molto.IntegrationTests.Abstractions;
using Molto.IntegrationTests.MsSql2014;
using Xunit;

namespace Molto.Ideas.Tests
{
    public class MoltoSnakeTests
    {

        [Fact]
        public async Task SnakeQueryAsync()
        {
            //Arrange
            var factory =
                new MsSql2014Factory(
                    "Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;");
            using (IDb db = factory.Db())
            {

                //Act
                var result = await "Employees > @0".Where(5).Timeout(6).Async<Test>(db).ConfigureAwait(false);

                //Assert
                
            }
        }

    }

    public class MoltoSnake
    {
        public string Where { get; set; }
        public object[] Args { get; set; }
        public int Timeout { get; set; }

        public MoltoSnake()
        {

        }
    }

    public static class MoltoSnakeExtensions
    {
        public static MoltoSnake Where(this string where, params object[] args)
        {
            var snake = new MoltoSnake();
            snake.Where = where;
            snake.Args = args;
            return snake;
        }

        public static MoltoSnake Timeout(this MoltoSnake snake, int timeout)
        {
            snake.Timeout = timeout;
            return snake;
        }

        public static async Task<IList<T>> Async<T>(this MoltoSnake snake, IDb db)
        {
            return await db.QueryAsync<T>(snake.Where, snake.Args).ConfigureAwait(false);
        }
    }
}
