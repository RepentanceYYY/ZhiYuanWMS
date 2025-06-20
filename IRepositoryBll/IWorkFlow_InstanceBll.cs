using Entity;
using Entity.DTOModels;
using Entity.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IWorkFlow_InstanceBll
    {
        List<GetWorkFlow_InstanceDTO> GetWorkFlow_InstancesByPage(int limit, int page, WorkFlow_InstanceStatusEnum statusOptions, out int count);
        bool CreateWorkFlow_Instance(string modelId, string outGoodsId, int outNum, string description, string reason,string userId, out string msg);
        WorkFlow_Instance GetWorkFlow_InstanceById(string id);
        bool UpdateWorkFlow_Instance(string id, string modelId, string outGoodsId, int outNum, string description, string reason, string userId);
        bool DeleteWorkFlow_Instance(string id,string currUesrId,out string msg);
        bool DeleteWorkFlow_Instances(List<string> ids);
        List<InstanceEnumOptionModel> GetStatusOptions();
    }
}
