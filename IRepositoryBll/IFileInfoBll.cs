using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IRepositoryBll
{
    public interface IFileInfoBll
    {
        bool UploadData(string currentUserId, Stream stream, string beforepath,out string msg);
        string GetIMAGESRC(string currentUserId, out string msg);
    }
}
