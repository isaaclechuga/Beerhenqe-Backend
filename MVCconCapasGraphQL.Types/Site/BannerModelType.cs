using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class BannerModelType : ObjectGraphType<BannerModel>
    {
        public BannerModelType()
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.SubTitle);
            Field(x => x.ImageUrl);
            Field(x => x.TargetUrl);
            Field(x => x.OrderIndex);
        }
    }
}
