using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DTOModels
{
    public class GetConsumableInfoDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }        
        public string ConsumableName { get; set; }
        public string Specification { get; set; }
        public string Num { get; set; }
        public string Unit { get; set; }
        public string Money { get; set; }
        public string WarningNum { get; set; }
        public string CreateTime { get; set; }
    }
}
