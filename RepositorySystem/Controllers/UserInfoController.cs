using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryBll;
using RepositorySystem.Filters;
using RepositorySystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace RepositorySystem.Controllers
{
    [CustomActionFilter]
    public class UserInfoController : Controller
    {
        IUserInfoBll _userInfoBll;
        IDepartmentInfoBll _departmentInfoBll;
        public UserInfoController(IUserInfoBll userInfoBll,IDepartmentInfoBll departmentInfoBll)
        {
            _userInfoBll = userInfoBll;
            _departmentInfoBll = departmentInfoBll;
        }
        /// <summary>
        /// 用户列表页
        /// </summary>
        /// <returns></returns>
        
        public IActionResult Index()
        {
            //如果不存在此用户名则转跳到登录页面
            //string userName = HttpContext.Session.GetString("userName");
            //if (string.IsNullOrEmpty(userName))
            //{
            //    return Redirect("/Account/LoginView");
            //}
            return View();
        }
        /// <summary>
        /// 用户添加页
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateUserInfoView()
        {
            return View();
        }
        public IActionResult CreateUserInfo(string account, string userName, string phoneNum, string email, string departmentId, int sex, string password)
        {
            CustomResultModel res = new CustomResultModel();
            //参数认证
            if (string.IsNullOrEmpty(account))
            {
                res.Msg = "账号不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(password))
            {
                res.Msg = "密码不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(userName))
            {
                res.Msg = "用户名不能为空";
                return Json(res);
            }
             string msg;

            bool isSuccess=_userInfoBll.CreateUserInfo(account, userName, phoneNum, email, departmentId, sex, password,out  msg);
            res.Msg = msg;
            res.IsSuccess = isSuccess;
            if (isSuccess)
            {
                res.Status = 1;
            }
            return Json(res);
        }
        /// <summary>
        /// 分页获取用户列表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IActionResult GetUserInfos(int page,int limit,string account,string userName)
        {
            #region 之前的显示方法


            //List<UserInfo> userInfos= _userInfoBll.GetUserInfos();
            //List<UserInfo> list = userInfos.OrderByDescending(u=>u.CreateTime).Skip((page-1)*limit).Take(limit).ToList();
            //var list1 = list.Select(u => new { 
            //    u.Id,
            //    u.UserName,
            //    u.Account,
            //    u.PhoneNum,
            //    u.Email,
            //    u.DepartmentId,
            //    CreateTime=u.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            //    Sex=u.Sex==0 ? "女":"男"
            //}).ToList();
            #endregion

            int count;
            //调用业务逻辑层的分页方法
            List<GetUserInfosDTO> list= _userInfoBll.GetUserInfosBypage(page, limit,account,userName, out count);
            object res = new
            {
                code = 0,
                msg = "",
                count =count,
                data = list
            };
            return Json(res);
        }
        /// <summary>
        /// 软删除单个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteUserInfo(string id)
        {

            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "请选择要删除的用户";
            }
            bool result= _userInfoBll.DeleteUserInfo(id);
            if (result)
            {
                res.Msg = "删除成功";
                res.Status = 1;
                return Json(res);
            }
            else
            {
                res.Msg = "删除失败";
                return Json(res);
            }
            
        }
        /// <summary>
        /// 编辑的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateUserInfoView()
        {
            return View();
        }
        /// <summary>
        /// 通过Id编辑的方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="phoneNum"></param>
        /// <param name="email"></param>
        /// <param name="sex"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateUserInfo(string id,string userName, string phoneNum, string email, int sex,string departmentId)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(userName))
            {
                res.Msg = "姓名不能为空";
                return Json(res);
            }
            bool result = _userInfoBll.UpdateUserInfo(id, userName,  phoneNum,  email,  sex,  departmentId);
            if (result)
            {
                res.Msg = "编辑成功";
                res.Status = 1;
                res.IsSuccess = true;
                return Json(res);
            }
            else
            {
                res.Msg = "编辑失败";
                return Json(res);
            }
        }
        /// <summary>
        /// 通过Id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetUserInfo(string id)
        {
            
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "ID不能为空";
                return Json(res);
            }
            UserInfo userInfo = _userInfoBll.GetUserInfoById(id);
            if (userInfo==null)
            {
                res.Msg = "未查询到用户";
                return Json(res);
            }
            //获取部门下拉选项集
             var list= _departmentInfoBll.GetDepartmentOptions();
             
            res.Msg = "查询成功";
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                UserInfo = userInfo,
                Options=list
            };
            return Json(res);
        }
        /// <summary>
        /// 批量伪删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IActionResult DeleteUserInfos(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids != null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的用户";
                return Json(res);
            }
            bool result = _userInfoBll.DeleteUserInfos(ids);
            if (result)
            {
                res.Msg = "删除成功";
                res.Status = 1;
                res.IsSuccess = true;
                return Json(res);
            }
            else
            {
                res.Msg = "删除失败";
                return Json(res);
            }
        }
        /// <summary>
        /// 获取部门下拉选项集
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDepartmentOptions()
        {
            CustomResultModel res = new CustomResultModel();
            List<OptionsModel> list= _departmentInfoBll.GetDepartmentOptions();

            res.IsSuccess = true;
            res.Status = 1;
            res.Msg = "成功";
            res.Datas = list;
            return Json(res);
        }
    }
}