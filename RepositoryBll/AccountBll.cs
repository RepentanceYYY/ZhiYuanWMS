using Common;
using Entity;
using IRepositoryBll;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryBll
{
    public class AccountBll : IAccountBll
    {
        IUserInfoBll _userInfoBll;
        RepositoryDBContext _DBcontext;
        public AccountBll(IUserInfoBll userInfoBll,RepositoryDBContext repositoryDBContext)
        {
            _userInfoBll = userInfoBll;
            _DBcontext = repositoryDBContext;
        }
        public bool UpdatePassword(string id, string oldpassword, string newpassword,out string msg)
        {
            
            
            UserInfo userInfo =_userInfoBll.GetUserInfoById(id);

            //原密码
            string md5password = MD5Helper.MD5Encrypt64(oldpassword);

            if (userInfo.PassWord != md5password)
            {
                msg = "原密码输入错误";
                return false;
            }
            //修改密码
            userInfo.PassWord = MD5Helper.MD5Encrypt64(newpassword);
            
            bool isSucess = _DBcontext.SaveChanges() > 0;
            if (isSucess)
            {
                msg = "修改成功in";
                return true;
            }
            else
            {
                msg = "修改失败";
                return false;
            }

        }
    }
}
