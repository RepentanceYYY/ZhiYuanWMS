using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetWorkFlow_InstanceDTO
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 工作流模板Id
        /// </summary>
        public string ModelId { get; set; }
        /// <summary>
        /// 工作流模板名字
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 申请理由
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 添加人Id
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 添加人姓名
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        ///出库数量
        /// </summary>
        public string OutNum { get; set; }
        /// <summary>
        /// 出库物资Id
        /// </summary>
        public string OutGoodsId { get; set; }
        /// <summary>
        /// 出库物资名字
        /// </summary>
        public string OutGoodsName { get; set; }
    }
}
