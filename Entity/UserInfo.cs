﻿using System;

namespace Entity
{
    public class UserInfo:BaseDeleteEntity
    {
        public string Account { get; set; }
        public string PassWord { get; set; }
        public string UserName { get; set; }
        public int Sex { get; set; }
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string DepartmentId { get; set; }                
        public bool IsAdmin { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
