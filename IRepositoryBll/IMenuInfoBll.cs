using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IMenuInfoBll
    {
        List<GetMenuInfoDTO> GetMenuInfosBypage(int page, int limit, string title, out int count);
        bool CreateMenuInfo(string title, string description, int level, int sort, string href, string parentId, string icon, string target, out string msg);
        List<OptionsModel> GetmenuInfoOptions();
        bool DeleteMenuInfo(string id);
        bool DeleteMenuInfos(List<string> ids);
        MenuInfo GetMenuInfoById(string id);
        List<OptionsModel> GetMenuOptions(string id);
        bool UpdateMenuInfo(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target);
        List<GetMenuListModelDTO> GetMenuList(string userId);
    }
}
