using Common;
using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using RepositoryDal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryBll
{
    public class UserInfoBll: IUserInfoBll
    {
        IUserInfoDal _userInfoDal;
        IDepartmentInfoDal _departmentInfoDal;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userInfoDal"></param>
        public UserInfoBll(IUserInfoDal userInfoDal,IDepartmentInfoDal departmentInfoDal)
        {
            _userInfoDal = userInfoDal;
            _departmentInfoDal = departmentInfoDal;
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userName"></param>
        /// <param name="phoneNum"></param>
        /// <param name="email"></param>
        /// <param name="departmentId"></param>
        /// <param name="sex"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateUserInfo(string account, string userName, string phoneNum, string email, string departmentId, int sex, string password,out string msg)
        {

            //判断用户是否已存在
            bool exist = _userInfoDal.GetEntities().Any(x=>x.Account==account && !x.IsDelete);            
            if (exist)
            {
                msg = "用户名已存在";
                return false;
            }
            UserInfo userInfo = new UserInfo() {
                Id = Guid.NewGuid().ToString(),
                Account = account,
                UserName = userName,
                PhoneNum = phoneNum,
                Email = email,
                DepartmentId = departmentId,
                Sex = sex,
                PassWord = MD5Helper.MD5Encrypt64(password),
                CreateTime=DateTime.Now
            };
            if (_userInfoDal.CreateEntity(userInfo))
            {
                msg = "添加成功";
                return true;
            }
            else
            {
                msg = "添加失败";
                return false;
            }
             
        }
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserInfos()
        {
            DbSet<UserInfo> userInfos = _userInfoDal.GetEntities();
            return userInfos.ToList();
        }
        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUserInfo(string id)
        {
            return _userInfoDal.DeleteEntity(id);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetUserInfosDTO> GetUserInfosBypage(int page, int limit, string account, string userName, out int count)
        {
            #region 通过内连接的方式

            //内连接 linq 获取所有用户信息和部门
            //var datas =  (from u in _userInfoDal.GetUserInfos().Where(x => !x.IsDelete)
            //              join d in _departmentInfoDal.GetDepartmentInfos()
            //              on u.DepartmentId equals d.Id
            //              select new
            //              {
            //                  u.Id,
            //                  u.UserName,
            //                  u.Account,
            //                  u.PhoneNum,
            //                  u.Email,
            //                  u.DepartmentId,
            //                  u.CreateTime,
            //                  u.Sex,
            //                  d.DepartmentName
            //              });
            #endregion
            //左连接
            var datas = (from u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
                         join d in _departmentInfoDal.GetEntities().Where(d=>!d.IsDelete)
                         on u.DepartmentId equals d.Id
                         into tempUD
                         from dt in tempUD.DefaultIfEmpty()
                         select new
                         {
                             u.Id,
                             u.UserName,
                             u.Account,
                             u.PhoneNum,
                             u.Email,
                             u.DepartmentId,
                             u.CreateTime,
                             u.Sex,
                             dt.DepartmentName
                         });

            //IQueryable<UserInfo> userInfos= _userInfoDal.GetUserInfos().Where(x => x.IsDelete == false);

            if (!string.IsNullOrEmpty(account))
            {
                datas = datas.Where(x => x.Account.Contains(account));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                datas = datas.Where(x => x.UserName.Contains(userName));
            }

            count = datas.Count();

            var tmp = datas.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
            {
                x.Id,
                x.UserName,
                x.Account,
                x.PhoneNum,
                x.Email,
                x.DepartmentId,
                x.CreateTime,
                x.Sex,
                x.DepartmentName
            }).ToList();

            //转换数据格式
            var res = tmp.Select(x => {
                string createTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                string sex = x.Sex == 1 ? "男" : "女";
                return new GetUserInfosDTO
                {
                    Id=x.Id,
                    UserName = x.UserName,
                    Account = x.Account,
                    PhoneNum = x.PhoneNum,
                    Email = x.Email,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = x.DepartmentName,
                    CreateTime = createTime,
                    Sex = sex                    
                };
            });
            return res.ToList();
        }

        public bool UpdateUserInfo(string id, string userName, string phoneNum, string email, int sex, string departmentId)
        {
            UserInfo userInfo = _userInfoDal.GetEntityById(id);
            if (userInfo==null)
            {
                return false;
            }
            userInfo.UserName = userName;
            userInfo.PhoneNum = phoneNum;
            userInfo.Email = email;
            userInfo.Sex = sex;
            userInfo.DepartmentId = departmentId;

            return _userInfoDal.UpdateEntity(userInfo);


        }
        /// <summary>
        /// 批量伪删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteUserInfos(List<string> ids)
        {            
            return _userInfoDal.DeleteEntityByIds(ids);
        }
        /// <summary>
        /// 根据id获取单个用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoById(string id)
        {
            return _userInfoDal.GetEntityById(id);
        }

        /// <summary>
        /// 获取用户下拉选项数据集
        /// </summary>
        public List<OptionsModel> GetUserInfoOptions()
        {
            return _userInfoDal.GetEntities().Where(u => !u.IsDelete).Select(u => new OptionsModel
            {
                Value = u.Id,
                Title = u.UserName
            }).ToList();
        }

        public bool Login(string account, string password, out string msg, out string userName,out string userId)
        {
            UserInfo userInfo= _userInfoDal.GetEntities().FirstOrDefault(u=>u.Account==account &&!u.IsDelete);
            if (userInfo==null)
            {
                msg = "账号不存在";
                userName = "";
                userId = "";
                return false;
            }
            string md5password= MD5Helper.MD5Encrypt64(password);
            if (userInfo.PassWord !=md5password)
            {
                msg = "密码错误";
                userName = "";
                userId = "";
                return false;
            }
            else
            {
                msg = "登录成功";
                userName = userInfo.UserName;
                userId = userInfo.Id;
                return true;
            }

        }
    }
}
