using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    public class BaseDeleteDal<T> : BaseDal<T>,IBaseDeleteDal<T> where T : BaseDeleteEntity
    {
        RepositoryDBContext _dbcontext;
        public BaseDeleteDal(RepositoryDBContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 软删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEntity(string id)
        {
            T entity = _dbcontext.Set<T>().FirstOrDefault(u => u.Id == id);
            if (entity != null)
            {
                entity.IsDelete = true;
                entity.DeleteTime = DateTime.Now;
                return _dbcontext.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
            
            
        }

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteEntityByIds(List<string> ids)
        {
            List<T> list = _dbcontext.Set<T>().Where(r => ids.Contains(r.Id)).ToList();
            DateTime dateTime = DateTime.Now;
            foreach (var item in list)
            {
                item.IsDelete = true;
                item.DeleteTime = dateTime;
            }
            return _dbcontext.SaveChanges() > 0;
        }
    }
}
