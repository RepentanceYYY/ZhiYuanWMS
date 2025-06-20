using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryDal
{
    public interface IR_UserInfo_RoleInfoDal:IBaseDal<IR_UserInfo_RoleInfo>
    {


        #region 没有用公开类之前的代码
        ///// <summary>
        ///// 添加用户角色
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool CreateUserRoleInfo(R_UserInfo_RoleInfo userRoleInfo);
        ///// <summary>
        ///// 真删除
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool Remove(R_UserInfo_RoleInfo userRoleInfo);
        ///// <summary>
        ///// 根据id获取用户角色信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public R_UserInfo_RoleInfo GetRUserRoleInfoById(string id);
        ///// <summary>
        ///// 获取所有角色信息
        ///// </summary>
        ///// <returns></returns>
        //public DbSet<R_UserInfo_RoleInfo> GetUserRoleInfos();
        ///// <summary>
        ///// 更新用户角色信息
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool UpdateUserRoleInfo(R_UserInfo_RoleInfo userRoleInfo);
        #endregion

    }
}
