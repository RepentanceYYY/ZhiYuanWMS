using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class RoleInfo:BaseDeleteEntity
    {
        
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
