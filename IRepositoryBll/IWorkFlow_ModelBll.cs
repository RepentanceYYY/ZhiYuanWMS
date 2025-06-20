using Entity;
using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IWorkFlow_ModelBll
    {
        List<GetWorkFlow_ModelDTO> GetWorkFlow_ModelsByPage(int limit, int page, string title, out int count);
        bool CreateWorkFlow_Model(string title, string description, out string msg);
        WorkFlow_Model GetWorkFlow_ModelById(string id);
        bool UpdateWorkFlow_Model(string id, string title, string description);
        bool DeleteWorkFlow_Model(string id);
        bool DeleteWorkFlow_Models(List<string> ids);
        List<OptionsModel> GetWorkFlow_ModelOptions();
    }
}
