using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;

namespace IRepositoryBll
{
    public interface IUserInfoBll
    {
         bool CreateUserInfo(string account, string userName, string phoneNum, string email, string departmentId, int sex, string password, out string msg);
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <returns></returns>
         List<UserInfo> GetUserInfos();
        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteUserInfo(string id);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetUserInfosDTO> GetUserInfosBypage(int page, int limit, string account, string userName, out int count);
        bool Login(string account, string password, out string msg, out string userName, out string userId);
        bool UpdateUserInfo(string id, string userName, string phoneNum, string email, int sex, string departmentId);
        /// <summary>
        /// 批量伪删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool DeleteUserInfos(List<string> ids);

        UserInfo GetUserInfoById(string id);
        List<OptionsModel> GetUserInfoOptions();
    }
}
