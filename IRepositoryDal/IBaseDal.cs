using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryDal
{
    public interface IBaseDal<T> where T:BaseEntity
    {
        /// <summary>
        /// 添加实体到数据库
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool CreateEntity(T entity);

        /// <summary>
        /// 真删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveEntity(T entity);

        //public bool DeleteRoleInfos(List<string> ids)
        //{
        //    List<RoleInfo> list = _dbcontext.RoleInfo.Where(r => ids.Contains(r.Id)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in list)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    return _dbcontext.SaveChanges() > 0;
        //}


        /// <summary>
        /// 根据id查询实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntityById(string id);

        /// <summary>
        /// 获取所有实体信息
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetEntities();

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity);
    }
}
