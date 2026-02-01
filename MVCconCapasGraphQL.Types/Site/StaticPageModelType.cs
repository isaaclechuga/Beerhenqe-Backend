using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class StaticPageModelType : ObjectGraphType<StaticPageModel>
    {
        public StaticPageModelType()
        {
            Field(x => x.PageCode);
            Field(x => x.Title);
            Field(x => x.Body);
        }
    }
}
