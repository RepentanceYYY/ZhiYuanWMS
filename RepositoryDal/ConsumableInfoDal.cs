using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class ConsumableInfoDal:BaseDeleteDal<ConsumableInfo>, IConsumableInfoDal
    {
        public ConsumableInfoDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {

        }
    }
}
