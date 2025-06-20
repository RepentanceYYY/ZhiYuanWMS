using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using IRepositoryDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryBll
{
    public class DepartmentInfoBll : IDepartmentInfoBll
    {
        IDepartmentInfoDal _departmentDal;
        IUserInfoDal _userInfoDal;
        public DepartmentInfoBll(IDepartmentInfoDal departmentDal,IUserInfoDal userInfoDal)
        {
            _departmentDal = departmentDal;
            _userInfoDal = userInfoDal;
        }

        public bool CreateDepartmentInfo(string departmentName, string leaderId, string parentId, string description, out string msg)
        {

            //判断部门是否已存在
            bool exist = _departmentDal.GetEntities().Any(x => x.DepartmentName == departmentName && !x.IsDelete);
            if (exist)
            {
                msg = "部门已存在";
                return false;
            }
            DepartmentInfo departmentInfo = new DepartmentInfo()
            {
                Id = Guid.NewGuid().ToString(),
                DepartmentName = departmentName,
                LeaderId = leaderId,
                ParentId = parentId,
                Description = description,
                CreateTime = DateTime.Now
            };
            if (_departmentDal.CreateEntity(departmentInfo))
            {
                msg = "添加成功";
                return true;
            }
            else
            {
                msg = "添加失败";
                return false;
            }



        }
        /// <summary>
        /// 删除单个用户业务逻辑层
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteDepartmentInfo(string id)
        {
            return _departmentDal.DeleteEntity(id);
        }

        public bool DeleteDepartmentInfos(List<string> ids)
        {
            return _departmentDal.DeleteEntityByIds(ids);
        }

        public DepartmentInfo GetDepartmentInfoById(string id)
        {
            return _departmentDal.GetEntityById(id);
        }

        public List<GetDepartmentInfoDTO> GetDepartmentInfosBypage(int page, int limit,string departmentName, out int count)
        {

            var temp=   from dt1 in _departmentDal.GetEntities().Where(d => !d.IsDelete)
                        join u in _userInfoDal.GetEntities().Where(u => !u.IsDelete)
                        on dt1.LeaderId equals u.Id
                        into tempDU
                        from u2 in tempDU.DefaultIfEmpty()

                        join dt2 in _departmentDal.GetEntities().Where(d => !d.IsDelete)
                        on dt1.ParentId equals dt2.Id
                        into tempDD
                        from ds in tempDD.DefaultIfEmpty()
                        select new
                        {
                            dt1.Id,
                            dt1.DepartmentName,
                            dt1.Description,
                            dt1.ParentId,
                            dt1.LeaderId,
                            dt1.CreateTime,
                            LeaderNmae = u2.UserName,
                            ParentName = ds.DepartmentName
                        };

            //IQueryable < DepartmentInfo > departmentInfos = _departmentDal.GetDepartmentInfos().Where(x => x.IsDelete == false);
            if (!string.IsNullOrEmpty(departmentName))
            {
                temp = temp.Where(x => x.DepartmentName.Contains(departmentName));
            }
            
            count = temp.Count();

            var list = temp.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
            {
                x.Id,
                x.Description,
                x.DepartmentName,
                x.LeaderId,
                x.ParentId,
                x.CreateTime,
                x.LeaderNmae,
                x.ParentName
            }).ToList();
            //转换数据格式
            var res = list.Select(x =>
            {
                //string createTime = x.CreateTime;

                return new GetDepartmentInfoDTO
                {
                    Id = x.Id,
                    Description = x.Description,
                    DepartmentName = x.DepartmentName,
                    LeaderId = x.LeaderId,
                    ParentId = x.ParentId,
                    CreateTime = x.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    LeaderName=x.LeaderNmae,
                    ParentName=x.ParentName
                };
            });
            return res.ToList();
        }
        /// <summary>
        /// 获取部门下拉选项集
        /// </summary>
        public List<OptionsModel> GetDepartmentOptions()
        {
            List<OptionsModel> list= _departmentDal.GetEntities().Where(x => !x.IsDelete).Select(x => new OptionsModel
            {
                Value=x.Id,
                Title= x.DepartmentName
            }).ToList();
            return list;
        }
        public List<OptionsModel> GetDepartmentOptions(string departmentId)
        {
            List<OptionsModel> list = _departmentDal.GetEntities().Where(x => x.Id !=departmentId && !x.IsDelete).Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.DepartmentName
            }).ToList();
            return list;
        }

        public bool UpdateDepartmentInfo(string id, string departmentName, string leaderId, string parentId, string description)
        {
            DepartmentInfo departmentInfo = _departmentDal.GetEntityById(id);
            if (departmentInfo == null)
            {
                return false;
            }
                departmentInfo.Id = id;
                departmentInfo.DepartmentName= departmentName;
                departmentInfo.LeaderId= leaderId;
                departmentInfo.ParentId= parentId;
                departmentInfo.Description = description;
            return _departmentDal.UpdateEntity(departmentInfo);
        }
    }
}
