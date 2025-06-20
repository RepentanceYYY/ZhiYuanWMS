using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class WorkFlow_InstanceDal:BaseDal<WorkFlow_Instance>, IWorkFlow_InstanceDal
    {
        public WorkFlow_InstanceDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {

        }
    }
}
