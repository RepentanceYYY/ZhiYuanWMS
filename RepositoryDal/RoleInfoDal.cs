using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    public class RoleInfoDal : BaseDeleteDal<RoleInfo>,IRoleInfoDal
    {
        RepositoryDBContext _dbcontext;
        public RoleInfoDal(RepositoryDBContext dbcontext):base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #region 未使用公开类前的代码的代码
        ///// <summary>
        ///// 添加角色
        ///// </summary>
        ///// <param name="roleInfo"></param>
        ///// <returns></returns>
        //public bool CreateRoleInfo(RoleInfo roleInfo)
        //{
        //    _dbcontext.RoleInfo.Add(roleInfo);
        //    return _dbcontext.SaveChanges()>0;
        //}

        //public bool DeleteRoleInfo(string id)
        //{
        //    RoleInfo roleInfo = GetRoleInfoById(id);
        //    if (roleInfo==null)
        //    {
        //        return false;
        //    }
        //    roleInfo.IsDelete = true;
        //    roleInfo.DeleteTime = DateTime.Now;
        //     return _dbcontext.SaveChanges() > 0;
        //}

        //public bool DeleteRoleInfos(List<string> ids)
        //{
        //    List<RoleInfo> list = _dbcontext.RoleInfo.Where(r => ids.Contains(r.Id)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in list)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    return _dbcontext.SaveChanges() > 0;
        //}

        //public RoleInfo GetRoleInfoById(string id)
        //{
        //    RoleInfo roleInfo = _dbcontext.RoleInfo.FirstOrDefault(r=>r.Id==id);
        //    return roleInfo;
        //}

        ///// <summary>
        ///// 获取所有角色信息
        ///// </summary>
        ///// <returns></returns>
        //public DbSet<RoleInfo> GetRoleInfos()
        //{
        //    return _dbcontext.RoleInfo;
        //}

        //public bool UpdateRoleInfo(RoleInfo roleInfo)
        //{
        //    _dbcontext.RoleInfo.Update(roleInfo);
        //    return _dbcontext.SaveChanges() > 0;
        //}
        #endregion

    }
}
