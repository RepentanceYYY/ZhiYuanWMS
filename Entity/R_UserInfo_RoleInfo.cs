using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class IR_UserInfo_RoleInfo:BaseEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
