using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IDepartmentInfoBll
    {
        public List<GetDepartmentInfoDTO> GetDepartmentInfosBypage(int page, int limit,string departmentName, out int count);
        bool CreateDepartmentInfo(string departmentName, string leaderId, string parentId, string description, out string msg);
        bool DeleteDepartmentInfo(string id);
        DepartmentInfo GetDepartmentInfoById(string id);
        bool UpdateDepartmentInfo(string id, string departmentName, string leaderId, string parentId, string description);
        bool DeleteDepartmentInfos(List<string> ids);
        /// <summary>
        /// 获取部门下拉选项集
        /// </summary>
        List<OptionsModel> GetDepartmentOptions();
        List<OptionsModel> GetDepartmentOptions(string departmentId);
    }
}
