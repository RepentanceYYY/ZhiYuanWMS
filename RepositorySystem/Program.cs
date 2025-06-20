using Common;
using Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //初始化数据库
            //InitDB();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        /// <summary>
        ///启动第一件事 初始化数据库
        /// </summary>
        public static void InitDB()
        {
            //启动第一件事，初始化数据
            var contextOptions=new DbContextOptionsBuilder<RepositoryDBContext>()
                .UseSqlServer($"Data Source=.;Initial Catalog=RepositorySysHH;User ID=sa;Password=123456")
                .Options;
            RepositoryDBContext _repositoryDBContext = new RepositoryDBContext(contextOptions);
            //如果有数据库就删除
            _repositoryDBContext.Database.EnsureDeleted();
            //如果没有数据库就创建
            _repositoryDBContext.Database.EnsureCreated();

            UserInfo userInfo = new UserInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Account = "admin",
                PassWord = MD5Helper.MD5Encrypt64("123456"),
                CreateTime=DateTime.Now,
                IsAdmin=true,
                UserName="管理员"
            };
            _repositoryDBContext.UserInfo.Add(userInfo);
            _repositoryDBContext.SaveChanges();
        }
    }
}
