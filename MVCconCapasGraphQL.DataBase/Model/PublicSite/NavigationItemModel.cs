using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCconCapasGraphQL.DataBase;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class NavigationItemModel : GeneralResponse
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int OrderIndex { get; set; }
    }
}
