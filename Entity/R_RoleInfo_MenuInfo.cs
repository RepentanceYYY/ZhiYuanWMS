using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 菜单角色表
    /// </summary>
    public class R_RoleInfo_MenuInfo:BaseEntity
    {
        public string RoleId { get; set; }
        public string MenuId { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
