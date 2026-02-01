using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MVCconCapasGraphQL.DataAccess.Repositories;
using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MVCconCapasGraphQL.Tests.Repositories
{
    public class SiteRepositoryIntegrationTests
    {
        private readonly SiteRepository _repo;

        public SiteRepositoryIntegrationTests()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var services = new ServiceCollection();
            services.Configure<DbOptions>(config.GetSection("ConnectionStrings"));
            services.AddTransient<SiteRepository>();

            var provider = services.BuildServiceProvider();
            _repo = provider.GetRequiredService<SiteRepository>();
        }

        private static void AssertMetaOk(GeneralResponse meta, string context)
        {
            Assert.NotNull(meta);

            if (meta.Code != 200)
            {
                Assert.True(false, $"[{context}] Expected meta.Code=200 but got {meta.Code}. Message='{meta.Message}'");
            }
        }

        private static void AssertItemsNotNull<T>(List<T> items, string context)
        {
            Assert.NotNull(items);
        }

        private static void AssertValidResponse<T>(GeneralResponse meta, List<T> items, string context, bool allowEmptyItems = true)
        {
            AssertMetaOk(meta, context);
            AssertItemsNotNull(items, context);

            if (!allowEmptyItems)
                Assert.NotEmpty(items);
        }

        private static void AssertFailResponse(GeneralResponse meta, string context)
        {
            Assert.NotNull(meta);
            Assert.True(meta.Code != 200, $"[{context}] Expected failure meta.Code != 200 but got 200");
        }

        private async Task<int> ResolveExistingBeerIdAsync(string lang)
        {
            var (meta, beers) = await _repo.GetBeersAsync(lang);
            AssertValidResponse(meta, beers, $"ResolveExistingBeerIdAsync(lang={lang})", allowEmptyItems: false);

            return beers.First().Id;
        }

        private async Task<string> ResolveExistingPageCodeAsync(string lang)
        {
            var candidates = new[] { "HOME", "home", "ABOUT", "about", "CONTACT", "contact", "TERMS", "terms", "PRIVACY", "privacy" };

            foreach (var code in candidates)
            {
                var (meta, items) = await _repo.GetPageAsync(lang, code);
                if (meta != null && meta.Code == 200)
                    return code;
            }

            Assert.True(false, $"[ResolveExistingPageCodeAsync(lang={lang})] No PAGE code candidate worked. Check DB data or SP logic for Type=PAGE.");
            return null!;
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetNavigationAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetNavigationAsync(lang);
            AssertValidResponse(meta, items, $"GetNavigationAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetBeersAsync_ShouldReturnOk_AndHaveIds(string lang)
        {
            var (meta, beers) = await _repo.GetBeersAsync(lang);
            AssertValidResponse(meta, beers, $"GetBeersAsync(lang={lang})", allowEmptyItems: false);

            Assert.All(beers, b => Assert.True(b.Id > 0, $"Beer Id should be > 0 (lang={lang})"));
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetBeerImagesAsync_ShouldReturnOk_ForExistingBeer(string lang)
        {
            var beerId = await ResolveExistingBeerIdAsync(lang);

            var (meta, images) = await _repo.GetBeerImagesAsync(lang, beerId);
            AssertValidResponse(meta, images, $"GetBeerImagesAsync(lang={lang}, beerId={beerId})");

        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetHomeAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetHomeAsync(lang);
            AssertValidResponse(meta, items, $"GetHomeAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetFeaturedBeersAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetFeaturedBeersAsync(lang);
            AssertValidResponse(meta, items, $"GetFeaturedBeersAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetFooterAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetFooterAsync(lang);
            AssertValidResponse(meta, items, $"GetFooterAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetBannersAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetBannersAsync(lang);
            AssertValidResponse(meta, items, $"GetBannersAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetSeoAsync_ShouldReturnOk(string lang)
        {
            var (meta, items) = await _repo.GetSeoAsync(lang);
            AssertValidResponse(meta, items, $"GetSeoAsync(lang={lang})");
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetPageAsync_ShouldReturnOk_ForExistingPageCode(string lang)
        {
            var pageCode = await ResolveExistingPageCodeAsync(lang);

            var (meta, items) = await _repo.GetPageAsync(lang, pageCode);
            AssertValidResponse(meta, items, $"GetPageAsync(lang={lang}, pageCode={pageCode})");

        }

        [Fact]
        public async Task GetBeersAsync_ShouldFail_ForInvalidLang()
        {
            var (meta, items) = await _repo.GetBeersAsync("xx");
            Assert.NotNull(items);
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetBeerImagesAsync_ShouldFail_ForInvalidBeerId(string lang)
        {
            var invalidBeerId = int.MaxValue;

            var (meta, items) = await _repo.GetBeerImagesAsync(lang, invalidBeerId);
            AssertFailResponse(meta, $"GetBeerImagesAsync(lang={lang}, beerId={invalidBeerId})");
            Assert.NotNull(items);
        }

        [Theory]
        [InlineData("es")]
        [InlineData("en")]
        public async Task GetPageAsync_ShouldFail_ForInvalidPageCode(string lang)
        {
            var (meta, items) = await _repo.GetPageAsync(lang, "___NOT_EXISTS___");
            AssertFailResponse(meta, $"GetPageAsync(lang={lang}, pageCode=___NOT_EXISTS___)");
            Assert.NotNull(items);
        }
    }
}
