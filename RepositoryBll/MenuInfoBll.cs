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
    public class MenuInfoBll : IMenuInfoBll
    {
        IMenuInfoDal _menuInfoDal;
        IR_UserInfo_RoleInfoDal _r_UserInfo_RoleInfoDal;
        IR_RoleInfo_MenuInfoDal _r_RoleInfo_MenuInfoDal;
        IUserInfoDal _userInfoDal;



        public MenuInfoBll(IMenuInfoDal menuInfoDal, IR_UserInfo_RoleInfoDal r_UserInfo_RoleInfoDal, IR_RoleInfo_MenuInfoDal r_RoleInfo_MenuInfoDal, IUserInfoDal userInfoDal)
        {
            _menuInfoDal = menuInfoDal;
            _userInfoDal = userInfoDal;
            _r_UserInfo_RoleInfoDal = r_UserInfo_RoleInfoDal;
            _r_RoleInfo_MenuInfoDal = r_RoleInfo_MenuInfoDal;
        }

        public bool CreateMenuInfo(string title, string description, int level, int sort, string href, string parentId, string icon, string target, out string msg)
        {
            //判断部门是否已存在
            bool exist = _menuInfoDal.GetEntities().Any(x => x.Title == title && !x.IsDelete);
            if (exist)
            {
                msg = "菜单已存在";
                return false;
            }
            MenuInfo menuInfo = new MenuInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                Level = level,
                Sort = sort,
                Href = href,
                ParentId = parentId,
                Icon = icon,
                Target = target,
                CreateTime = DateTime.Now
            };
            if (_menuInfoDal.CreateEntity(menuInfo))
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

        public bool DeleteMenuInfo(string id)
        {
            return _menuInfoDal.DeleteEntity(id);
        }

        public bool DeleteMenuInfos(List<string> ids)
        {
            return _menuInfoDal.DeleteEntityByIds(ids);
        }

        public MenuInfo GetMenuInfoById(string id)
        {
            return _menuInfoDal.GetEntityById(id);
        }

        /// <summary>
        /// 获取菜单下拉集
        /// </summary>
        /// <returns></returns>
        public List<OptionsModel> GetmenuInfoOptions()
        {
            List<OptionsModel> list = _menuInfoDal.GetEntities().Where(m => !m.IsDelete).Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.Title
            }).ToList();
            return list;
        }

        public List<GetMenuInfoDTO> GetMenuInfosBypage(int page, int limit, string title, out int count)
        {
            var tempDatas = (from m1 in _menuInfoDal.GetEntities().Where(x => !x.IsDelete)
                             join m2 in _menuInfoDal.GetEntities().Where(x => !x.IsDelete)
                             on m1.ParentId equals m2.Id
                             into TempM1M2
                             from mm2 in TempM1M2.DefaultIfEmpty()
                             
                             select new
                             {
                                 m1.Id,
                                 m1.Title,
                                 m1.Description,
                                 m1.Level,
                                 m1.Sort,
                                 m1.Href,
                                 ParentTitle = mm2.Title,
                                 m1.Icon,
                                 m1.Target,
                                 m1.CreateTime,
                             });

            #region MyRegion
            //var tmp = from mn0 in _menuInfoDal.GetEntities().Where(m => !m.IsDelete)
            //        join mn1 in _menuInfoDal.GetEntities().Where(m => !m.IsDelete)
            //        on mn0.ParentId equals mn1.Id
            //        into tempMN
            //        from mns in tempMN.DefaultIfEmpty()
            //        select new
            //        {
            //            mn0.Id,
            //            mn0.Title,
            //            mn0.Description,
            //            mn0.Level,
            //            mn0.Sort,
            //            mn0.Href,
            //            mn0.ParentId,
            //            mn0.Icon,
            //            mn0.Target,
            //            mn0.CreateTime,
            //            ParentName = mns.Title
            //        };
            #endregion

            
            if (!string.IsNullOrEmpty(title))
            {
                tempDatas = tempDatas.Where(m => m.Title.Contains(title));
            }


            count = tempDatas.Count();


            var list = tempDatas.OrderByDescending(m => m.CreateTime).Skip((page - 1) * limit).Take(limit).Select(m => new
            {
                m.Id,
                m.Title,
                m.Description,
                m.Level,
                m.Sort,
                m.Href,
                m.ParentTitle,
                m.Icon,
                m.Target,
                m.CreateTime
            }).ToList();
            //转换数据格式
            var res = list.Select(m =>
            {
                string createTime = m.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");

                return new GetMenuInfoDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    Level = m.Level+"",
                    Sort = m.Sort+"",
                    Href = m.Href,
                    ParentTitle = m.ParentTitle,
                    Icon = m.Icon,
                    Target = m.Target,
                    CreateTime = createTime,
                };
            });
            return res.ToList();
        }
        /// <summary>
        /// 通过用户Id获取首页左侧菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GetMenuListModelDTO> GetMenuList(string userId)
        {
            #region 0.1,0.2方式
            ////先查询拥有的角色
            //List<string> roleIds= _r_UserInfo_RoleInfoDal.GetEntities().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
            ////查询角色下拥有的菜单
            //List<string> menuIds=_r_RoleInfo_MenuInfoDal.GetEntities().Where(x => roleIds.Contains(x.RoleId)).Select(x => x.MenuId).ToList();

            //内连接-错误写法
            //return (from ur in _r_UserInfo_RoleInfoDal.GetEntities().Where(x => x.UserId == userId)
            //                    join rm in _r_RoleInfo_MenuInfoDal.GetEntities()
            //                    on ur.RoleId equals rm.RoleId
            //                    join m in _menuInfoDal.GetEntities()
            //                    on rm.MenuId equals m.Id
            //                    select m
            //                    ).OrderBy(x => x.Sort).Where(x => !x.IsDelete).Select(x => new GetMenuListModelDTO
            //                    {
            //                        Title = x.Title,
            //                        Href = x.Href,
            //                        Icon = x.Icon,
            //                        Target = x.Target

            //                    }).ToList();
            #endregion

            //查询当前登录是不是管理员
            UserInfo userInfo = _userInfoDal.GetEntityById(userId);
            if (userInfo == null)
            {
                return new List<GetMenuListModelDTO>();
            }
            //内连接获取 当前角色拥有得菜单id集合
            List<string> menuIds=(from ur in _r_UserInfo_RoleInfoDal.GetEntities().Where(x => x.UserId == userId)
                                    join rm in _r_RoleInfo_MenuInfoDal.GetEntities()
                                    on ur.RoleId equals rm.RoleId
                                    select rm.MenuId).ToList();

            //当前角色拥有的菜单
            List<MenuInfo> allMenus;

            if (userInfo.IsAdmin)
            {
                allMenus = _menuInfoDal.GetEntities().Where(x => !x.IsDelete).OrderBy(x => x.Sort).ToList();
            }
            else
            {
                allMenus = _menuInfoDal.GetEntities().Where(x => menuIds.Contains(x.Id) && !x.IsDelete).OrderBy(x => x.Sort).ToList();
            }

            //先查询出一级菜单
            var topMenus= allMenus.Where(x => x.Level == 1).Select(x => new GetMenuListModelDTO
            {
                Id = x.Id,
                Title = x.Title,
                Href = x.Href,
                Icon = x.Icon,
                Target = x.Target

            }).ToList();
            GetMenuChilds(topMenus, allMenus);
            return topMenus;

            #region MyRegion
            //var topMenus = menulist.Where(x => x.Level == 1).Select(x => new GetMenuListModelDTO
            //{
            //    Title = x.Title,
            //    Href = x.Href,
            //    Icon = x.Icon,
            //    Target = x.Target

            //}).ToList();
            ////为一级菜单找子集菜单

            //return _menuInfoDal.GetEntities().Where(x => menuIds.Contains(x.Id) && !x.IsDelete).OrderBy(x => x.Sort).Where(x => !x.IsDelete).Select(x => new GetMenuListModelDTO
            //{
            //    Title = x.Title,
            //    Href = x.Href,
            //    Icon = x.Icon,
            //    Target = x.Target

            //}).ToList();
            #endregion

        }
        /// <summary>
        /// 递归方法找自己的子菜单集合
        /// </summary>
        /// <param name="parentMenus"></param>
        /// <param name="allMenus"></param>
        public void GetMenuChilds(List<GetMenuListModelDTO> parentMenus,List<MenuInfo> allMenus)
        {
            foreach (var parentMenu in parentMenus)
            {
                //
                List<GetMenuListModelDTO> childMenus = allMenus.Where(x => x.ParentId == parentMenu.Id).Select(x => new GetMenuListModelDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Href = x.Href,
                    Icon = x.Icon,
                    Target = x.Target
                }).ToList();

                //调用自己继续找子菜单
                GetMenuChilds(childMenus, allMenus);

                //赋值子菜单
                parentMenu.Child = childMenus;

                
            }
        }

        public List<OptionsModel> GetMenuOptions(string id)
        {
            List<OptionsModel> list = _menuInfoDal.GetEntities().Where(x => x.Id != id && !x.IsDelete).Select(x => new OptionsModel
            {
                Value = x.Id,
                Title = x.Title
            }).ToList();
            return list;
        }

        public bool UpdateMenuInfo(string id, string title, string description, int level, int sort, string href, string parentId, string icon, string target)
        {
            MenuInfo menuInfo = _menuInfoDal.GetEntityById(id);
            if (menuInfo == null)
            {
                return false;
            }
            menuInfo.Title = title;
            menuInfo.Description = description;
            menuInfo.Level = level;
            menuInfo.Sort = sort;
            menuInfo.Href = href;
            menuInfo.ParentId = parentId;
            menuInfo.Icon = icon;
            menuInfo.Target = target;
            return _menuInfoDal.UpdateEntity(menuInfo);
        }
    }
}
