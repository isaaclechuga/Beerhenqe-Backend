using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class BeerImageModel : GeneralResponse
    {
        public int Id { get; set; }
        public int BeerId { get; set; }
        public string? ImageUrlNeutral { get; set; }
        public string? ImageUrlEs { get; set; }
        public string? ImageUrlEn { get; set; }
        public bool IsPrimary { get; set; }
        public int OrderIndex { get; set; }
    }
}
