using Entity;
using Entity.DTOModels;
using Entity.Enums;
using IRepositoryBll;
using IRepositoryDal;
using RepositoryDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryBll
{
    public class WorkFlow_InstanceStepBll : IWorkFlow_InstanceStepBll
    {
        IWorkFlow_InstanceStepDal _workFlow_InstanceStepDal;
        IUserInfoDal _userInfoDal;
        IWorkFlow_InstanceDal _workFlow_InstanceDal;
        IWorkFlow_ModelDal _workFlow_ModelDal;
        IConsumableInfoDal _consumableInfoDal;
        IR_UserInfo_RoleInfoDal _ur_UserInfo_RoleInfoDal;
        RepositoryDBContext _dbcontext;
        IRoleInfoDal _roleInfoDal;
        IConsumableRecordDal _consumableRecordDal;
        IDepartmentInfoDal _departmentInfoDal;
        public WorkFlow_InstanceStepBll(RepositoryDBContext dbcontext, IDepartmentInfoDal departmentInfoDal, IRoleInfoDal roleInfoDal, IConsumableRecordDal consumableRecordDal, IR_UserInfo_RoleInfoDal iR_UserInfo_RoleInfoDal, IConsumableInfoDal consumableInfoDal, IWorkFlow_ModelDal workFlow_ModelDal, IWorkFlow_InstanceDal workFlow_InstanceDal, IWorkFlow_InstanceStepDal workFlow_InstanceStepDal, IUserInfoDal userInfoDal)
        {
            _workFlow_InstanceStepDal = workFlow_InstanceStepDal;
            _userInfoDal = userInfoDal;
            _workFlow_InstanceDal = workFlow_InstanceDal;
            _workFlow_ModelDal = workFlow_ModelDal;
            _ur_UserInfo_RoleInfoDal = iR_UserInfo_RoleInfoDal;
            _consumableInfoDal = consumableInfoDal;
            _roleInfoDal = roleInfoDal;
            _consumableRecordDal = consumableRecordDal;
            _departmentInfoDal = departmentInfoDal;
            _dbcontext = dbcontext;
        }
        /// <summary>
        /// 根据id获取工作流步骤信息
        /// </summary>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public object GetWorkFlowDetail(string stepId)
        {
            ////申请人、申请物品、数量、理由
            var data = (from wis in _workFlow_InstanceStepDal.GetEntities().Where(x => x.Id == stepId)
                        join wi in _workFlow_InstanceDal.GetEntities()
                        on wis.InstanceId equals wi.Id

                        join u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
                        on wi.Creator equals u.Id

                        join c in _consumableInfoDal.GetEntities().Where(x => !x.IsDelete)
                        on wi.OutGoodsId equals c.Id

                        join bfwis in _workFlow_InstanceStepDal.GetEntities()
                        on wis.BeforeStepId equals bfwis.Id
                        into tempBFwiss
                        from wwiss in tempBFwiss.DefaultIfEmpty()

                        join u2 in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
                        on wwiss.ReviewerId equals u2.Id
                        into tempUR
                        from urs in tempUR.DefaultIfEmpty()
                        select new
                        {
                            Id = wis.Id,
                            CreatorName = u.UserName,
                            OutGoodsName = c.ConsumableName,
                            OutNum = wi.OutNum,
                            Reason = wi.Reason,
                            Creator = wi.Creator,//申请人ID
                            ReviewerId = wis.ReviewerId,//审核人ID
                            //上一个步骤的信息
                            BeforeReviewTime=wwiss.ReviewTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                            BeforeReviewStatus = wwiss.ReviewStatus.ToString(),
                            BeforeReviewerUserName = urs.UserName,
                            BeforeReason = wwiss.ReviewReason
                        }).FirstOrDefault();

            return data;

        }

        public List<GetWorkFlow_InstanceStepDTO> GetWorkFlow_InstanceStepsByPage(int limit, int page, string currentUserId, string creatorName, out int count)
        {

            //步骤表内联实例表 
            List<string> Step= _workFlow_InstanceStepDal.GetEntities().Where(x => x.ReviewerId.Contains(currentUserId)).Select(x => x.Id).ToList();

            var tmp = from witep in _workFlow_InstanceStepDal.GetEntities().Where(x=>x.ReviewerId == currentUserId)//审核人群中包含登录人的步骤流
                      join wi in _workFlow_InstanceDal.GetEntities()
                      on witep.InstanceId equals wi.Id
                      //步骤表内联模板表
                      join wm in _workFlow_ModelDal.GetEntities().Where(x => !x.IsDelete)
                      on wi.ModelId equals wm.Id
                      //实例表内联用户表
                      join u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
                      on wi.Creator equals u.Id
                      //工作实例表内联耗材信息表
                      join ci in _consumableInfoDal.GetEntities().Where(x => !x.IsDelete)
                      on wi.OutGoodsId equals ci.Id

                      join u2 in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
                      on witep.ReviewerId equals u2.Id
                      select new
                      {
                          Id = witep.Id,//主键id
                          ModelName = wm.Title,//模板名字
                          CreatorName = u.UserName,//申请人名字
                          ReviewerName = u2.UserName,//审核人名字
                          OutGoodsName = ci.ConsumableName,//审核物品
                          OutNum = wi.OutNum,//审核数量
                          ReviewStatus = witep.ReviewStatus,//审核状态
                          ReviewTime = witep.ReviewTime, // 审核时间
                          CreateTime = wi.CreateTime//添加时间
                      };
            count = tmp.Count();
            if (!string.IsNullOrEmpty(creatorName))
            {
                tmp = tmp.Where(x => x.CreatorName.Contains(creatorName));
            }

            //分页
            var list = tmp.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
            {
                x.Id,
                x.ModelName,
                x.CreatorName,
                x.ReviewerName,
                x.OutGoodsName,
                x.OutNum,
                x.ReviewStatus,
                x.ReviewTime,
                x.CreateTime

            }).ToList();

            //转换数据格式
            var res = list.Select(x =>
              {
                  return new GetWorkFlow_InstanceStepDTO
                  {
                      Id = x.Id,
                      ModelName = x.ModelName,
                      CreatorName = x.CreatorName,
                      OutGoodsName = x.OutGoodsName,
                      ReviewerName = x.ReviewerName,
                      OutNum = x.OutNum + "",
                      ReviewStatus = x.ReviewStatus.ToString(),
                      ReviewTime = x.ReviewTime == null ? "" : x.ReviewTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
                      CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
                  };
              });

            return res.ToList();
        }

        public bool UpdateWorkFlow_InstanceStep(string id, string reviewReason, InstanceStepStatusEnum reviewStatus, int amount, string currentUserId, out string msg)
        {


            using (var transaction = _dbcontext.Database.BeginTransaction())
            {
                try
                {

                    WorkFlow_InstanceStep workFlow_InstanceStep = _workFlow_InstanceStepDal.GetEntityById(id);
                    #region 参数认证
                    
                    if (workFlow_InstanceStep == null)
                    {
                        msg = "找不到此申请";
                        //回滚事务
                        transaction.Rollback();
                        return false;
                    }
                    //判断当前登录人是否有权限审批
                    if (workFlow_InstanceStep.ReviewerId != currentUserId)
                    {
                        msg = "你无权审批";
                        //回滚事务
                        transaction.Rollback();
                        return false;
                    }
                    if (workFlow_InstanceStep.ReviewStatus != InstanceStepStatusEnum.审核中)
                    {
                        msg = "请勿重复审核";
                        transaction.Rollback();
                        return false;
                    }
                    #endregion

                    //集合保存—操作结果
                    List<bool> list = new List<bool>();
                    //修改工作流步骤表
                    workFlow_InstanceStep.ReviewStatus = reviewStatus;
                    workFlow_InstanceStep.ReviewReason = reviewReason;
                    workFlow_InstanceStep.ReviewTime = DateTime.Now;
                    //更新
                    list.Add(_workFlow_InstanceStepDal.UpdateEntity(workFlow_InstanceStep));


                    //查询当前登录人是什么角色（部门领导、仓库管理员）
                    //获取角色名字的集合
                    List<string> roleNames = (from ur in _ur_UserInfo_RoleInfoDal.GetEntities().Where(r => r.UserId == currentUserId)
                                              join r in _roleInfoDal.GetEntities().Where(r => !r.IsDelete)
                                              on ur.RoleId equals r.Id
                                              select r.RoleName).ToList();
                    string roleName = roleNames.FirstOrDefault();

                    if (string.IsNullOrEmpty(roleName))
                    {
                        msg = "你没有权限处理";
                        transaction.Rollback();
                        return false;
                    }
                    //仓库管理员在操作
                    if (roleName == "仓库管理员")
                    {
                        if (reviewStatus == InstanceStepStatusEnum.同意)
                        {
                            //查询工作流实例
                            WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(workFlow_InstanceStep.InstanceId);
                            if (workFlow_Instance == null)
                            {
                                msg = "未获取到工作流实例";
                                transaction.Rollback();
                                return false;
                            }

                            //修改状态
                            workFlow_Instance.Status = WorkFlow_InstanceStatusEnum.结束;
                            list.Add(_workFlow_InstanceDal.UpdateEntity(workFlow_Instance));

                            //查询耗材表
                            ConsumableInfo consumableInfo = _consumableInfoDal.GetEntityById(workFlow_Instance.OutGoodsId);
                            if (consumableInfo == null)
                            {
                                msg = "为获取到耗材信息";
                                transaction.Rollback();
                                return false;
                            }
                            #region 判断当前耗材库存与入库出库记录是否正确

                            //List<ConsumableRecord> consumableRecords = _consumableRecordDal.GetEntities().Where(c => c.ConsumableId == consumableInfo.Id).ToList();

                            ////计算入库数量
                            //int inputSum = consumableRecords.Where(c => c.Type == ConsumableRecordTypeEnum.入库).Sum(c => c.Num);

                            ////计算出库数量
                            //int outSum = consumableRecords.Where(c => c.Type == ConsumableRecordTypeEnum.出库).Sum(c => c.Num);

                            ////如果当前耗材的库存不等于入库去出库的数量，证明数据已经出问题了
                            //if (inputSum - outSum != consumableInfo.Num)
                            //{
                            //    msg = "库存有误，请联系管理员";
                            //    transaction.Rollback();
                            //    return false;
                            //}
                            #endregion

                            //判断当前库存是否充足
                            if (consumableInfo.Num - workFlow_Instance.OutNum < 0)
                            {
                                msg = "库存不足";
                                transaction.Rollback();
                                return false;
                            }

                            //减库存
                            consumableInfo.Num -= workFlow_Instance.OutNum;
                            //更新库存
                            list.Add(_consumableInfoDal.UpdateEntity(consumableInfo));

                            //添加一条出库记录
                            ConsumableRecord consumableRecord = new ConsumableRecord()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ConsumableId = consumableInfo.Id,
                                CreateTime = DateTime.Now,
                                Creator = currentUserId,
                                Num = workFlow_Instance.OutNum,
                                Type = ConsumableRecordTypeEnum.出库
                            };

                            list.Add(_consumableRecordDal.CreateEntity(consumableRecord));

                            
                        }
                        else //驳回
                        {
                            //给上一个步骤的人处理，也正好是部门经理
                            WorkFlow_InstanceStep BefoStep = _workFlow_InstanceStepDal.GetEntityById(workFlow_InstanceStep.BeforeStepId);
                            if (BefoStep == null)
                            {
                                msg = "未获取到上一个步骤";
                                transaction.Rollback();
                                return false;
                            }

                            //给部门经理创建一条新的步骤信息
                            WorkFlow_InstanceStep entity = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = workFlow_InstanceStep.Id,
                                CreateTime = DateTime.Now,
                                InstanceId = workFlow_InstanceStep.InstanceId,
                                ReviewerId = BefoStep.ReviewerId,//上个步骤的审核人 也就是部门经理
                                ReviewStatus = InstanceStepStatusEnum.审核中
                            };

                            list.Add(_workFlow_InstanceStepDal.CreateEntity(entity));
                        }
                        //查询其他仓库管理员的工作流步骤信息，把状态改成"已被他人审核"，排除掉当前审核步骤信息
                        List<WorkFlow_InstanceStep> otherSteps = _workFlow_InstanceStepDal.GetEntities().Where(w => w.BeforeStepId == workFlow_InstanceStep.BeforeStepId && w.Id != workFlow_InstanceStep.Id).ToList();

                        //计算
                        int count = 0;
                        foreach (var item in otherSteps)
                        {
                            item.ReviewStatus = InstanceStepStatusEnum.已被他人审批;
                            item.ReviewTime = DateTime.Now;
                            if (_workFlow_InstanceStepDal.UpdateEntity(item))
                            {
                                count++;
                            }
                        }
                        list.Add(count == otherSteps.Count);

                    }
                    else if (roleName == "部门主管")
                    {
                        if (reviewStatus == InstanceStepStatusEnum.同意)
                        {
                            List<string> roleUserIds = (from r in _roleInfoDal.GetEntities().Where(r => r.RoleName == "仓库管理员" && !r.IsDelete)
                                                        join ur in _ur_UserInfo_RoleInfoDal.GetEntities()
                                                        on r.Id equals ur.RoleId
                                                        select ur.UserId).ToList();
                            if (roleUserIds.Count <=0)
                            {
                                msg = "未获取到仓库管理员";
                                transaction.Rollback();
                                return false;
                            }
                            //计数
                            int count = 0;
                            foreach (var roleUserId in roleUserIds)
                            {
                                //给每个仓库管理员创建一条仓库步骤信息
                                WorkFlow_InstanceStep entity = new WorkFlow_InstanceStep()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BeforeStepId = workFlow_InstanceStep.Id,
                                    CreateTime = DateTime.Now,
                                    InstanceId = workFlow_InstanceStep.InstanceId,
                                    ReviewerId = roleUserId,
                                    ReviewStatus = InstanceStepStatusEnum.审核中
                                };
                                //添加
                                if (_workFlow_InstanceStepDal.CreateEntity(entity))
                                {
                                    count++;
                                }
                            }
                            list.Add(count == roleUserIds.Count);
                        }
                        else//驳回
                        {
                            //查询申请人用户Id，查询工作流实例信息
                            WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(workFlow_InstanceStep.InstanceId);
                            if (workFlow_Instance == null)
                            {
                                msg = "未获取到工作流实例信息";
                                transaction.Rollback();
                                return false;
                            }
                            //给申请人创建一条工作流步骤信息
                            WorkFlow_InstanceStep entity = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = workFlow_InstanceStep.Id,
                                CreateTime = DateTime.Now,
                                InstanceId = workFlow_InstanceStep.InstanceId,
                                ReviewerId = workFlow_Instance.Creator,
                                ReviewStatus = InstanceStepStatusEnum.审核中
                            };
                            list.Add(_workFlow_InstanceStepDal.CreateEntity(entity));
                        }
                    }
                    else // 申请人 普通员工
                    {
                        if (reviewStatus == InstanceStepStatusEnum.同意)
                        {
                            //修改当时申请数量
                            WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(workFlow_InstanceStep.InstanceId);
                            if (workFlow_Instance == null)
                            {
                                msg = "未获取到工作流实例信息";
                                //回滚事务
                                transaction.Rollback();
                                return false;
                            }
                            //修改申请耗材的数量
                            workFlow_Instance.OutNum = amount;
                            list.Add(_workFlow_InstanceDal.UpdateEntity(workFlow_Instance));

                            //查询上一个步骤的信息 获取上一个步骤的审核人
                            WorkFlow_InstanceStep BeforeStep = _workFlow_InstanceStepDal.GetEntityById(workFlow_InstanceStep.BeforeStepId);
                            if (BeforeStep == null)
                            {
                                msg = "未找到上一个工作流步骤";
                                //回滚事务
                                transaction.Rollback();
                                return false;
                            }

                            //创建一条步骤信息给部门经理去审核
                            WorkFlow_InstanceStep entity = new WorkFlow_InstanceStep()
                            {
                                Id = Guid.NewGuid().ToString(),
                                BeforeStepId = workFlow_InstanceStep.Id,
                                CreateTime = DateTime.Now,
                                InstanceId = workFlow_InstanceStep.InstanceId,
                                ReviewerId = BeforeStep.ReviewerId,
                                ReviewStatus = InstanceStepStatusEnum.审核中
                            };
                            list.Add(_workFlow_InstanceStepDal.CreateEntity(entity));
                        }
                        else
                        {
                            msg = "状态有误";
                            //回滚事务
                            transaction.Rollback();
                            return false;
                        }
                    }



                    if (!list.Contains(false))
                    {
                        msg = "审核成功";
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        msg = "审核失败";
                        //回滚事务
                        transaction.Rollback();
                        return false;
                    }


                }
                catch (Exception)
                {

                    msg = "出现异常";
                    //回滚事务
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
