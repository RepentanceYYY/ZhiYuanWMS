using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class R_RoleInfo_MenuInfoDal : BaseDal<R_RoleInfo_MenuInfo>,IR_RoleInfo_MenuInfoDal
    {

        public R_RoleInfo_MenuInfoDal(RepositoryDBContext dbcontext):base(dbcontext)
        {

        }
    }
}
