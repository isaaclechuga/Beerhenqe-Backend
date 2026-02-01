using Moq;
using MVCconCapasGraphQL.DataAccess.Repositories.Contracts;
using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MVCconCapasGraphQL.Tests.Repositories
{
    public class SiteRepositoryTests
    {
        [Fact]
        public async Task GetBeersAsync_ShouldReturnItems()
        {
            var mockRepo = new Mock<ISiteRepository>();

            mockRepo.Setup(r => r.GetBeersAsync("es"))
                .ReturnsAsync((
                    new GeneralResponse
                    {
                        Code = 200,
                        Message = "OK"
                    },
                    new List<BeerModel>
                    {
                        new BeerModel
                        {
                            Id = 1                           
                        }
                    }
                ));

            var repo = mockRepo.Object;

            var result = await repo.GetBeersAsync("es");

            Assert.Equal(200, result.meta.Code);
            Assert.NotNull(result.items);
            Assert.Single(result.items);
        }
    }
}
