using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class BeerModel : GeneralResponse
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Style { get; set; }
        public decimal Abv { get; set; }
        public int Ibu { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
