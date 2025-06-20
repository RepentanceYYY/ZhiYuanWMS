using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryDal
{
    public class UserInfoDal: BaseDeleteDal<UserInfo>,IUserInfoDal
    {
        RepositoryDBContext _DBContext;
        public UserInfoDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {
            _DBContext = repositoryDBContext;
        }
        #region 以前的代码


        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="userInfo"></param>
        ///// <returns></returns>
        //public bool CreateUserInfo(UserInfo userInfo)
        //{
        //    _DBContext.UserInfo.Add(userInfo);
        //    return _DBContext.SaveChanges()>0;
        //}
        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <returns></returns>
        //public DbSet<UserInfo> GetUserInfos()
        //{
        //    return _DBContext.UserInfo;
        //}

        //public bool DeleteUserInfo(string id)
        //{
        //    //UserInfo userInfo = _DBContext.UserInfo.FirstOrDefault(a=>a.Id==id.ToString());
        //    //_DBContext.UserInfo.Remove(userInfo);

        //    UserInfo userInfo = _DBContext.UserInfo.FirstOrDefault(x => x.Id == id);
        //    userInfo.IsDelete = true;
        //    userInfo.DeleteTime = DateTime.Now;
        //    return _DBContext.SaveChanges()>0;
        //}

        ///// <summary>
        ///// 根据ID获取用户信息
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //public UserInfo GetUserInfoById(string index)
        //{
        //    UserInfo userInfo = _DBContext.UserInfo.FirstOrDefault(u=>u.Id==index);
        //    return userInfo;
        //}

        //public bool UpdateUserInfo(UserInfo userInfo)
        //{
        //    _DBContext.UserInfo.Update(userInfo);
        //    return _DBContext.SaveChanges()>0;
        //}

        //public bool GetUserInfoByIds(List<string> ids)
        //{
        //    List<UserInfo> userInfo = _DBContext.UserInfo.Where(x => ids.Contains(x.Id)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in userInfo)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    return _DBContext.SaveChanges() > 0;
        //}
        #endregion
    }
}
