using Common;
using Entity;
using IRepositoryBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositorySystem.Models;
using System;
using System.IO;

namespace RepositorySystem.Controllers
{
    public class AccountController : Controller
    {
        IUserInfoBll _userInfoBll;
        IAccountBll _accountBll;
        IFileInfoBll _fileInfoBll;
        public AccountController(IUserInfoBll userInfoBll,IAccountBll accountBll, IFileInfoBll fileInfoBll)
        {
            _userInfoBll = userInfoBll;
            _accountBll = accountBll;
            _fileInfoBll = fileInfoBll;
        }
        public IActionResult LoginView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string account, string password)
        {
            CustomResultModel res = new CustomResultModel();
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
            string msg;
            string userName;
            string userId;
            bool isSuccess= _userInfoBll.Login(account, password,out msg,out userName,out userId);
            res.Msg = msg;
            if (isSuccess)
            {
                HttpContext.Session.SetString("userName", userName);
                HttpContext.Session.SetString("userId", userId);
                res.IsSuccess = true;
                res.Status = 1;
                res.Datas = new
                {
                    userName,
                    userId
                };

            }
            return Json(res);
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            CustomResultModel res = new CustomResultModel();
            HttpContext.Session.Remove("userName");
            res.Msg = "登出成功";
            res.IsSuccess = true;
            res.Status = 1;
            return Json(res);
        }
        /// <summary>
        /// 修改密码的视图
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        public IActionResult UpdatePasswordView()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdatePassword(string Id,string oldpassword, string newpassword)
        {
            CustomResultModel res = new CustomResultModel();

            if (string.IsNullOrEmpty(Id))
            {
                res.Msg = "Id不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(oldpassword))
            {
                res.Msg = "原密码不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(newpassword))
            {
                res.Msg = "现密码不能为空";
                return Json(res);
            }
            string msg;
            bool isSucess= _accountBll.UpdatePassword(Id, oldpassword, newpassword,out msg);
            res.Msg = msg;
            if (isSucess)
            {
                res.Status = 1;
                res.IsSuccess = true;
            }
            return Json(res);
            
        }
        
        /// <summary>
        /// 上传资料的视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UploadDataView()
        {
            return View();
        }
        /// <summary>
        /// 上传图片的接口
        /// </summary>
        /// <returns></returns>
        public IActionResult UploadData(IFormFile formFild)
        {
            CustomResultModel res = new CustomResultModel();

            string beforepath = formFild.FileName;
            //System.Drawing.Image img = System.Drawing.Image.FromFile(path);
            //获取文件流对象
            Stream stream = formFild.OpenReadStream();

            string currentUserId = HttpContext.Session.GetString("userId");
            string msg;
            //判断是否是图片
            if (IsImage(stream))
            {
                //获取当前用户id
                
                bool result = _fileInfoBll.UploadData(currentUserId, stream, beforepath,out msg);

                res.Msg = msg;
                //如果操作成功;
                if (result)
                {
                    res.IsSuccess = true;
                    res.Status = 1;
                     
                }
                return Json(res);

            }
            else
            {
                res.Msg = "请选择图片";
                return Json(res);
            }
            
            
        }
        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns>返回结果</returns>
        private Boolean IsImage(Stream stream)
        {
            try
            {
                System.Drawing.Image ResourceImage = System.Drawing.Image.FromStream(stream);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取头像功能
        /// </summary>
        /// <returns></returns>
        public IActionResult GetIMAGESRC()
        {
            string currentUserId = HttpContext.Session.GetString("userId");
            string msg;

             string imgsrc= _fileInfoBll.GetIMAGESRC(currentUserId,out msg);
            object res = new
            {
                msg= msg,
                src= imgsrc
            };
            return Json(res);
        }
    }
}
