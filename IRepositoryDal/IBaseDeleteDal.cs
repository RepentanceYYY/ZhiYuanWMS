using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryDal
{
    public interface IBaseDeleteDal<T>:IBaseDal<T> where T:BaseDeleteEntity
    {
        /// <summary>
        /// 软删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteEntity(string id);

        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteEntityByIds(List<string> ids);
    }
}
