using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetMenuInfoDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Sort { get; set; }
        public string Href { get; set; }
        public string ParentTitle { get; set; }
        public string ParentName { get; set; }
        public string Icon { get; set; }
        public string Target { get; set; }
        public string CreateTime { get; set; }
        public string IsDelete { get; set; }
        public string DeleteTime { get; set; }
    }
}
