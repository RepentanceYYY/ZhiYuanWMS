using Entity;
using Entity.Enums;
using IRepositoryBll;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
//using FileInfo = Entity.FileInfo;

namespace RepositoryBll
{
    public class FileInfoBll : IFileInfoBll
    {
        IFileInfoDal _fileInfoDal;
        RepositoryDBContext _dbcontext;
        public FileInfoBll(RepositoryDBContext dbcontext, IFileInfoDal fileInfoDal)
        {
            _fileInfoDal = fileInfoDal;
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetIMAGESRC(string currentUserId, out string msg)
        {
            //拿到最后一个图片信息
            Entity.FileInfo fileInfo = _fileInfoDal.GetEntities().OrderByDescending(x=>x.CreateTime).Where(x=>x.Creator == currentUserId).FirstOrDefault();
            if (fileInfo==null)
            {
                msg = "未获取图片文件";
                return "";
            }
            string src = "/" + "img"+"/"+ fileInfo.NewFileName+ fileInfo.Extension;
            msg = "成功获取图片";
            return src;

        }

        public bool UploadData(string currentUserId, Stream stream, string beforepath,out string msg)
        {
            
            //获取当前路径
            //获取项目当前运行的目录
            string directoryPaht = Directory.GetCurrentDirectory();

            //找到wwww文件夹
            string testPath = Path.Combine(directoryPaht, "wwwroot", "img");
            //如果没有文件夹就创建
            if (!Directory.Exists(testPath))
            {
                //创建文件夹
                Directory.CreateDirectory(testPath);
            }
            

            //获取当前时间包括毫秒，用于取新名
            string date = DateTime.Now.ToString("yyyyMMddHHmmss")+DateTime.Now.Millisecond.ToString();
            //获取图片文件后缀                    
            string str = Path.GetExtension(beforepath);
            //设置路径和文件名和后缀
            string path2 = Path.Combine(testPath,date)+str;
            //获取流里面的图片
            Image image = Image.FromStream(stream);

            image.Save(path2);

            Entity.FileInfo fileInfo = new Entity.FileInfo()
            {
                Id = Guid.NewGuid().ToString(),
                RelationId = currentUserId,
                OldFileName = Path.GetFileNameWithoutExtension(beforepath),
                NewFileName = date,
                Extension= Path.GetExtension(beforepath),
                Length = stream.Length,
                CreateTime = DateTime.Now,
                Creator = currentUserId,
                Category = FileInfoCategoryEnum.图片
            };
            bool re=_fileInfoDal.CreateEntity(fileInfo);
            if (re)
            {
                msg = "成功";
                return true;
            }
            else
            {
                msg = "失败";
                return false;
            }                    
            }            
        }
    }
