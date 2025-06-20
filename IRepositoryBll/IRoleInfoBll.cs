using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IRoleInfoBll
    {
        List<GetRoleInfoDTO> GetRoleInfosByPage(int limit, int page, string roleName, out int count);
        bool CreateRoleInfo(string roleName, string description, out string msg);
        RoleInfo GetRoleInfoById(string id);
        bool UpdateRoleInfo(string id, string roleName, string description);
        bool DeleteRoleInfo(string id);
        bool DeleteRoleInfos(List<string> ids);
        List<string> GetBindUserIds(string roleId);
        bool BindUserInfo(string roleId, List<string> userIds);
        List<string> GetBindMenuIds(string roleId);
        bool BindMenuInfo(string roleId, List<string> menuIds);
    }
}
