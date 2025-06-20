using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    public class R_UserInfo_RoleInfoDal : BaseDal<IR_UserInfo_RoleInfo>,IR_UserInfo_RoleInfoDal
    {
        


        public R_UserInfo_RoleInfoDal(RepositoryDBContext dbcontext) :base(dbcontext)
        {
            
        }


        #region 没有用共用类之前的代码
        ///// <summary>
        ///// 添加用户角色
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool CreateUserRoleInfo(R_UserInfo_RoleInfo userRoleInfo)
        //{
        //    _dbcontext.R_UserInfo_RoleInfo.Add(userRoleInfo);
        //    return _dbcontext.SaveChanges()>0;
        //}
        ///// <summary>
        ///// 真删除
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool Remove(R_UserInfo_RoleInfo userRoleInfo)
        //{
        //    _dbcontext.R_UserInfo_RoleInfo.Remove(userRoleInfo);
        //     return _dbcontext.SaveChanges() > 0;
        //}
        ///// <summary>
        ///// 根据id获取用户角色信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public R_UserInfo_RoleInfo GetRUserRoleInfoById(string id)
        //{            
        //    return _dbcontext.R_UserInfo_RoleInfo.FirstOrDefault(u=>u.Id==id);
        //}

        ///// <summary>
        ///// 获取所有角色信息
        ///// </summary>
        ///// <returns></returns>
        //public DbSet<R_UserInfo_RoleInfo> GetUserRoleInfos()
        //{
        //    return _dbcontext.R_UserInfo_RoleInfo;
        //}
        ///// <summary>
        ///// 更新用户角色信息
        ///// </summary>
        ///// <param name="userRoleInfo"></param>
        ///// <returns></returns>
        //public bool UpdateUserRoleInfo(R_UserInfo_RoleInfo userRoleInfo)
        //{
        //    _dbcontext.R_UserInfo_RoleInfo.Update(userRoleInfo);
        //    return _dbcontext.SaveChanges() > 0;
        //}
        #endregion


    }
}
