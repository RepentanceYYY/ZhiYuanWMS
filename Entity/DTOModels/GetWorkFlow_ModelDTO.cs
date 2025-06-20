using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetWorkFlow_ModelDTO
    {
        /// <summary>
        /// 工作流模板表(
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
