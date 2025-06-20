using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface ICategoryBll
    {
        List<GetCategoryDTO> GetCategorysByPage(int limit, int page, string categoryName, out int count);
        bool CreateCategory(string categoryName, string description, out string msg);
        bool DeleteCategory(string id);
        Category GetCategoryById(string id);
        bool UpdateCategory(string id, string categoryName, string description);
        bool DeleteCategorys(List<string> ids);
        List<OptionsModel> GetCategoryOptions();        
    }
}
