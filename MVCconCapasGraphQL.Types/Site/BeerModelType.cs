using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class BeerModelType : ObjectGraphType<BeerModel>
    {
        public BeerModelType()
        {
            Field(x => x.Id);
            Field(x => x.Sku);
            Field(x => x.Style);
            Field(x => x.Abv);
            Field(x => x.Ibu);
            Field(x => x.Price);
            Field(x => x.Name);
            Field(x => x.Description);
            Field(x => x.ImageUrl);
        }
    }
}
