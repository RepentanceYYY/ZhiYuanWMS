using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 工作流步骤表
    /// </summary>
    public class WorkFlow_InstanceStep:BaseEntity
    {

        /// <summary>
        /// 工作流实例Id
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 审核人Id
        /// </summary>
        public string ReviewerId { get; set; }
        /// <summary>
        /// 审核理由
        /// </summary>
        public string ReviewReason { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public InstanceStepStatusEnum ReviewStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ReviewTime { get; set; }
        /// <summary>
        /// 上一个步骤Id
        /// </summary>
        public string BeforeStepId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
