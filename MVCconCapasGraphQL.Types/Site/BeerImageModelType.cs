using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using MVCconCapasGraphQL.DataBase.Model.PublicSite;

namespace MVCconCapasGraphQL.Types.Site
{
    public class BeerImageModelType : ObjectGraphType<BeerImageModel>
    {
        public BeerImageModelType()
        {
            Field(x => x.Id);
            Field(x => x.BeerId);
            Field(x => x.ImageUrlNeutral, nullable: true);
            Field(x => x.ImageUrlEs, nullable: true);
            Field(x => x.ImageUrlEn, nullable: true);
            Field(x => x.IsPrimary);
            Field(x => x.OrderIndex);
        }
    }
}
