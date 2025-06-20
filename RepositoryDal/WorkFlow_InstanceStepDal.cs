using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class WorkFlow_InstanceStepDal:BaseDal<WorkFlow_InstanceStep>, IWorkFlow_InstanceStepDal
    {
        public WorkFlow_InstanceStepDal(RepositoryDBContext dbcontext):base(dbcontext)
        {

        }
    }
}
