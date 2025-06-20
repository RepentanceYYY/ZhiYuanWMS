using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    public class BaseDal<T> :IBaseDal<T> where T:BaseEntity
    {
        RepositoryDBContext _dbcontext;
        public BaseDal(RepositoryDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 添加实体到数据库
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool CreateEntity(T entity)
        {
            _dbcontext.Set<T>().Add(entity);
            return _dbcontext.SaveChanges() > 0;
        }

        /// <summary>
        /// 真删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveEntity(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            return _dbcontext.SaveChanges() > 0;
        }

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
        public T GetEntityById(string id)
        {
            return _dbcontext.Set<T>().FirstOrDefault(x=>x.Id==id);
        }

        /// <summary>
        /// 获取所有实体信息
        /// </summary>
        /// <returns></returns>
        public DbSet<T> GetEntities()
        {
            return _dbcontext.Set<T>();
        }

        /// <summary>
        /// 编辑实体
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool UpdateEntity(T entity)
        {
            _dbcontext.Set<T>().Update(entity);
            return _dbcontext.SaveChanges() > 0;
        }

    }
}
