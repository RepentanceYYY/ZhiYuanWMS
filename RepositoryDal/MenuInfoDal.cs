using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    
    public class MenuInfoDal :BaseDeleteDal<MenuInfo>, IMenuInfoDal
    {
        RepositoryDBContext _dbcontext;
        public MenuInfoDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {
            _dbcontext = repositoryDBContext;
        }

        #region 以前的代码
        //public bool CreateMenuInfo(MenuInfo menuInfo)
        //{
        //    _dbcontext.MenuInfo.Add(menuInfo);
        //    return _dbcontext.SaveChanges()>0;
        //}
        ///// <summary>
        ///// 删除dal层
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public bool DeleteMenuInfo(string id)
        //{
        //    MenuInfo menuInfo= GetMenuInfoById(id);
        //    menuInfo.IsDelete = true;
        //    menuInfo.DeleteTime = DateTime.Now;
        //    return _dbcontext.SaveChanges() > 0;
        //}

        //public DbSet<MenuInfo> GetMenuInfos()
        //{
        //    return _dbcontext.MenuInfo;
        //}
        ///// <summary>
        ///// 根据ID获取单个菜单信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public MenuInfo GetMenuInfoById(string id)
        //{
        //    return _dbcontext.MenuInfo.FirstOrDefault(m => m.Id == id);
        //}

        //public bool DeleteMenuInfos(List<string> ids)
        //{
        //    List<MenuInfo> MenuInfo = _dbcontext.MenuInfo.Where(x => ids.Contains(x.Id)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in MenuInfo)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    return _dbcontext.SaveChanges() > 0;
        //}

        //public bool UpdateMenuInfo(MenuInfo menuInfo)
        //{
        //    _dbcontext.MenuInfo.Update(menuInfo);
        //    return _dbcontext.SaveChanges()>0;
        //}
        #endregion

    }
}
