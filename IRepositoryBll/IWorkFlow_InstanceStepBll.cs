using Entity;
using Entity.DTOModels;
using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRepositoryBll
{
    public interface IWorkFlow_InstanceStepBll
    {
        List<GetWorkFlow_InstanceStepDTO> GetWorkFlow_InstanceStepsByPage(int limit, int page,string currentUserId,string creatorName, out int count);
        object GetWorkFlowDetail(string stepId);
        bool UpdateWorkFlow_InstanceStep(string id, string reviewReason, InstanceStepStatusEnum reviewStatus,int outNum, string currentUserId, out string msg);
    }
}
