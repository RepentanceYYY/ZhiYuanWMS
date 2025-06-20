using Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class FileInfo:BaseEntity
    {
        public string RelationId { get; set; }
        public string OldFileName { get; set; }
        public string NewFileName { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public FileInfoCategoryEnum Category { get; set; }
    }
}
