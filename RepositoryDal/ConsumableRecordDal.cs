using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class ConsumableRecordDal:BaseDal<ConsumableRecord>, IConsumableRecordDal
    {
        public ConsumableRecordDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {

        }
    }
}
