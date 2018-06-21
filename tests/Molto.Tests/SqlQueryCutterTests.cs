using FluentAssertions;
using Xunit;

namespace Molto.Tests
{
    public class SqlQueryCutterTests
    {

        [Theory]
        [InlineData("SELECT * FROM TABLE", "FROM TABLE")]
        [InlineData("SELECT ID, Name FROM TABLE", "FROM TABLE")]
        [InlineData("SELECT ID, Name FROM TABLE WHERE ID > 5", "FROM TABLE WHERE ID > 5")]
        [InlineData("SELECT ID, Name FROM TABLE WHERE (SELECT ID FROM TABLE2 t2 WHERE TABLE.id = t2.tableId) ", "FROM TABLE WHERE (SELECT ID FROM TABLE2 t2 WHERE TABLE.id = t2.tableId)")]
        [InlineData("SELECT (Select id From Table2 LIMIT 1) as T2id FROM TABLE WHERE (SELECT ID FROM TABLE2 t2 WHERE TABLE.id = t2.tableId) ", "FROM TABLE WHERE (SELECT ID FROM TABLE2 t2 WHERE TABLE.id = t2.tableId)")]
        public void CutOutSelect(string origin, string expected)
        {
            //Arrange
            var service = new SqlQueryCutter();

            //Act
            var result = service.TrimSelectStart(origin);

            //Assert
            result.Should().Be(expected);
        }
    }
}