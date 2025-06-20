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

#region MyRegion
//namespace RepositoryBll
//{
//    public class WorkFlow_InstanceStepBll : IWorkFlow_InstanceStepBll
//    {
//        IWorkFlow_InstanceStepDal _workFlow_InstanceStepDal;
//        IUserInfoDal _userInfoDal;
//        IWorkFlow_InstanceDal _workFlow_InstanceDal;
//        IWorkFlow_ModelDal _workFlow_ModelDal;
//        IConsumableInfoDal _consumableInfoDal;
//        IR_UserInfo_RoleInfoDal _iR_UserInfo_RoleInfoDal;
//        RepositoryDBContext _dbcontext;
//        IRoleInfoDal _roleInfoDal;
//        IConsumableRecordDal _consumableRecordDal;
//        IDepartmentInfoDal _departmentInfoDal;
//        public WorkFlow_InstanceStepBll(RepositoryDBContext dbcontext, IDepartmentInfoDal departmentInfoDal, IRoleInfoDal roleInfoDal, IConsumableRecordDal consumableRecordDal, IR_UserInfo_RoleInfoDal iR_UserInfo_RoleInfoDal, IConsumableInfoDal consumableInfoDal, IWorkFlow_ModelDal workFlow_ModelDal, IWorkFlow_InstanceDal workFlow_InstanceDal, IWorkFlow_InstanceStepDal workFlow_InstanceStepDal, IUserInfoDal userInfoDal)
//        {
//            _workFlow_InstanceStepDal = workFlow_InstanceStepDal;
//            _userInfoDal = userInfoDal;
//            _workFlow_InstanceDal = workFlow_InstanceDal;
//            _workFlow_ModelDal = workFlow_ModelDal;
//            _iR_UserInfo_RoleInfoDal = iR_UserInfo_RoleInfoDal;
//            _consumableInfoDal = consumableInfoDal;
//            _roleInfoDal = roleInfoDal;
//            _consumableRecordDal = consumableRecordDal;
//            _departmentInfoDal = departmentInfoDal;
//            _dbcontext = dbcontext;
//        }
//        /// <summary>
//        /// 根据id获取工作流步骤信息
//        /// </summary>
//        /// <param name="stepId"></param>
//        /// <returns></returns>
//        public object GetWorkFlowDetail(string stepId)
//        {
//            ////申请人、申请物品、数量、理由
//            var data = (from wis in _workFlow_InstanceStepDal.GetEntities().Where(x => x.Id == stepId)
//                        join wi in _workFlow_InstanceDal.GetEntities()
//                        on wis.InstanceId equals wi.Id

//                        join u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
//                        on wi.Creator equals u.Id

//                        join c in _consumableInfoDal.GetEntities().Where(x => !x.IsDelete)
//                        on wi.OutGoodsId equals c.Id
//                        select new
//                        {
//                            Id = wis.Id,
//                            CreatorName = u.UserName,
//                            OutGoodsName = c.ConsumableName,
//                            OutNum = wi.OutNum,
//                            Reason = wi.Reason,
//                            Creator = wi.Creator,//申请人ID
//                            ReviewerId = wis.ReviewerId//审核人ID

//                        }).FirstOrDefault();

//            return data;

//        }

//        public List<GetWorkFlow_InstanceStepDTO> GetWorkFlow_InstanceStepsByPage(int limit, int page, string currentUserId, string creatorName, out int count)
//        {
//            //工作流模板名称、申请人名字、审核人名称、审核理由、审核时间、审核状态、添加时间
//            #region 
//            //(from u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
//            // join d in _departmentInfoDal.GetEntities().Where(x => !x.IsDelete && x.LeaderId == currentUserId)
//            // on u.DepartmentId equals d.Id
//            // select u.UserName).FirstOrDefault();
//            #endregion
//            //查询出当前登录人的名字
//            string LeaderName = _userInfoDal.GetEntityById(currentUserId).UserName;
//            //步骤表内联实例表 
//            var tmp = from witep in _workFlow_InstanceStepDal.GetEntities().Where(x => x.ReviewerId == currentUserId)//审核人id于当前登录人id相同
//                      join wi in _workFlow_InstanceDal.GetEntities()
//                      on witep.InstanceId equals wi.Id
//                      //步骤表内联模板表
//                      join wm in _workFlow_ModelDal.GetEntities().Where(x => !x.IsDelete)
//                      on wi.ModelId equals wm.Id
//                      //实例表内联用户表
//                      join u in _userInfoDal.GetEntities().Where(x => !x.IsDelete)
//                      on wi.Creator equals u.Id
//                      //工作实例表内联耗材信息表
//                      join ci in _consumableInfoDal.GetEntities().Where(x => !x.IsDelete)
//                      on wi.OutGoodsId equals ci.Id
//                      select new
//                      {
//                          Id = witep.Id,//主键id
//                          ModelName = wm.Title,//模板名字
//                          CreatorName = u.UserName,//申请人名字
//                          ReviewerName = LeaderName,//审核人名字
//                          OutGoodsName = ci.ConsumableName,//审核物品
//                          OutNum = wi.OutNum,//审核数量
//                          ReviewStatus = witep.ReviewStatus,//审核状态
//                          ReviewTime = witep.ReviewTime, // 审核时间
//                          CreateTime = wi.CreateTime//添加时间
//                      };
//            count = tmp.Count();
//            if (!string.IsNullOrEmpty(creatorName))
//            {
//                tmp = tmp.Where(x => x.CreatorName.Contains(creatorName));
//            }

