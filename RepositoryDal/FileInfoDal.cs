using Entity;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryDal
{
    public class FileInfoDal:BaseDal<FileInfo>, IFileInfoDal
    {
        public FileInfoDal(RepositoryDBContext repositoryDBContext):base(repositoryDBContext)
        {

        }
    }
}
