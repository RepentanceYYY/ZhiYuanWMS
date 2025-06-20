using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RepositorySystem.Controllers
{
    public class ConsumableInfoController : Controller
    {
        IConsumableInfoBll _consumableInfoBll;
        ICategoryBll _categoryBll;
        public ConsumableInfoController(IConsumableInfoBll consumableInfoBll, ICategoryBll categoryBll)
        {
            _consumableInfoBll = consumableInfoBll;
            _categoryBll = categoryBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取所有耗材信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetConsumableInfos(int limit,int page,string consumableName)
        {
            int count;
            List<GetConsumableInfoDTO> list = _consumableInfoBll.GetConsumableInfosByPage(limit, page, consumableName,out count);
            object res = new
            {
                code=0,
                msg="",
                count=count,
                data=list
            };
            return Json(res);
         }
        public IActionResult CreateConsumableInfoView()   
        {
            return View();
        }
        /// <summary>   
        /// 获取耗材种类下拉框
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCategoryOptions()
        {
            CustomResultModel res = new CustomResultModel();
            List<OptionsModel> list = _categoryBll.GetCategoryOptions().ToList();
            res.IsSuccess = true;
            res.Datas = list;
            res.Msg = "获取成功";
            res.Status = 1;
            return Json(res);
        }
        public IActionResult CreateConsumableInfo(string consumableName,string specification,int num,string unit,decimal money,int warningNum,string categoryId,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(consumableName))
            {
                res.Msg = "耗材名称不能为空";
                return Json(res);
            }
            string msg;
            bool result = _consumableInfoBll.CreateConsumableInfo(consumableName, specification, num, unit, money, warningNum, categoryId, description,out msg);

            res.Msg= msg;
            if (result)
            {
                res.Status = 1;
                res.IsSuccess = true;
            }
            return Json(res);
        }
        public IActionResult UpdateConsumableInfoView()
        {
            return View();
        }
        /// <summary>
        /// 编辑的视图
        /// </summary>
        /// <param name="id"></param>
        /// <param name="consumableName"></param>
        /// <param name="specification"></param>
        /// <param name="num"></param>
        /// <param name="unit"></param>
        /// <param name="money"></param>
        /// <param name="warningNum"></param>
        /// <param name="categoryId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateConsumableInfo(string id,string consumableName, string specification, int num, string unit, decimal money, int warningNum, string categoryId, string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "id不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(consumableName))
            {
                res.Msg = "耗材名称不能为空";
                return Json(res);
            }
            bool result = _consumableInfoBll.UpdateConsumableInfo(id, consumableName, specification, num, unit, money, warningNum, categoryId, description);
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
        /// 获取耗材信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetConsumableInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "ID不能为空";
                return Json(res);
            }
            ConsumableInfo consumableInfo = _consumableInfoBll.GetConsumableInfoById(id);
            if (consumableInfo == null)
            {
                res.Msg = "未查询到耗材信息";
                return Json(res);
            }
            //获取部门下拉选项集
            var list = _categoryBll.GetCategoryOptions();

            res.Msg = "查询成功";
            res.IsSuccess = true;
            res.Status = 1;
            res.Datas = new
            {
                ConsumableInfo = consumableInfo,
                options = list
            };
            return Json(res);
        }

        public IActionResult DeleteConsumableInfo(string id)
        {
            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "请选择要删除的耗材";
            }
            bool result = _consumableInfoBll.DeleteConsumableInfo(id);
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
        public IActionResult DeleteConsumableInfos(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids != null && ids.Count <= 0)
            {
                res.Msg = "请选择要删除的耗材";
                return Json(res);
            }
            bool result = _consumableInfoBll.DeleteConsumableInfos(ids);
            if (!result)
            {
                res.Msg = "删除失败";
                return Json(res);
            }
            res.IsSuccess = true;
            res.Msg = "删除成功";
            res.Status = 1;
            return Json(res);
        }
        /// <summary>
        /// 导入Excel表格
        /// </summary>
        /// <returns></returns>
        public IActionResult Upload(IFormFile formFild)
        {
            CustomResultModel res = new CustomResultModel();

            //判断当前文件是否为excel文件
            string FName = formFild.FileName.Substring(formFild.FileName.LastIndexOf('.')+1);
            if (FName !="xlsx")
            {
                res.Msg = "请选择excel文件";
                return Json(res);
            }
            //看request
            var request = HttpContext.Request;
            //获取文件流对象
            Stream stream = formFild.OpenReadStream();
            //获取当前登陆人的Id
            string userId = HttpContext.Session.GetString("userId");

            string msg;
            bool result = _consumableInfoBll.Upload(stream, userId,out msg);

            res.Msg = msg;

            if (result)
            {
                res.IsSuccess = true;
                res.Status = 1;
            }
            

            return Json(res);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        public IActionResult DownLoad()
        {
            Stream stream =_consumableInfoBll.DownLoad();
            return  File(stream, "application/octet-stream", "出库模板.xlsx");
        }

    }
}
