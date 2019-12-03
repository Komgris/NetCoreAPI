using Xunit;
using FluentAssertions;
namespace CIM.DAL.Implements.Tests
{
    public class DirectSqlRepositoryTests
    {

        [Fact]
        public void ExecuteReaderTest()
        {

            var repo = new DirectSqlRepository();
            var result = repo.ExecuteReader("select * from sites ", null);
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ExecuteNonQueryTest()
        {

            var repo = new DirectSqlRepository();
            repo.ExecuteNonQuery(@"INSERT INTO [dbo].[Sites]
           ([Name]
           ,[IsActive]
           ,[IsDelete]
           ,[CreatedBy],[CreatedAt]) VALUES ( 'test001' ,0 ,0 ,'test' , getDate())", null);
            var result = repo.ExecuteReader("select * from sites where name = 'test001'", null);
            result.Should().NotBeNullOrEmpty();

        }

    }
}