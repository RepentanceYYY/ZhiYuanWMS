using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    /// <summary>
    /// 工作流步骤表显示模板
    /// </summary>
    public class GetWorkFlow_InstanceStepDTO
    {
        public string Id { get; set; }//主键id
        public string ModelName { get; set; }//模板名字
        public string CreatorName { get; set; }//申请人名字
        public string ReviewerName { get; set; }//审核人名字
        public string OutNum { get; set; }//审核数量
        public string OutGoodsName { get; set; }//审核物品
        public string ReviewStatus { get; set; }//审核状态
        
        public string ReviewTime { get; set; }// 审核时间
        public string CreateTime { get; set; }//添加时间
    }
}
