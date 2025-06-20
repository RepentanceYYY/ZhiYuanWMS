using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Enums
{
    /// <summary>
    /// 工作流步骤状态枚举
    /// </summary>
    public enum InstanceStepStatusEnum
    {
        审核中=1,
        同意=2,
        驳回=3,
        作废=4,
        已被他人审批=5
    }
}
