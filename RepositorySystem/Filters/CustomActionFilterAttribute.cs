using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RepositorySystem.Models;
using System;

namespace RepositorySystem.Filters
{
    /// <summary>
    /// 自定义过滤器
    /// </summary>
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string username=context.HttpContext.Session.GetString("userName");
            if (string.IsNullOrEmpty(username))
            {
                string path = context.HttpContext.Request.Path.ToString().ToLower();
                //如果访问的是页面
                if (path == "/" || path.Contains("index") || path.Contains("view"))
                {
                    //使用result不用进入之前的Action方法
                    context.Result = new RedirectResult("/Account/LoginView");
                }
                else
                {
                    CustomResultModel res = new CustomResultModel();
                    res.IsSuccess = false;
                    res.Status = 4;
                    res.Msg = "登录失效请重新登录";
                    context.Result = new JsonResult(res);
                }
            }
            
        }
    }
}
