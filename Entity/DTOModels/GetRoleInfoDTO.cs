using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetRoleInfoDTO
    {
            public string Id { get; set; }
            public string RoleName { get; set; }
            public string Description { get; set; }
            public string CreateTime { get; set; }
            public string IsDelete { get; set; }
            public string DeleteTime { get; set; }

    }
}