//            //分页
//            var list = tmp.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
//            {
//                x.Id,
//                x.ModelName,
//                x.CreatorName,
//                x.ReviewerName,
//                x.OutGoodsName,
//                x.OutNum,
//                x.ReviewStatus,
//                x.ReviewTime,
//                x.CreateTime

//            }).ToList();

//            //转换数据格式
//            var res = list.Select(x =>
//            {
//                return new GetWorkFlow_InstanceStepDTO
//                {
//                    Id = x.Id,
//                    ModelName = x.ModelName,
//                    CreatorName = x.CreatorName,
//                    OutGoodsName = x.OutGoodsName,
//                    ReviewerName = x.ReviewerName,
//                    OutNum = x.OutNum + "",
//                    ReviewStatus = x.ReviewStatus.ToString(),
//                    ReviewTime = x.ReviewTime == null ? "" : x.ReviewTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss"),
//                    CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
//                };
//            });

//            return res.ToList();
//        }

//        public bool UpdateWorkFlow_InstanceStep(string id, string reviewReason, InstanceStepStatusEnum reviewStatus, int outNum, string currentUserId, out string msg)
//        {


//            using (var transaction = _dbcontext.Database.BeginTransaction())
//            {
//                try
//                {

//                    WorkFlow_InstanceStep workFlow_InstanceStep = _workFlow_InstanceStepDal.GetEntityById(id);

//                    if (workFlow_InstanceStep == null)
//                    {
//                        msg = "找不到此申请";
//                        //回滚事务
//                        transaction.Rollback();
//                        return false;
//                    }
//                    //判断当前登录人是否有权限审批
//                    if (workFlow_InstanceStep.ReviewerId != currentUserId)
//                    {
//                        msg = "你无权审批";
//                        //回滚事务
//                        transaction.Rollback();
//                        return false;
//                    }
//                    if (workFlow_InstanceStep.ReviewStatus != InstanceStepStatusEnum.审核中)
//                    {
//                        msg = "请勿重复审核";
//                        transaction.Rollback();
//                        return false;
//                    }


//                    //集合保存—操作结果
//                    List<bool> list = new List<bool>();
//                    //修改工作流步骤表
//                    workFlow_InstanceStep.ReviewStatus = reviewStatus;
//                    workFlow_InstanceStep.ReviewReason = reviewReason;
//                    workFlow_InstanceStep.ReviewTime = DateTime.Now;
//                    list.Add(_workFlow_InstanceStepDal.UpdateEntity(workFlow_InstanceStep));


//                    //********************************************当前只做一个仓库管理员的处理*********************************************************************************
//                    //作为仓库管理员的用户id
//                    string userId = (from r in _roleInfoDal.GetEntities().Where(x => !x.IsDelete && x.RoleName == "仓库管理员")
//                                     join ur in _iR_UserInfo_RoleInfoDal.GetEntities()
//                                     on r.Id equals ur.RoleId
//                                     select ur.UserId).ToList().FirstOrDefault();
//                    //判断是否有仓库管理员
//                    if (string.IsNullOrEmpty(userId))
//                    {
//                        msg = "仓库管理员不存在";
//                        transaction.Rollback();
//                        return false;
//                    }

//                    //找出对应的工作流实例
//                    WorkFlow_Instance workFlow_Instance = _workFlow_InstanceDal.GetEntityById(workFlow_InstanceStep.InstanceId);
//                    //找出物品对应的物品信息表
//                    ConsumableInfo consumableInfo = _consumableInfoDal.GetEntityById(workFlow_Instance.OutGoodsId);

//                    if (reviewStatus == InstanceStepStatusEnum.同意)
//                    {
//                        //如果审核人是仓库管理员
//                        if (currentUserId == userId)
//                        {
//                            //结束这条工作流实例

//                            workFlow_Instance.Status = WorkFlow_InstanceStatusEnum.结束;
//                            list.Add(_workFlow_InstanceDal.UpdateEntity(workFlow_Instance));



