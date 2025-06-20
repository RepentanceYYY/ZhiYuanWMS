using Entity;
using Entity.DTOModels;
using Entity.Enums;
using IRepositoryBll;
using IRepositoryDal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryBll
{
    public class WorkFlow_InstanceBll : IWorkFlow_InstanceBll
    {
        RepositoryDBContext _dbContext;
        IWorkFlow_InstanceDal _workFlow_InstanceDal;
        IWorkFlow_ModelDal _workFlow_ModelDal;
        IWorkFlow_InstanceStepDal _workFlow_InstanceStepDal;
        IConsumableInfoDal _consumableInfoDal;
        IUserInfoDal _userInfoDal;
        IDepartmentInfoDal _departmentInfoDal;

        public WorkFlow_InstanceBll(RepositoryDBContext dbContext, IWorkFlow_InstanceStepDal workFlow_InstanceStepDal, IDepartmentInfoDal departmentInfoDal,IUserInfoDal userInfoDal, IConsumableInfoDal consumableInfoDal, IWorkFlow_InstanceDal workFlow_InstanceDal, IWorkFlow_ModelDal workFlow_ModelDal)
        {
            _dbContext = dbContext;
            _workFlow_InstanceDal = workFlow_InstanceDal;
            _workFlow_ModelDal = workFlow_ModelDal;
            _workFlow_InstanceStepDal = workFlow_InstanceStepDal;
            _consumableInfoDal = consumableInfoDal;
            _userInfoDal = userInfoDal;
            _departmentInfoDal = departmentInfoDal;
        }

        public bool CreateWorkFlow_Instance(string modelId, string outGoodsId, int outNum, string description, string reason,string userId, out string msg)
        {

            using (var Transaction = _dbContext.Database.BeginTransaction())
            {
                WorkFlow_Instance workFlow_Instance = new WorkFlow_Instance()
                {
                    Id = Guid.NewGuid().ToString(),
                    ModelId = modelId,
                    OutGoodsId = outGoodsId,
                    OutNum = outNum,
                    Description = description,
                    Reason = reason,
                    Creator = userId,
                    Status = WorkFlow_InstanceStatusEnum.审批中,//临时放进去
                    CreateTime = DateTime.Now
                };
                               
                //找出部门经理
                string LeaderId = (from u in _userInfoDal.GetEntities().Where(x => x.Id == userId && !x.IsDelete)
                                     join d in _departmentInfoDal.GetEntities().Where(x => !x.IsDelete)
                                     on u.DepartmentId equals d.Id
                                     select d.LeaderId).FirstOrDefault();

                if (string.IsNullOrEmpty(LeaderId))
                {
                    msg = "未获取到用户部门领导";
                    Transaction.Rollback();//回滚
                    return false;
                }
                //给工作流步骤表加数据
                WorkFlow_InstanceStep workFlow_InstanceStep = new WorkFlow_InstanceStep()
                {
                    Id = Guid.NewGuid().ToString(),
                    InstanceId=workFlow_Instance.Id,
                    ReviewerId = LeaderId,
                    ReviewStatus = InstanceStepStatusEnum.审核中,
                    CreateTime = DateTime.Now

                };

                _dbContext.WorkFlow_Instance.Add(workFlow_Instance);
                _dbContext.WorkFlow_InstanceStep.Add(workFlow_InstanceStep);
                int index = _dbContext.SaveChanges();
                if (index==2)
                {
                    msg = "添加成功";
                    Transaction.Commit();
                    return true;
                }
                else
                {
                    msg = "添加失败";
                    Transaction.Rollback();
                    return false;
                }
                
            }
            
        }
        /// <summary>
        /// 作废工作流实例
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currUesrId"></param>
        /// <returns></returns>
        public bool DeleteWorkFlow_Instance(string id,string currUesrId,out string msg)
        {
            using (var transaction=_dbContext.Database.BeginTransaction())
            {
                try
                {
                    WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(id);

                    if (workFlow_Instance == null)
                    {
                        msg = "未获取到此工作流实例";
                        transaction.Rollback();
                        return false;
                    }

                    if (workFlow_Instance.Creator != currUesrId)
                    {
                        msg = "您无权作废此工作流实例";
                        transaction.Rollback();
                        return false;
                    }

                    if (workFlow_Instance.Status  != WorkFlow_InstanceStatusEnum.审批中)
                    {
                        msg = "无法作废此工作流实例";
                        transaction.Rollback();
                        return false;
                    }
                    List<bool> list = new List<bool>();
                    //修改状态
                    workFlow_Instance.Status = WorkFlow_InstanceStatusEnum.作废;
                    list.Add(_workFlow_InstanceDal.UpdateEntity(workFlow_Instance));

                    List<WorkFlow_InstanceStep> workFlow_InstanceSteps= _workFlow_InstanceStepDal.GetEntities().Where(x => x.InstanceId == id && x.ReviewStatus == InstanceStepStatusEnum.审核中).ToList();

                    foreach (var item in workFlow_InstanceSteps)
                    {
                        item.ReviewStatus = InstanceStepStatusEnum.作废;
                        list.Add(_workFlow_InstanceStepDal.UpdateEntity(item));
                    }
                    if (!list.Contains(false))
                    {
                        msg = "作废成功";
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        msg = "作废失败";
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {

                    msg = "出现未知异常";
                    transaction.Rollback();
                    return false;
                }
                

            }
        }

        public bool DeleteWorkFlow_Instances(List<string> ids)
        {
            foreach (var id in ids)
            {
                WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(id);
                if (workFlow_Instance == null)
                {
                    return false;
                }
                bool exist = _workFlow_InstanceDal.RemoveEntity(workFlow_Instance);
                if (!exist)
                {
                    return false;
                }
            }
            return true;
        }

        public List<InstanceEnumOptionModel> GetStatusOptions()
        {
            
            
            InstanceEnumOptionModel h1 = new InstanceEnumOptionModel();
            h1.Title = WorkFlow_InstanceStatusEnum.作废.ToString();
            h1.Value = WorkFlow_InstanceStatusEnum.作废;

            InstanceEnumOptionModel h2 = new InstanceEnumOptionModel();
            h2.Title = WorkFlow_InstanceStatusEnum.审批中.ToString();
            h2.Value = WorkFlow_InstanceStatusEnum.审批中;

            InstanceEnumOptionModel h3 = new InstanceEnumOptionModel();
            h3.Title = WorkFlow_InstanceStatusEnum.结束.ToString();
            h3.Value = WorkFlow_InstanceStatusEnum.结束;
            List<InstanceEnumOptionModel> list = new List<InstanceEnumOptionModel>();
            list.Add(h1);
            list.Add(h2);
            list.Add(h3);

            return list;
        }

        /// <summary>
        /// 通过id获取工作流实例信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorkFlow_Instance GetWorkFlow_InstanceById(string id)
        {
            return _workFlow_InstanceDal.GetEntityById(id);
        }

        public List<GetWorkFlow_InstanceDTO> GetWorkFlow_InstancesByPage(int limit, int page, WorkFlow_InstanceStatusEnum statusOptions, out int count)
        {
            //工作流实例表左联工作流模板表
            var temp = (from wi in _workFlow_InstanceDal.GetEntities()
                        join wm in _workFlow_ModelDal.GetEntities().Where(x => !x.IsDelete)
                        on wi.ModelId equals wm.Id
                        into tempIM
                        from wim in tempIM.DefaultIfEmpty()

                        join cb in _consumableInfoDal.GetEntities().Where(x => !x.IsDelete)
                        on wi.OutGoodsId equals cb.Id
                        into tempIMC
                        from imc in tempIMC.DefaultIfEmpty()

                        join u in _userInfoDal.GetEntities().Where(x=>!x.IsDelete)
                        on wi.Creator equals u.Id
                        into tempIMCU
                        from imuc in tempIMCU.DefaultIfEmpty()

                        select new
                        {
                           wi.Id,
                           wim.Title,
                           wi.Status,
                           wi.Description,
                           wi.Reason,
                           wi.CreateTime,
                           imuc.UserName,
                           wi.OutNum,
                           imc.ConsumableName                           
                        });
            if (statusOptions.ToString() !="0")
            {
                temp = temp.Where(x => x.Status== statusOptions);
            }

            count = temp.Count();
            //转换数据格式
            var list = temp.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
            {
                x.Id,
                x.Title,
                x.Status,
                x.Description,
                x.Reason,
                x.CreateTime,
                x.UserName,
                x.OutNum,
                x.ConsumableName
            }).ToList();

            var res = list.Select(x =>
              {
                  string createTime = x.CreateTime.ToString("yyyy-mm-dd HH:mm:ss");

                  return new GetWorkFlow_InstanceDTO
                  {
                      Id = x.Id,
                      ModelName = x.Title,
                      Status = x.Status.ToString(),
                      Description = x.Description,
                      Reason = x.Reason,
                      CreateTime = createTime,
                      CreatorName = x.UserName,
                      OutNum = x.OutNum + "",
                      OutGoodsName = x.ConsumableName
                  };
              });

            return res.ToList();

        }

        public bool UpdateWorkFlow_Instance(string id, string modelId, string outGoodsId, int outNum, string description, string reason, string userId)
        {
            WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(id);
            if (workFlow_Instance==null)
            {
                return false;
            }
            workFlow_Instance.ModelId = modelId;
            workFlow_Instance.OutGoodsId = outGoodsId;
            workFlow_Instance.OutNum = outNum;
            workFlow_Instance.Description = description;
            workFlow_Instance.Reason = reason;
            workFlow_Instance.Creator = userId;
            return _workFlow_InstanceDal.UpdateEntity(workFlow_Instance);
        }
    }
}
