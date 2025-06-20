using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class MenuInfo:BaseDeleteEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int Sort { get; set; }
        public string Href { get; set; }
        public string ParentId { get; set; }
        public string Icon { get; set; }
        public string Target { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
