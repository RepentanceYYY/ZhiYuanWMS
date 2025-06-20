using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem.Controllers
{
    public class RoleInfoController : Controller
    {
        IRoleInfoBll _roleInfoBll;
        IUserInfoBll _userInfoBll;
        IMenuInfoBll _menuInfoBll;
        public RoleInfoController(IRoleInfoBll roleInfoBll, IUserInfoBll userInfoBll, IMenuInfoBll menuInfoBll)
        {
            _roleInfoBll = roleInfoBll;
            _userInfoBll = userInfoBll;
            _menuInfoBll = menuInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IActionResult GetRoleInfos(int limit,int page,string roleName)
        {
            int count;
            List<GetRoleInfoDTO> list= _roleInfoBll.GetRoleInfosByPage(limit,page,roleName,out count);

            object res = new
            {
                code=0,
                msg="",
                count=count,
                data=list
            };
            return Json(res);
        }
        /// <summary>
        /// 添加的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateRoleInfoView()
        {
            return View();
        }
        public IActionResult CreateRoleInfo(string roleName,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(roleName))
            {
                res.Msg = "角色名不能为空";
                return Json(res);
            }
            string msg;
            bool isSuccess= _roleInfoBll.CreateRoleInfo(roleName,description,out msg);
            res.Msg = msg;
            if (isSuccess)
            {
                res.Status = 1;
                res.IsSuccess = true;                
            }
            return Json(res);
        }
        /// <summary>
        /// 编辑的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateRoleInfoView()
        {
            return View();
        }
        /// <summary>
        /// 通过Id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetRoleInfoById(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "id不能为空";
                return Json(res);
            }
            RoleInfo roleInfo= _roleInfoBll.GetRoleInfoById(id);

            if (roleInfo==null)
            {
                res.Msg = "获取角色信息失败";
                
            }
            else
            {
                res.Msg = "获取成功";
                res.Datas = roleInfo;
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);
        }
        /// <summary>
        /// 编辑的方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateRoleInfo(string id,string roleName,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(roleName))
            {
                res.Msg = "角色名不能为空";
                return Json(res);
            }
            bool Result= _roleInfoBll.UpdateRoleInfo(id,roleName,description);
            
            if (Result)
            {
                res.Msg = "修改成功";
                res.IsSuccess = true;
                res.Status = 1;
            }
            else
            {
                res.Msg = "修改失败";
            }
            return Json(res);
        }
        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteRoleInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "编号不能为空";
                return Json(res);
            }
            bool result= _roleInfoBll.DeleteRoleInfo(id);
            if (result)
            {
                res.Msg = "删除成功";
                res.IsSuccess = true;
                res.Status = 1;
            }
            else
            {
                res.Msg = "删除失败";
            }
            return Json(res);
        }
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IActionResult DeleteRoleInfos(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids !=null && ids.Count<=0)
            {
                res.Msg = "请选择要删除的角色";
                return Json(res);
            }
            bool result = _roleInfoBll.DeleteRoleInfos(ids);
            if (result)
            {
                res.Msg = "删除成功";
                res.Status = 1;
                res.IsSuccess = true;
            }
            else
            {
                res.Msg = "删除失败";
            }
            return Json(res);
        }
        /// <summary>
        /// 绑定用户到角色的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult BindUserInfoView()
        {
            return View();
        }
        /// <summary>
        /// 绑定用户到角色的方法
        /// </summary>
        /// <returns></returns>
        public IActionResult BindUserInfo(string roleId, List<string> userIds)
        {
            //返回值模板对象
            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(roleId))
            {
                res.Msg = "角色id为空";
                return Json(res);
            }

            if (userIds != null && userIds.Count <= 0)
            {
                res.Msg = "请选择用户";
                return Json(res);
            }

            //绑定用户
            bool isSuccess = _roleInfoBll.BindUserInfo(roleId, userIds);
            res.Msg = "绑定失败";
            if (isSuccess)
            {
                res.IsSuccess = true;
                res.Status = 1;
                res.Msg = "绑定成功";
            }

            return Json(res);
        }

        /// <summary>
        /// 通过角色Id和菜单数据进行绑定的方法
        /// </summary>
        /// <returns></returns>
        public IActionResult BindMenuInfo(string roleId, List<string> menuIds)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(roleId))
            {
                res.Msg = "角色Id不能为空";
                return Json(res);
            }
            if (menuIds !=null && menuIds.Count<=0)
            {
                res.Msg = "请选择菜单";
                return Json(res);
            }

            //绑定菜单
            bool Result = _roleInfoBll.BindMenuInfo(roleId, menuIds);

            res.Msg = "绑定失败";
            if (Result)
            {
                res.Msg = "绑定成功";
                res.Status = 1;
                res.IsSuccess = true;
            }
            return Json(res);
        }

        /// <summary>
        /// 获取备选用户数据和已绑定角色id的方法
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IActionResult GetBindUserOptions(string roleId)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(roleId))
            {
                res.Msg = "角色id为空";
                return Json(res);
            }

            //获取用户备选数据
            List<OptionsModel> options = _userInfoBll.GetUserInfoOptions();

            //获取角色已绑定用户id集合
            List<string> bindUserIds = _roleInfoBll.GetBindUserIds(roleId);

            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                options,
                bindUserIds
            };
            return Json(res);
        }

        /// <summary>
        /// 绑定菜单到角色的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult BindMenuInfoView()
        {
            return View();
        }

        /// <summary>
        /// 获取备选菜单数据和已绑定数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IActionResult GetBindMenuOptions(string roleId)
        {
            //返回模板
            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(roleId))
            {
                res.Msg = "角色Id不能为空";
                return Json(res);
            }
            //获取菜单备选数据
            List<OptionsModel> options = _menuInfoBll.GetmenuInfoOptions();
            //获取角色已绑定菜单id的集合
            List<string> bindMenuIds = _roleInfoBll.GetBindMenuIds(roleId);
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                options,
                bindMenuIds
            };
            return Json(res);
        }
    }
}
