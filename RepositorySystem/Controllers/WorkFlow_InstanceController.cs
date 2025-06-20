using Entity;
using Entity.DTOModels;
using Entity.Enums;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System.Collections;
using System.Collections.Generic;

namespace RepositorySystem.Controllers
{
    public class WorkFlow_InstanceController : Controller
    {
        IWorkFlow_InstanceBll _workFlow_InstanceBll;
        IWorkFlow_ModelBll _workFlow_ModelBll;
        IConsumableInfoBll _consumableInfoBll;
        public WorkFlow_InstanceController(IConsumableInfoBll consumableInfoBll, IWorkFlow_ModelBll workFlow_ModelBll, IWorkFlow_InstanceBll workFlow_InstanceBll)
        {
            _workFlow_InstanceBll = workFlow_InstanceBll;
            _workFlow_ModelBll = workFlow_ModelBll;
            _consumableInfoBll = consumableInfoBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetWorkFlow_Instances(int limit,int page, WorkFlow_InstanceStatusEnum statusOptions)
        {
            int count;
            List < GetWorkFlow_InstanceDTO > list = _workFlow_InstanceBll.GetWorkFlow_InstancesByPage(limit, page, statusOptions, out count);
            object res = new
            {
                code = 0,
                msg = "",
                count = count,
                data = list
            };
            return Json(res);
        }

        public IActionResult CreateWorkFlow_InstanceView()
        {
            return View();
        }

        public IActionResult CreateWorkFlow_Instance(string modelId,string outGoodsId,int outNum,string description,string reason)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(modelId))
            {
                res.Msg = "工作流模板不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(outGoodsId))
            {
                res.Msg = "物品不能为空";
                return Json(res);
            }
            if (outNum<0)
            {
                res.Msg = "申请数量不能小于零";
                return Json(res);
            }
            if (string.IsNullOrEmpty(reason))
            {
                res.Msg = "申请理由不能为空";
                return Json(res);
            }

            string userId = HttpContext.Session.GetString("userId");
            string msg;
            bool isSuccess = _workFlow_InstanceBll.CreateWorkFlow_Instance(modelId,outGoodsId,outNum,description, reason, userId, out msg);
            res.Msg = msg;
            if (isSuccess)
            {
                res.Status = 1;
                res.IsSuccess = true;
            }
            return Json(res);
        }

        public IActionResult GetOptions()
        {
            //返回值模板对象
            CustomResultModel res = new CustomResultModel();
            //获取工作流模板下拉选项集
            List<OptionsModel> workFlow_ModelOptions = _workFlow_ModelBll.GetWorkFlow_ModelOptions();
            //获取物资信息下拉选项集
            List<OptionsModel> consumableInfoOptions = _consumableInfoBll.GetConsumableInfoOptions();
            //获取状态下拉选项集
            List<InstanceEnumOptionModel> statusOptions =_workFlow_InstanceBll.GetStatusOptions();


            res.Msg = "成功";
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                workFlow_ModelOptions,
                consumableInfoOptions,
                statusOptions
            };
            return Json(res);
        }

        public IActionResult UpdateWorkFlow_InstanceView()
        {
            return View();
        }

        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetWorkFlow_Instance(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "工作流实例编号不能为空";
                return Json(res);
            }
            WorkFlow_Instance workFlow_Instance = _workFlow_InstanceBll.GetWorkFlow_InstanceById(id);
            if (workFlow_Instance == null)
            {
                res.Msg = "未获取到工作流实例";
                return Json(res);
            }
            //获取工作流模板下拉选项集
            List<OptionsModel> workFlow_ModelOptions = _workFlow_ModelBll.GetWorkFlow_ModelOptions();
            //获取物资信息下拉选项集
            List<OptionsModel> consumableInfoOptions = _consumableInfoBll.GetConsumableInfoOptions();
            res.Msg = "成功获取到用户信息";
            res.Status = 1;
            res.IsSuccess = true;
            res.Datas = new
            {
                workFlow_Instance,
                workFlow_ModelOptions,
                consumableInfoOptions
            };
            return Json(res);
        }
        [HttpPost]
        public IActionResult UpdateWorkFlow_Instance(string id, string modelId, string outGoodsId, int outNum, string description, string reason)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(modelId))
            {
                res.Msg = "工作流模板不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(outGoodsId))
            {
                res.Msg = "物品不能为空";
                return Json(res);
            }
            if (outNum < 0)
            {
                res.Msg = "申请数量不能小于零";
                return Json(res);
            }
            if (string.IsNullOrEmpty(reason))
            {
                res.Msg = "申请理由不能为空";
                return Json(res);
            }
            string userId = HttpContext.Session.GetString("userId");

            bool result = _workFlow_InstanceBll.UpdateWorkFlow_Instance(id, modelId, outGoodsId, outNum, description, reason, userId);
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
        /// 通过ID作废单个工作流实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteWorkFlow_Instance(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "工作流实例id不能为空";
                return Json(res);
            }
            string currUesrId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(currUesrId))
            {
                res.Msg = "登录已过，请重新登录";
                return Json(res);
            }
            string msg;
            bool result = _workFlow_InstanceBll.DeleteWorkFlow_Instance(id, currUesrId,out msg);
            
            if (result)
            {
                res.Status = 1;
                res.IsSuccess = true;
                res.Msg = "作废成功";
            }
            else
            {
                res.Msg = "作废失败";
            }
            return Json(res);
        }

        public IActionResult DeleteWorkFlow_Instances(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids != null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的工作流实例";
                return Json(res);
            }
            bool result = _workFlow_InstanceBll.DeleteWorkFlow_Instances(ids);
            if (!result)
            {
                res.Msg = "删除失败";
                return Json(res);
            }
            res.Msg = "删除成功";
            res.Status = 1;
            return Json(res);
        }
    }
}
