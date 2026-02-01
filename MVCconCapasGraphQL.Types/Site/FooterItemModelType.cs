using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class FooterItemModelType : ObjectGraphType<FooterItemModel>
    {
        public FooterItemModelType()
        {
            Field(x => x.Id);
            Field(x => x.ParentId, nullable: true);
            Field(x => x.Label);
            Field(x => x.Url);
            Field(x => x.OrderIndex);
        }
    }
}
