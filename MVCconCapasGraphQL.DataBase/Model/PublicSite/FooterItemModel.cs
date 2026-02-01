using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class FooterItemModel : GeneralResponse
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public int OrderIndex { get; set; }
    }
}