//                            //耗材记录表添加一条数据
//                            ConsumableRecord consumableRecord = new ConsumableRecord()
//                            {
//                                Id = Guid.NewGuid().ToString(),
//                                ConsumableId = workFlow_Instance.OutGoodsId,
//                                Num = workFlow_Instance.OutNum,
//                                Type = ConsumableRecordTypeEnum.出库,
//                                CreateTime = DateTime.Now,
//                                Creator = currentUserId
//                            };
//                            list.Add(_consumableRecordDal.CreateEntity(consumableRecord));

//                            //如果不够拿出
//                            if (consumableInfo.Num - workFlow_Instance.OutNum <= 0)
//                            {
//                                msg = $"当前库存为{consumableInfo.Num},库存不足!请驳回!";
//                                //回滚事务
//                                transaction.Rollback();
//                                return false;
//                            }
//                            consumableInfo.Num -= workFlow_Instance.OutNum;
//                            list.Add(_consumableInfoDal.UpdateEntity(consumableInfo));

//                        }

//                        else
//                        {
//                            //创建一条新的步骤数据
//                            WorkFlow_InstanceStep entity = new WorkFlow_InstanceStep()
//                            {
//                                Id = Guid.NewGuid().ToString(),
//                                InstanceId = workFlow_InstanceStep.InstanceId,
//                                BeforeStepId = workFlow_InstanceStep.Id,//上一个步骤id
//                                ReviewStatus = InstanceStepStatusEnum.审核中,

//                                CreateTime = DateTime.Now
//                            };
//                            //如果审核人是申请人
//                            if (currentUserId == workFlow_Instance.Creator && workFlow_Instance.Creator == workFlow_InstanceStep.ReviewerId)
//                            {
//                                if (reviewStatus == InstanceStepStatusEnum.驳回)
//                                {
//                                    msg = "申请人只能同意";
//                                    transaction.Rollback();
//                                    return false;
//                                }
//                                //if (outNum>= workFlow_Instance.OutNum)
//                                //{
//                                //    msg = "请您减少申请数量";
//                                //    transaction.Rollback();
//                                //    return false;
//                                //}
//                                if (outNum <= 0)
//                                {
//                                    msg = "申请数量不能小于或等于零";
//                                    transaction.Rollback();
//                                    return false;
//                                }
//                                string LeaderId = (from u in _userInfoDal.GetEntities().Where(x => x.Id == currentUserId && !x.IsDelete)
//                                                   join d in _departmentInfoDal.GetEntities().Where(x => !x.IsDelete)
//                                                   on u.DepartmentId equals d.Id
//                                                   select d.LeaderId).ToList().FirstOrDefault();
//                                if (string.IsNullOrEmpty(LeaderId))
//                                {
//                                    msg = "找不到部门领导";
//                                    transaction.Rollback();
//                                    return false;
//                                }
//                                //审核人id 给申请人的部门领导审核
//                                entity.ReviewerId = LeaderId;
//                                //把申请人审核的步骤同意了
//                                workFlow_InstanceStep.ReviewStatus = reviewStatus;
//                                workFlow_Instance.OutNum = outNum;

//                                list.Add(_workFlow_InstanceStepDal.UpdateEntity(workFlow_InstanceStep));



//                            }
//                            //如果审核人是部门领导
//                            else
//                            {

//                                entity.ReviewerId = userId;//审核人id 给管理员
//                            }

//                            list.Add(_workFlow_InstanceStepDal.CreateEntity(entity));
//                        }
//                    }
//                    //选择驳回
//                    else
//                    {

//                        //创建一条新的工作流步骤数据
//                        WorkFlow_InstanceStep NewworkFlow_InstanceStep = new WorkFlow_InstanceStep()
//                        {
//                            Id = Guid.NewGuid().ToString(),
//                            InstanceId = workFlow_InstanceStep.InstanceId,
//                            ReviewStatus = InstanceStepStatusEnum.审核中,
//                            CreateTime = DateTime.Now

//                        };
//                        //如果是仓库管理员在驳回
//                        if (currentUserId == userId)
//                        {
//                            NewworkFlow_InstanceStep.ReviewerId = _workFlow_InstanceStepDal.GetEntities().Where(x => x.Id == workFlow_InstanceStep.BeforeStepId).ToList().FirstOrDefault().ReviewerId;
//                        }
//                        //如果是部门领导在驳回
//                        else
//                        {
//                            NewworkFlow_InstanceStep.ReviewerId = workFlow_Instance.Creator;
//                        }
//                        list.Add(_workFlow_InstanceStepDal.CreateEntity(NewworkFlow_InstanceStep));
//                    }

//                    if (!list.Contains(false))
//                    {
//                        msg = "审核成功";
//                        transaction.Commit();
//                        return true;
//                    }
//                    else
//                    {
//                        msg = "审核失败";
//                        //回滚事务
//                        transaction.Rollback();
//                        return false;
//                    }


//                }
//                catch (Exception)
//                {

//                    msg = "出现异常";
//                    //回滚事务
//                    transaction.Rollback();
//                    return false;
//                }
//            }
//        }
//    }
//}
#endregion

