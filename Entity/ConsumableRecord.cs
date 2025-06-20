using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 耗材记录实体类
    /// </summary>
    public class ConsumableRecord:BaseEntity
    {
        public string ConsumableId { get; set; }
        public int Num { get; set; }
        /// <summary>
        /// 1 入库 2出库
        /// </summary>
        public ConsumableRecordTypeEnum Type { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
    }
}
