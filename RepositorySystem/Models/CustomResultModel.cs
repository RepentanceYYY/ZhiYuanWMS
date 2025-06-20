using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositorySystem.Models
{
    public class CustomResultModel
    {
        /// <summary>
        /// 默认为false
        /// </summary>
        public bool IsSuccess { get; set; } = false;
        /// <summary>
        /// 友好提示
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 1成功2失败3异常4登录失效，请重新登录
        /// </summary>
        public int Status { get; set; } = 2;
        /// <summary>
        /// 数据
        /// </summary>
        public object Datas { get; set; }
    }
}
