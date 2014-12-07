using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libRedundancy.Models
{
    public class FileSystemItem
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public int OwnerID { get; set; }
        public int ParentID { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastChangeDateTime { get; set; }
        public string Hash { get; set; }
        public string MimeType { get; set; }
        public string FilePath { get; set; }
        public string Thumbnail { get; set; }
        public dynamic SizeInBytes { get; set; }
        public string SizeWithUnit { get; set; }
        public string UsedUserAgent { get; set; }
    }
}
