using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCconCapasGraphQL.DataBase.Model.PublicSite
{
    public class HomeContentModel : GeneralResponse
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }

    }
}
