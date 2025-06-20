using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class DepartmentInfo:BaseDeleteEntity
    {
        public string Description { get; set; }
        public string DepartmentName { get; set; }
        public string LeaderId { get; set; }
        public string ParentId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
