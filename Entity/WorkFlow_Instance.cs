using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 工作流实例表
    /// </summary>
    public class WorkFlow_Instance:BaseEntity
    {
        /// <summary>
        /// ModelId
        /// </summary>
        public string ModelId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public WorkFlow_InstanceStatusEnum Status { get; set; }
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
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 添加人Id
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        ///出库数量
        /// </summary>
        public int OutNum { get; set; }
        /// <summary>
        /// 出库物资Id
        /// </summary>
        public string OutGoodsId { get; set; }

    }
}
