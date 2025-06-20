using Entity;
using Entity.DTOModels;
using IRepositoryBll;
using IRepositoryDal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryBll
{
    public class RoleInfoBll : IRoleInfoBll
    {
        IRoleInfoDal _roleInfoDal;
        IR_UserInfo_RoleInfoDal _r_UserInfo_RoleInfoDal;
        IR_RoleInfo_MenuInfoDal _r_RoleInfo_MenuInfoDal;
        public RoleInfoBll(IRoleInfoDal roleInfoDal, IR_UserInfo_RoleInfoDal r_UserInfo_RoleInfoDal, IR_RoleInfo_MenuInfoDal r_RoleInfo_MenuInfoDal)
        {
            _roleInfoDal = roleInfoDal;
            _r_UserInfo_RoleInfoDal = r_UserInfo_RoleInfoDal;
            _r_RoleInfo_MenuInfoDal = r_RoleInfo_MenuInfoDal;
        }

        public bool BindMenuInfo(string roleId, List<string> menuIds)
        {
            //查询当前菜单已经绑定中间表数据集合
            var menuRoleList = _r_RoleInfo_MenuInfoDal.GetEntities().Where(x => x.RoleId == roleId).ToList();

            //先获取已经绑定菜单
            List<string> bindMenuIds = menuRoleList.Select(x => x.MenuId).ToList();

            //解绑操作
            foreach (var menuRole in menuRoleList)
            {
                if (!menuIds.Contains(menuRole.MenuId))
                {
                    _r_RoleInfo_MenuInfoDal.RemoveEntity(menuRole);
                }
            }
            foreach (var menuId in menuIds)
            {
                //如果之前没做过绑定的菜单，做绑定
                if (!bindMenuIds.Contains(menuId))
                {
                    R_RoleInfo_MenuInfo r_RoleInfo_MenuInfo = new R_RoleInfo_MenuInfo() {
                        Id=Guid.NewGuid().ToString(),
                        CreateTime=DateTime.Now,
                        MenuId=menuId,
                        RoleId=roleId
                    };
                    _r_RoleInfo_MenuInfoDal.CreateEntity(r_RoleInfo_MenuInfo);
                }
            }
            return true;
        }

        public bool BindUserInfo(string roleId, List<string> userIds)
        {
            //查询当前角色已经绑定中间表数据集合
            var userRoleList = _r_UserInfo_RoleInfoDal.GetEntities().Where(x => x.RoleId == roleId).ToList();

            //先获取已经绑定用户
            List<string> bindUserIds = userRoleList.Select(x => x.UserId).ToList();

            //解绑操作
            foreach (var userRole in userRoleList)
            {
                if (!userIds.Contains(userRole.UserId))
                {
                    _r_UserInfo_RoleInfoDal.RemoveEntity(userRole);
                }
            }

            //遍历选择绑定的用户集合，做绑定
            foreach (var userId in userIds)
            {
                //如果之前没绑定过的用户，现在绑定
                if (!bindUserIds.Contains(userId))
                {
                    IR_UserInfo_RoleInfo r_UserInfo_RoleInfo = new IR_UserInfo_RoleInfo() {
                        Id = Guid.NewGuid().ToString(),
                        CreateTime=DateTime.Now,
                        RoleId=roleId,
                        UserId=userId
                    };
                    _r_UserInfo_RoleInfoDal.CreateEntity(r_UserInfo_RoleInfo);
                }
            }
            return true;
        }

        /// <summary>
        /// 添加角色业务逻辑层
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CreateRoleInfo(string roleName, string description, out string msg)
        {
            //判断角色是否存在
            bool exist = _roleInfoDal.GetEntities().Any(r => r.RoleName == roleName && !r.IsDelete);
            if (exist)
            {
                msg = "角色已存在";
                return false;
            }
            RoleInfo roleInfo = new RoleInfo()
            {
                Id = Guid.NewGuid().ToString(),
                RoleName=roleName,
                Description=description,
                CreateTime=DateTime.Now
            };
            
            if (_roleInfoDal.CreateEntity(roleInfo))
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
        /// 根据id软删除单个角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRoleInfo(string id)
        {
            return _roleInfoDal.DeleteEntity(id);
        }
        /// <summary>
        /// 软删除多个角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteRoleInfos(List<string> ids)
        {
            return _roleInfoDal.DeleteEntityByIds(ids);
        }

        public List<string> GetBindMenuIds(string roleId)
        {
            return _r_RoleInfo_MenuInfoDal.GetEntities().Where(x => x.RoleId == roleId).Select(x => x.MenuId).ToList();
        }

        /// <summary>
        /// 获取角色已绑定用户id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<string> GetBindUserIds(string roleId)
        {
            return _r_UserInfo_RoleInfoDal.GetEntities().Where(x=>x.RoleId==roleId).Select(x=>x.UserId).ToList();
        }
        /// <summary>
        /// 通过id获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleInfo GetRoleInfoById(string id)
        {
            return _roleInfoDal.GetEntityById(id);
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="roleName"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<GetRoleInfoDTO> GetRoleInfosByPage(int limit, int page, string roleName, out int count)
        {
            IQueryable<RoleInfo> roleInfos=  _roleInfoDal.GetEntities().Where(r=>!r.IsDelete);

            //条件筛选
            if (!string.IsNullOrEmpty(roleName))
            {
                roleInfos = roleInfos.Where(r => r.RoleName.Contains(roleName));
            }

            count = roleInfos.Count();

            var tmp = roleInfos.OrderByDescending(r => r.CreateTime).Skip((page - 1) * limit).Take(limit).Select(r => new
            {
                r.Id,
                r.RoleName,
                r.Description,
                r.CreateTime,

            }).ToList();

            //转换数据格式
            var res = tmp.Select(r =>
            {
                string createTime = r.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                return new GetRoleInfoDTO
                {
                    Id = r.Id,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    CreateTime = createTime
                };
            });
            return res.ToList();
        }
        /// <summary>
        /// 根据id修改单个角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(string id, string roleName, string description)
        {
            RoleInfo roleInfo = _roleInfoDal.GetEntityById(id);
            if (roleInfo==null)
            {
                return false;
            }
            roleInfo.RoleName = roleName;
            roleInfo.Description = description;

            return _roleInfoDal.UpdateEntity(roleInfo);
        }
    }
}
