using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System.Collections.Generic;

namespace RepositorySystem.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryBll _categoryBll;
        public CategoryController(ICategoryBll categoryBll)
        {
            _categoryBll = categoryBll;
        }
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取所有耗材种类
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCategorys(int limit,int page,string categoryName)
        {
            //统计条数
            int count;
           List<GetCategoryDTO> list= _categoryBll.GetCategorysByPage(limit, page, categoryName,out count);

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
        /// 添加耗材种类的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateCategoryView()
        {
            return View();
        }
        /// <summary>
        /// 添加耗材种类
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCategory(string categoryName,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(categoryName))
            {
                res.Msg = "耗材种类不能为空";
            }
            string msg;
            bool result = _categoryBll.CreateCategory(categoryName, description,out msg);
            res.Msg = msg;
            //如果添加成功
            if (result)
            {
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);
        }
        /// <summary>
        /// 根据ID删除单个耗材种类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteCategory(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "耗材id为空";
                return Json(res);
            }
            bool result = _categoryBll.DeleteCategory(id);

            res.Msg = "删除失败";

            if (result)
            {
                res.Msg = "删除成功";
                res.IsSuccess = true;
                res.Status = 1;

            }
            return Json(res);
        }
        /// <summary>
        /// 编辑耗材种类的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateCategoryView()
        {
            return View();
        }
        public IActionResult GetCategoryById(string id)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "耗材Id不能为空";
            }
            Category category = _categoryBll.GetCategoryById(id);
            if (category==null)
            {
                res.Msg = "获取耗材种类信息失败";

            }
            else
            {
                res.IsSuccess = true;
                res.Status = 1;
                res.Msg = "获取耗材种类信息成功";
                res.Datas = category;
            }
            return Json(res);
        }

        public IActionResult UpdateCategory(string id,string categoryName,string description)
        {
            CustomResultModel res = new CustomResultModel();
            if (string.IsNullOrEmpty(id))
            {
                res.Msg = "耗材种类Id不能为空";
            }
            if (string.IsNullOrEmpty(categoryName))
            {
                res.Msg = "耗材种类名称不能为空";
            }

            bool result = _categoryBll.UpdateCategory(id,categoryName,description);

            res.Msg = "修改失败";

            if (result)
            {
                res.Msg = "修改成功";
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);
        }
        /// <summary>
        /// 批量真删除耗材种类信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IActionResult DeleteCategorys(List<string> ids)
        {
            CustomResultModel res = new CustomResultModel();
            if (ids!=null && ids.Count<=0)
            {
                res.Msg = "请选择要删除的耗材种类";
                return Json(res);
            }
            bool result = _categoryBll.DeleteCategorys(ids);

            res.Msg = "删除失败";
            if (result)
            {
                res.Msg = "删除成功";
                res.IsSuccess = true;
                res.Status = 1;
            }
            return Json(res);

        }
    }
}
