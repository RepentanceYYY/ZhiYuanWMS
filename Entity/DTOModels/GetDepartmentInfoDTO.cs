using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetDepartmentInfoDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string DepartmentName { get; set; }
        public string LeaderId { get; set; }
        public string LeaderName { get; set; }
        public string ParentId { get; set; }        
        public string ParentName { get; set; }        
        public string CreateTime { get; set; }
    }
}
