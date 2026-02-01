using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class HomeContentModelType : ObjectGraphType<HomeContentModel>
    {
        public HomeContentModelType()
        {
            Field(x => x.Title);
            Field(x => x.Subtitle);
            Field(x => x.Body);
            Field(x => x.ImageUrl);
        }
    }
}
