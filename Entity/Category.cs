using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 耗材类别实体类
    /// </summary>
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }

    }
}
