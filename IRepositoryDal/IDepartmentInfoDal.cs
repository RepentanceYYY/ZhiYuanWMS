using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
namespace IRepositoryDal
{
    public interface IDepartmentInfoDal:IBaseDeleteDal<DepartmentInfo>
    {
        #region 以前的实现方式的代码
        //DbSet<DepartmentInfo> GetDepartmentInfos();
        //bool CreateDepartmentInfo(DepartmentInfo departmentInfo);
        //bool DeleteDepartmentInfo(string id);
        //DepartmentInfo GetDepartmentInfoById(string id);
        //bool UpdateDepartmentInfo(DepartmentInfo departmentInfo);
        //bool DeleteDepartmentInfos(List<string> ids);
        #endregion

    }
}
