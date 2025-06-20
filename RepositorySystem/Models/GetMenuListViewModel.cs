using Entity.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem.Models
{
    public class GetMenuListViewModel
    {
        public GetMenuListViewModel()
        {
            
        }
        public GetMenuListViewModel(List<GetMenuListModelDTO> menus)
        {
            this.MenuInfo.FirstOrDefault().Child = menus;
        }
        public GetMenuListModelDTO HomeInfo { get; set; } = new GetMenuListModelDTO
        {
            Title = "首页",
            Href = "/layuimini/page/welcome-1.html?t=1"
        };
        public GetMenuListModelDTO LogoInfo { get; set; } = new GetMenuListModelDTO
        {
            Title = "致远仓储",
            Image = "/layuimini/images/图标（去底）.png",
            Href = ""
        };
        public List<GetMenuListModelDTO> MenuInfo { get; set; } = new List<GetMenuListModelDTO>()
        {
            new GetMenuListModelDTO
            {
                 Title="常规管理",
                        Icon="fa fa-address-book",
                        Href="",
                        Target="_self"
            }
        };

    }
}
