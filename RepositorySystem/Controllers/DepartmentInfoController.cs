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
    public class DepartmentInfoController : Controller
    {
        IDepartmentInfoBll _departmentBll;
        IUserInfoBll _userInfoBll;
        public DepartmentInfoController(IDepartmentInfoBll departmentBll,IUserInfoBll userInfoBll)
        {
            _departmentBll = departmentBll;
            _userInfoBll = userInfoBll;
        }
        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDepartmentInfos(int page,int limit,string departmentName)
        {
            int count;//记录数量
            //调用业务逻辑层的分页方法
            List<GetDepartmentInfoDTO> list= _departmentBll.GetDepartmentInfosBypage(page,limit, departmentName,out count);
            object res = new {
                code=0,
                msg="",
                count=count,
                data=list
            };
            return Json(res);
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 添加的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateDepartmentInfoView()
        {
            return View();
        }
        /// <summary>
        /// 添加的方法
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateDepartmentInfo(string departmentName, string leaderId, string parentId, string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(departmentName))
            {
                res.Msg="部门名称不能为空";
            }
            string msg;
            bool Issucceed= _departmentBll.CreateDepartmentInfo(departmentName, leaderId, parentId, description,out msg);
            res.Msg = msg;
            if (Issucceed)
            {
                res.Status = 1;
                res.IsSuccess = Issucceed;
            }
            return Json(res);
        }
        /// <summary>
        /// 通过ID伪删除单个部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteDepartmentInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "部门id不能为空";
                return Json(res);
            }
            bool result = _departmentBll.DeleteDepartmentInfo(id);
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

        public IActionResult UpdateDepartmentInfoView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateDepartmentInfo(string id, string departmentName, string leaderId, string parentId, string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(departmentName))
            {
                res.Msg = "部门名称不能为空";
                return Json(res);
            }
            bool result=_departmentBll.UpdateDepartmentInfo(id, departmentName, leaderId, parentId, description);
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
        /// 通过id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetDepartmentInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "部门编号不能为空";
                return Json(res);
            }
            DepartmentInfo departmentInfo = _departmentBll.GetDepartmentInfoById(id);
            if (departmentInfo==null)
            {
                res.Msg = "未获取到用户";
                return Json(res);
            }
            List<OptionsModel> departmentOptions= _departmentBll.GetDepartmentOptions(departmentInfo.Id);
            List<OptionsModel>userInfoOptions= _userInfoBll.GetUserInfoOptions();
            res.Msg = "成功获取到用户信息";
            res.Status = 1;
            res.IsSuccess = true;
            res.Datas = new
            {
                departmentInfo,
                departmentOptions,
                userInfoOptions
            };
            return Json(res);

        }
        public IActionResult DeleteDepartmentInfos(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if(ids !=null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的部门";
                return Json(res);
            }
            bool result= _departmentBll.DeleteDepartmentInfos(ids);
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
        /// 获取用户和部门下拉选项数据集
        /// </summary>
        /// <returns></returns>
        public IActionResult GetOptions()
        {
            //返回值模板对象
            CustomResultModel res = new CustomResultModel();
            //获取部门下拉选项集
            List<OptionsModel>  departmentOptions = _departmentBll.GetDepartmentOptions();
            //获取用户下拉选项集
            List<OptionsModel> userOptions=  _userInfoBll.GetUserInfoOptions();

            res.Msg = "成功";
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                departmentOptions,
                userOptions
            };
            return Json(res);
        }

    }
}
