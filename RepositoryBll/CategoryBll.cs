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
    public class CategoryBll : ICategoryBll
    {
        ICategoryDal _categoryDal;

        public CategoryBll(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }
        /// <summary>
        /// 添加耗材
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="description"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateCategory(string categoryName, string description, out string msg)
        {
            Category exist = _categoryDal.GetEntities().FirstOrDefault(x => x.CategoryName == categoryName);
            if (exist !=null)
            {
                msg = "已存在此耗材种类";
                return false;
            }
            Category category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryName = categoryName,
                Description = description
            };

            if (_categoryDal.CreateEntity(category))
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
        /// 真删除耗材信息BLL
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCategory(string id)
        {
            Category category = _categoryDal.GetEntityById(id);
            if (category==null)
            {
                return false;
            }
            return _categoryDal.RemoveEntity(category);
        }
        /// <summary>
        /// 批量真删除耗材种类信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteCategorys(List<string> ids)
        {
            foreach (var id in ids)
            {
                Category category = _categoryDal.GetEntityById(id);
                if (category== null)
                {
                    return false;
                }
                if (!_categoryDal.RemoveEntity(category))
                {
                    return false;
                }                
            }

            return true;
        }

        public Category GetCategoryById(string id)
        {
            return _categoryDal.GetEntityById(id);
        }
        /// <summary>
        /// 获取耗材种类下拉框
        /// </summary>
        /// <returns></returns>
        public List<OptionsModel> GetCategoryOptions()
        {
            List<OptionsModel> list= _categoryDal.GetEntities().Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.CategoryName
            }).ToList();
            return list;
        }

        /// <summary>
        /// 分页获取耗材信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="categoryName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetCategoryDTO> GetCategorysByPage(int limit, int page, string categoryName, out int count)
        {
            IQueryable<Category> categorys = _categoryDal.GetEntities();

            if (!string.IsNullOrEmpty(categoryName))
            {
                categorys = categorys.Where(x => x.CategoryName.Contains(categoryName));
            }

            count = categorys.Count();

            var tmp = categorys.Skip((page - 1) * limit).Take(limit).Select(x => new
                    {
                        x.Id,
                        x.CategoryName,
                        x.Description
                    }).ToList();

            //转换数据格式
            var res = tmp.Select(x =>
            {
                return new GetCategoryDTO
                {
                    Id=x.Id,
                    CategoryName=x.CategoryName,
                    Description=x.Description
                };
            });
            return res.ToList();
        }
        /// <summary>
        /// 编辑耗材种类bll层
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdateCategory(string id, string categoryName, string description)
        {
            Category category = _categoryDal.GetEntityById(id);

            if (category==null)
            {
                return false;
            }
            category.CategoryName = categoryName;
            category.Description = description;

            return _categoryDal.UpdateEntity(category);
        }
    }
}
