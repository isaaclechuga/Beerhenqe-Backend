using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MVCconCapasGraphQL.DataAccess.Repositories.Contracts;
using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Connections;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataAccess.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly string _cs;

        public SiteRepository(IOptions<DbOptions> opts)
        {
            _cs = opts.Value.Beerhenqe;
        }

        private async Task<(GeneralResponse, List<T>)> Exec<T>(
            object param)
        {
            var meta = new GeneralResponse { Code = -100, Message = "not ok" };
            var items = new List<T>();

            using var conn = new SqlConnection(_cs);
            await conn.OpenAsync();

            using var multi = await conn.QueryMultipleAsync(
                "site.GetPublicSiteData",
                param,
                commandType: CommandType.StoredProcedure
            );

            var header = await multi.ReadFirstOrDefaultAsync<GeneralResponse>();
            if (header == null || header.Code != 200)
                return (meta, items);

            meta.Code = header.Code;
            meta.Message = header.Message;

            items = (await multi.ReadAsync<T>()).ToList();
            return (meta, items);
        }

        public Task<(GeneralResponse, List<NavigationItemModel>)>
            GetNavigationAsync(string lang)
            => Exec<NavigationItemModel>(new { Type = "NAVIGATION", Lang = lang });

        public Task<(GeneralResponse, List<BeerModel>)>
            GetBeersAsync(string lang)
            => Exec<BeerModel>(new { Type = "BEERS", Lang = lang });

        public Task<(GeneralResponse, List<BeerImageModel>)>
            GetBeerImagesAsync(string lang, int beerId)
            => Exec<BeerImageModel>(new { Type = "BEER_IMAGES", Lang = lang, BeerId = beerId });

        public Task<(GeneralResponse, List<HomeContentModel>)>
            GetHomeAsync(string lang)
            => Exec<HomeContentModel>(new { Type = "HOME", Lang = lang });

        public Task<(GeneralResponse, List<FeaturedBeerModel>)>
            GetFeaturedBeersAsync(string lang)
            => Exec<FeaturedBeerModel>(new { Type = "FEATURED_BEERS", Lang = lang });

        public Task<(GeneralResponse, List<FooterItemModel>)>
            GetFooterAsync(string lang)
            => Exec<FooterItemModel>(new { Type = "FOOTER", Lang = lang });

        public Task<(GeneralResponse, List<BannerModel>)>
            GetBannersAsync(string lang)
            => Exec<BannerModel>(new { Type = "BANNERS", Lang = lang });

        public Task<(GeneralResponse, List<SeoModel>)>
            GetSeoAsync(string lang)
            => Exec<SeoModel>(new { Type = "SEO", Lang = lang });

        public Task<(GeneralResponse, List<StaticPageModel>)>
            GetPageAsync(string lang, string pageCode)
            => Exec<StaticPageModel>(new { Type = "PAGE", Lang = lang, PageCode = pageCode });
    }
}
