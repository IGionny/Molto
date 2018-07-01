using Molto.Abstractions;
using Molto.IntegrationTests.Abstractions;
using Molto.MapByAttributes;
using Molto.MsSql2014;

namespace Molto.IntegrationTests.MsSql2014
{
    public class MsSql2014TestsOnAlias : BaseCrudTestsOnAlias
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
                    _factory = new MsSql2014Factory("Data Source=.\\;Initial Catalog=tests;User Id=test;Password=test;Trusted_Connection=False;", new MapByAttributeEntityMapper());
                }

                return _factory;
            }
        }


        protected override IDb MakeDb()
        {
            return Factory.Db();
        }

        protected override IEntityDatabaseMapProvider _mapProvider => Factory.EntityDatabaseMapProvider;
    }
}