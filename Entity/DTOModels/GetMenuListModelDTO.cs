using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetMenuListModelDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public string Target { get; set; }
        public List<GetMenuListModelDTO> Child { get; set; }

    }
}
