using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 耗材信息实体类
    /// </summary>
    public class ConsumableInfo:BaseDeleteEntity
    {
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string ConsumableName { get; set; }
        public string Specification { get; set; }
        public int Num { get; set; }
        public string Unit { get; set; }
        public decimal Money { get; set; }
        public int WarningNum { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
