using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class BaseDeleteEntity:BaseEntity
    {
        public bool IsDelete { get; set; }
        public DateTime DeleteTime { get; set; }
    }
}
