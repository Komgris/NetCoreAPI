using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;

namespace CIM.DAL.Implements.Tests
{
    public class DirectSqlRepositoryTests
    {
        Dictionary<string, string> InMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:CIMDatabase", "Server=103.70.6.198;Initial Catalog=cim_db;Persist Security Info=False;User ID=cim;Password=4dev@psec;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"},
        };


        [Fact]
        public void ExecuteReaderTest()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(InMemorySettings)
            .Build();
            var repo = new DirectSqlRepository(configuration);
            var result = repo.ExecuteReader("select * from sites ", null);
            result.Should().NotBeNullOrEmpty();
        }

        //[Fact]
        //public void ExecuteNonQueryTest()
        //{
        //    IConfiguration configuration = new ConfigurationBuilder()
        //    .AddInMemoryCollection(InMemorySettings)
        //    .Build();

        //    var repo = new DirectSqlRepository(configuration);
        //    repo.ExecuteNonQuery(@"INSERT INTO [dbo].[Sites]
        //   ([Name]
        //   ,[IsActive]
        //   ,[IsDelete]
        //   ,[CreatedBy],[CreatedAt]) VALUES ( 'test001' ,0 ,0 ,'test' , getDate())", null);
        //    var result = repo.ExecuteReader("select * from sites where name = 'test001'", null);
        //    result.Should().NotBeNullOrEmpty();

        //}

    }
}