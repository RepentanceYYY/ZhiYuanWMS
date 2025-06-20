using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IAccountBll
    {
        bool UpdatePassword(string id, string oldpassword, string newpassword,out string msg);
    }
}
