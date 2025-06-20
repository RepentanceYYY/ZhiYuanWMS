using Entity;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryDal
{
    public class DepartmentInfoDal :BaseDeleteDal<DepartmentInfo>,IDepartmentInfoDal
    {
        RepositoryDBContext _DBContext;
        public DepartmentInfoDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {
            _DBContext = repositoryDBContext;
        }
        #region 以前的实现方式的代码


        //public bool CreateDepartmentInfo(DepartmentInfo departmentInfo)
        //{
        //    _DBContext.DepartmentInfo.Add(departmentInfo);
        //    return _DBContext.SaveChanges() > 0;
        //}

        //public bool DeleteDepartmentInfo(string id)
        //{
        //    DepartmentInfo departmentInfo =_DBContext.DepartmentInfo.FirstOrDefault(x=>x.Id==id);
        //    departmentInfo.IsDelete = true;
        //    departmentInfo.DeleteTime = DateTime.Now;
        //    return _DBContext.SaveChanges() > 0;
        //}

        //public bool DeleteDepartmentInfos(List<string> ids)
        //{
        //    List <DepartmentInfo> departmentInfo = _DBContext.DepartmentInfo.Where(x => ids.Contains(x.Id)).ToList();
        //    DateTime dateTime = DateTime.Now;
        //    foreach (var item in departmentInfo)
        //    {
        //        item.IsDelete = true;
        //        item.DeleteTime = dateTime;
        //    }
        //    return _DBContext.SaveChanges()>0;
        //}

        //public DepartmentInfo GetDepartmentInfoById(string id)
        //{
        //    DepartmentInfo departmentInfo= _DBContext.DepartmentInfo.FirstOrDefault(x => x.Id == id);
        //    return departmentInfo;
        //}

        //public DbSet<DepartmentInfo> GetDepartmentInfos()
        //{
        //    return _DBContext.DepartmentInfo;
        //}

        //public bool UpdateDepartmentInfo(DepartmentInfo departmentInfo)
        //{
        //    _DBContext.DepartmentInfo.Update(departmentInfo);
        //    return _DBContext.SaveChanges() > 0;
        //}
        #endregion
    }
}
