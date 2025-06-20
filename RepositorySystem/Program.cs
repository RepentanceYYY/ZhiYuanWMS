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
            //��ʼ�����ݿ�
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
        ///������һ���� ��ʼ�����ݿ�
        /// </summary>
        public static void InitDB()
        {
            //������һ���£���ʼ������
            var contextOptions=new DbContextOptionsBuilder<RepositoryDBContext>()
                .UseSqlServer($"Data Source=.;Initial Catalog=RepositorySysHH;User ID=sa;Password=123456")
                .Options;
            RepositoryDBContext _repositoryDBContext = new RepositoryDBContext(contextOptions);
            //��������ݿ��ɾ��
            _repositoryDBContext.Database.EnsureDeleted();
            //���û�����ݿ�ʹ���
            _repositoryDBContext.Database.EnsureCreated();

            UserInfo userInfo = new UserInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Account = "admin",
                PassWord = MD5Helper.MD5Encrypt64("123456"),
                CreateTime=DateTime.Now,
                IsAdmin=true,
                UserName="����Ա"
            };
            _repositoryDBContext.UserInfo.Add(userInfo);
            _repositoryDBContext.SaveChanges();
        }
    }
}
