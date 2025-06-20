using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class WorkFlow_Model:BaseDeleteEntity
    {
        /// <summary>
        /// 工作流模板表(
        /// </summary>
        
        /// <summary>
        /// 模板标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
