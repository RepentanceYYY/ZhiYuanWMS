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
    public class WorkFlow_ModelBll : IWorkFlow_ModelBll
    {
        IWorkFlow_ModelDal _workFlow_ModelDal;
        public WorkFlow_ModelBll(IWorkFlow_ModelDal workFlow_ModelDal)
        {
            _workFlow_ModelDal = workFlow_ModelDal;
        }
        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateWorkFlow_Model(string title, string description, out string msg)
        {
            //判断模板是否存在
            bool exist = _workFlow_ModelDal.GetEntities().Any(r => r.Title == title && !r.IsDelete);
            if (exist)
            {
                msg = "模板已存在";
                return false;
            }
            WorkFlow_Model workFlow_Model = new WorkFlow_Model()
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                CreateTime = DateTime.Now
            };

            if (_workFlow_ModelDal.CreateEntity(workFlow_Model))
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
        /// 分页获取工作模板
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetWorkFlow_ModelDTO> GetWorkFlow_ModelsByPage(int limit, int page, string title, out int count)
        {
            IQueryable<WorkFlow_Model> workFlow_Model = _workFlow_ModelDal.GetEntities().Where(x => !x.IsDelete);

            //条件筛选
            if (!string.IsNullOrEmpty(title))
            {
                workFlow_Model = workFlow_Model.Where(x => x.Title.Contains(title));
            }

            count = workFlow_Model.Count();

            var tmp = workFlow_Model.OrderByDescending(x => x.CreateTime).Skip((page - 1) * limit).Take(limit).Select(x => new
            {
                x.Id,
                x.Title,
                x.CreateTime,
                x.Description
            }).ToList();

            //格式转换
            var res = tmp.Select(x =>
              {
                  string createTime = x.CreateTime.ToString("yyyy-MM-dd");
                  return new GetWorkFlow_ModelDTO
                  {
                      Id = x.Id,
                      Title=x.Title,
                      Description=x.Description,
                      CreateTime= createTime
                  };
              });
            return res.ToList();
        }

        public WorkFlow_Model GetWorkFlow_ModelById(string id)
        {
            return _workFlow_ModelDal.GetEntityById(id);
        }

        public bool UpdateWorkFlow_Model(string id, string title, string description)
        {
            WorkFlow_Model workFlow_Model = _workFlow_ModelDal.GetEntityById(id);
            if (workFlow_Model==null)
            {
                return false;
            }
            workFlow_Model.Title = title;
            workFlow_Model.Description = description;
            
            return _workFlow_ModelDal.UpdateEntity(workFlow_Model);
        }

        public bool DeleteWorkFlow_Model(string id)
        {
            return _workFlow_ModelDal.DeleteEntity(id);
        }

        public bool DeleteWorkFlow_Models(List<string> ids)
        {
            return _workFlow_ModelDal.DeleteEntityByIds(ids);
        }
        /// <summary>
        /// 获取工作流模板下拉集
        /// </summary>
        /// <returns></returns>
        public List<OptionsModel> GetWorkFlow_ModelOptions()
        {
            List<OptionsModel> list = _workFlow_ModelDal.GetEntities().Where(m => !m.IsDelete).Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.Title
            }).ToList();
            return list;
        }
    }
}
