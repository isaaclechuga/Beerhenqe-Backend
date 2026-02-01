using MVCconCapasGraphQL.DataBase;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataAccess.Repositories.Contracts
{
    public interface ISiteRepository
    {
            Task<(GeneralResponse meta, List<BannerModel> items)> GetBannersAsync(string lang);
            Task<(GeneralResponse meta, List<BeerImageModel> items)> GetBeerImagesAsync(string lang, int beerId);
            Task<(GeneralResponse meta, List<BeerModel> items)> GetBeersAsync(string lang);
            Task<(GeneralResponse meta, List<FeaturedBeerModel> items)> GetFeaturedBeersAsync(string lang);
            Task<(GeneralResponse meta, List<FooterItemModel> items)> GetFooterAsync(string lang);
            Task<(GeneralResponse meta, List<HomeContentModel> items)> GetHomeAsync(string lang);
            Task<(GeneralResponse meta, List<NavigationItemModel> items)> GetNavigationAsync(string lang);
            Task<(GeneralResponse meta, List<SeoModel> items)> GetSeoAsync(string lang);
            Task<(GeneralResponse meta, List<StaticPageModel> items)> GetPageAsync(string lang, string pageCode);
    }
}