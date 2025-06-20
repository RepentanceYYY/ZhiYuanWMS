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
    public class WorkFlow_ModelController : Controller
    {
        IWorkFlow_ModelBll _workFlow_ModelBll;
        public WorkFlow_ModelController(IWorkFlow_ModelBll workFlow_ModelBll)
        {
            _workFlow_ModelBll = workFlow_ModelBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取所有工作模板
        /// </summary>
        /// <returns></returns>
        public IActionResult GetWorkFlow_Models(int limit,int page,string title)
        {
            int count;
            List<GetWorkFlow_ModelDTO> list = _workFlow_ModelBll.GetWorkFlow_ModelsByPage(limit,page,title,out count);

            object res = new
            {
                code=0,
                msg="",
                count=count,
                data=list

            };
            return Json(res);
        }
        public IActionResult CreateWorkFlow_ModelView()
        {
            return View();
        }

        public IActionResult CreateWorkFlow_Model(string title,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(title))
            {
                res.Msg = "标题不能为空";
                return Json(res);
            }
            string msg;
            bool isSuccess = _workFlow_ModelBll.CreateWorkFlow_Model(title,description, out msg);
            res.Msg = msg;
            if (isSuccess)
            {
                res.Status = 1;
                res.IsSuccess = true;
            }
            return Json(res);
        }

        public IActionResult GetWorkFlow_ModelById(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "id不能为空";
                return Json(res);
            }
            WorkFlow_Model workFlow_Model = _workFlow_ModelBll.GetWorkFlow_ModelById(id);

            if (workFlow_Model == null)
            {
                res.Msg = "获取工作流模板信息失败";

            }
            else
            {
                res.Msg = "获取成功";
                res.Datas = workFlow_Model;
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);
        }

        /// <summary>
        /// 编辑的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateWorkFlow_ModelView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateWorkFlow_Model(string id, string title, string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(title))
            {
                res.Msg = "工作流模板名不能为空";
                return Json(res);
            }
            bool Result = _workFlow_ModelBll.UpdateWorkFlow_Model(id, title, description);

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

        public IActionResult DeleteWorkFlow_Model(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "编号不能为空";
                return Json(res);
            }
            bool result = _workFlow_ModelBll.DeleteWorkFlow_Model(id);
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

        public IActionResult DeleteWorkFlow_Models(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids != null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的工作流模板";
                return Json(res);
            }
            bool result = _workFlow_ModelBll.DeleteWorkFlow_Models(ids);
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


    }
}
