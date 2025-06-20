using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    /// <summary>
    /// 下拉选项集
    /// </summary>
    public class OptionsModel
    {
        public string Value { get; set; }
        public string  Title { get; set; }
        public ConsumableRecordTypeEnum consumableRecordTypeEnum { get; set; }


    }
}
