using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class CategoryDal:BaseDal<Category>, ICategoryDal
    {

        public CategoryDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {

        }
    }
}
