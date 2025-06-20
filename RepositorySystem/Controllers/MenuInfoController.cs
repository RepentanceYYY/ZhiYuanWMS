using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System.Collections.Generic;

namespace RepositorySystem.Controllers
{
    public class MenuInfoController : Controller
    {
        IMenuInfoBll _menuInfoBll;
        public MenuInfoController(IMenuInfoBll menuInfoBll)
        {
            _menuInfoBll = menuInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 分页获取菜单信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMenuInfos(int page, int limit, string title)
        {
            int count;//记录数量
            //调用业务逻辑层的分页方法
            List<GetMenuInfoDTO> list = _menuInfoBll.GetMenuInfosBypage(page, limit, title, out count);
            object res = new
            {
                code = 0,
                msg = "",
                count = count,
                data = list
            };
            return Json(res);
        }
        public IActionResult CreateMenuInfoView()
        {
            return View();
        }
        /// <summary>
        /// 添加的方法
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateMenuInfo(string title, string description, int level, int sort,string href,string parentId,string icon,string target)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(title))
            {
                res.Msg = "菜单标题不能为空";
            }
            string msg;
            bool Issucceed = _menuInfoBll.CreateMenuInfo(title, description, level, sort, href, parentId, icon, target, out msg);
            res.Msg = msg;
            if (Issucceed)
            {
                res.Status = 1;
                res.IsSuccess = Issucceed;
            }
            return Json(res);
        }
        /// <summary>
        /// 获取菜单下拉选项数据集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetOptions()
        {
            //返回值模板对象
            CustomResultModel res = new CustomResultModel();
            //获取菜单下拉选项集
            List<OptionsModel> menuOptions = _menuInfoBll.GetmenuInfoOptions();

            res.Msg = "成功";
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                menuOptions
            };
            return Json(res);
        }

        /// <summary>
        /// 通过ID伪删除单个菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteMenuInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "菜单id不能为空";
                return Json(res);
            }
            bool result = _menuInfoBll.DeleteMenuInfo(id);
            if (result)
            {
                res.Status = 1;
                res.IsSuccess = true;
                res.Msg = "删除成功";
            }
            else
            {
                res.Msg = "删除失败";
            }
            return Json(res);
        }

        public IActionResult DeleteMenuInfos(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids != null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的菜单";
                return Json(res);
            }
            bool result = _menuInfoBll.DeleteMenuInfos(ids);
            if (!result)
            {
                res.Msg = "删除失败";
                return Json(res);
            }
            res.Msg = "删除成功";
            res.Status = 1;
            return Json(res);
        }
        /// <summary>
        /// 通过id获取单个菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetMenuInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "菜单编号不能为空";
                return Json(res);
            }
            MenuInfo menuInfo = _menuInfoBll.GetMenuInfoById(id);
            if (menuInfo == null)
            {
                res.Msg = "未获取到菜单";
                return Json(res);
            }
            List<OptionsModel> menuOptions = _menuInfoBll.GetMenuOptions(menuInfo.Id);
            res.Msg = "成功获取到菜单信息";
            res.Status = 1;
            res.IsSuccess = true;
            res.Datas = new
            {
                menuInfo,
                menuOptions,
            };
            return Json(res);

        }
        public IActionResult UpdateMenuInfoView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateMenuInfo(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(title))
            {
                res.Msg = "菜单名称不能为空";
                return Json(res);
            }
            bool result = _menuInfoBll.UpdateMenuInfo(id, title, description, level, sort, href, parentId, icon, target);
            if (!result)
            {
                res.Msg = "修改失败";
                return Json(res);
            }

            res.Msg = "修改成功";
            res.Status = 1;
            res.IsSuccess = true;

            return Json(res);

        }

        /// <summary>
        /// 获取首页左侧菜单数据
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMenuList()
        {
            
            //通过session拿到当前登陆人的Id
            string userId= HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(userId))
            {
                GetMenuListModelDTO res = new GetMenuListModelDTO();
                return Json(res); 
            }
            else
            {
                //查询
                List<GetMenuListModelDTO> list = _menuInfoBll.GetMenuList(userId);

                GetMenuListViewModel res = new GetMenuListViewModel(list);
                #region 以前的代码
                //var res = new
                //{
                //    homeInfo = new GetMenuListModelDTO
                //    {
                //        Title= "首页",
                //        Href= "/layuimini/page/welcome-1.html?t=1"
                //    },
                //    logoInfo = new GetMenuListModelDTO
                //    {
                //        Title= "LAYUI MINI",
                //        Image= "/layuimini/images/logo.png",
                //        Href=""
                //    },
                //    menuInfo = new List<object>
                //    {
                //        new GetMenuListModelDTO
                //        {
                //            Title="常规管理",
                //            Icon="fa fa-address-book",
                //            Href="",
                //            Target="_self",
                //            Child=new List<GetMenuListModelDTO>
                //            {
                //                new GetMenuListModelDTO
                //                {
                //                     Title ="用户管理",
                //                     Href  ="/UserInfo/Index",
                //                     Icon  ="fa fa-window-maximize",
                //                    Target ="_self"
                //                },
                //                new GetMenuListModelDTO
                //                {
                //                     Title ="部门管理",
                //                     Href  ="/DepartmentInfo/Index",
                //                     Icon  ="fa fa-window-maximize",
                //                    Target ="_self"
                //                },
                //                new GetMenuListModelDTO
                //                {
                //                     Title ="角色管理",
                //                     Href  ="/RoleInfo/Index",
                //                     Icon  ="fa fa-window-maximize",
                //                    Target ="_self"
                //                },
                //                new GetMenuListModelDTO
                //                {
                //                     Title ="菜单管理",
                //                     Href  ="/MenuInfo/Index",
                //                     Icon  ="fa fa-window-maximize",
                //                    Target ="_self"
                //                }
                //            }
                //        }
                //    }
                //};
                #endregion

                return Json(res);
            }            
        }
    }
}
