using Entity;
using IRepositoryBll;
using IRepositoryDal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RepositoryBll;
using RepositoryDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //services.AddDbContext<RepositoryDBContext>(options => options.UseSqlServer($"Data Source=.;Initial Catalog=RepositorySysHH;User ID=sa;Password=123"));
            services.AddDbContext<RepositoryDBContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            //×¢²ásession
            services.AddSession();

            services.AddScoped<IAccountBll, AccountBll>();

            services.AddScoped<IUserInfoBll,UserInfoBll>();            
            services.AddScoped<IUserInfoDal, UserInfoDal>();

            services.AddScoped<IDepartmentInfoBll, DepartmentInfoBll>();
            services.AddScoped<IDepartmentInfoDal, DepartmentInfoDal>();

            services.AddScoped<IRoleInfoBll,RoleInfoBll>();
            services.AddScoped<IRoleInfoDal, RoleInfoDal>();

            services.AddScoped<IMenuInfoBll,MenuInfoBll>();
            services.AddScoped<IMenuInfoDal,MenuInfoDal>();

            services.AddScoped<IR_UserInfo_RoleInfoDal, R_UserInfo_RoleInfoDal>();
            services.AddScoped<IR_RoleInfo_MenuInfoDal, R_RoleInfo_MenuInfoDal>();

            
            services.AddScoped<ICategoryBll, CategoryBll>();
            services.AddScoped<ICategoryDal, CategoryDal>();

            services.AddScoped<IConsumableInfoBll, ConsumableInfoBll>();
            services.AddScoped<IConsumableInfoDal, ConsumableInfoDal>();

            services.AddScoped<IConsumableRecordDal,ConsumableRecordDal>();

            services.AddScoped<IWorkFlow_ModelDal, WorkFlow_ModelDal>();
            services.AddScoped<IWorkFlow_ModelBll, WorkFlow_ModelBll>();

            services.AddScoped<IWorkFlow_InstanceBll, WorkFlow_InstanceBll>();
            services.AddScoped<IWorkFlow_InstanceDal, WorkFlow_InstanceDal>();

            services.AddScoped<IWorkFlow_InstanceStepBll, WorkFlow_InstanceStepBll>();
            services.AddScoped<IWorkFlow_InstanceStepDal, WorkFlow_InstanceStepDal>();

            services.AddScoped<IFileInfoBll, FileInfoBll>();
            services.AddScoped<IFileInfoDal, FileInfoDal>();






        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=LoginView}/{id?}");
            });
        }
    }
}
