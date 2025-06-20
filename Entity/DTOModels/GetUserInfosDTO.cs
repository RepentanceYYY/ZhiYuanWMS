using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class GetUserInfosDTO
    {
        public string Id { get; set; }
        public string Account { get; set; }       
        public string UserName { get; set; }
        public string Sex { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CreateTime { get; set; }

        
    }
}
