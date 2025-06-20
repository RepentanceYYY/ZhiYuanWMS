using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace IRepositoryDal
{
    public interface IUserInfoDal:IBaseDeleteDal<UserInfo>
    {
        #region 以前的代码

        //bool CreateUserInfo(UserInfo userInfo);

        //DbSet<UserInfo> GetUserInfos();

        //bool DeleteUserInfo(string id);

        ///// <summary>
        ///// 根据ID获取用户信息
        ///// </summary>
        ///// <param name="index"></param>
        ///// <returns></returns>
        //UserInfo GetUserInfoById(string index);
        //bool UpdateUserInfo(UserInfo userInfo);

        //bool GetUserInfoByIds(List<string> ids);
        #endregion


    }
}
