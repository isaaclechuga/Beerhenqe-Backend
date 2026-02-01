using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class SeoModel : GeneralResponse
    {
        public string PageCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string KeyWords { get; set; }
    }
}
