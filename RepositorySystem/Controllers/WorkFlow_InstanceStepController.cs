using Entity;
using Entity.DTOModels;
using Entity.Enums;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem.Controllers
{
    public class WorkFlow_InstanceStepController : Controller
    {
        IWorkFlow_InstanceStepBll _workFlow_InstanceStepBll;
        public WorkFlow_InstanceStepController(IWorkFlow_InstanceStepBll workFlow_InstanceStepBll)
        {
            _workFlow_InstanceStepBll = workFlow_InstanceStepBll;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetWorkFlow_InstanceSteps(int limit,int page,string creatorName)
        {
            int count;
            string currentUserId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new
                {
                    code = 0,
                    msg = "",
                    count = 0,
                    data = new List<object>()
                });
            }

            List<GetWorkFlow_InstanceStepDTO> list = _workFlow_InstanceStepBll.GetWorkFlow_InstanceStepsByPage(limit, page, currentUserId, creatorName,out count);
            object res = new
            {
                code = 0,
                msg = "",
                count = count,
                data = list
            };
            return Json(res);
        }


        public IActionResult GetWorkFlowDetail(string stepId)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(stepId))
            {
                res.Msg = "步骤id不能为空";
                return Json(res);
            }
            object data= _workFlow_InstanceStepBll.GetWorkFlowDetail(stepId);
            if (data == null)
            {
                res.Msg = "未查询到申请详情信息";
                return Json(res);
            }
            else
            {
                res.Msg = "查询成功";
                res.Status = 1;
                res.Datas = data;
                res.IsSuccess = true;
                return Json(res);
            }
        }
        /// <summary>
        /// 审批的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateWorkFlow_InstanceStepView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateWorkFlow_InstanceStep(string id,string reviewReason,int outNum, InstanceStepStatusEnum reviewStatus)
        {
            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "步骤id不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(reviewReason))
            {
                res.Msg = "审核理由不能为空";
                return Json(res);
            }            
            if (reviewStatus !=InstanceStepStatusEnum.同意 && reviewStatus !=InstanceStepStatusEnum.驳回)
            {
                res.Msg = "状态有误";
                return Json(res);
            }
            string currentUserId = HttpContext.Session.GetString("userId");
            if (currentUserId ==null)
            {
                res.Msg = "登录过期";
                return Json(res);
            }

            string msg;
            //审批操作
            bool result= _workFlow_InstanceStepBll.UpdateWorkFlow_InstanceStep(id, reviewReason, reviewStatus, outNum, currentUserId, out msg);
            res.Msg = msg;
            if (result)
            {
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);
        }
    }
}
