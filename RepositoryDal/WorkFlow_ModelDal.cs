using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class WorkFlow_ModelDal:BaseDeleteDal<WorkFlow_Model>, IWorkFlow_ModelDal
    {
        public WorkFlow_ModelDal(RepositoryDBContext dbContext):base(dbContext)
        {

        }
    }
}
