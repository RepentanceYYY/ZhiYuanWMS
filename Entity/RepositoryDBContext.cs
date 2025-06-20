using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 数据库上下文类
    /// </summary>
    public class RepositoryDBContext:DbContext
    {
        public RepositoryDBContext(DbContextOptions<RepositoryDBContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer($"Data Source=.;Initial Catalog=RepositorySysHH;User ID=sa;Password=123");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<DepartmentInfo> DepartmentInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<IR_UserInfo_RoleInfo> R_UserInfo_RoleInfo { get; set; }
        public DbSet<R_RoleInfo_MenuInfo> R_RoleInfo_MenuInfo { get; set; }
        public DbSet<ConsumableInfo> ConsumableInfo { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ConsumableRecord> ConsumableRecord { get; set; }
        public DbSet<WorkFlow_Instance> WorkFlow_Instance { get; set; }
        public DbSet<WorkFlow_InstanceStep> WorkFlow_InstanceStep { get; set; }
        public DbSet<WorkFlow_Model> WorkFlow_Model { get; set; }

        public DbSet<FileInfo> FileInfo { get; set; }


    }
}
