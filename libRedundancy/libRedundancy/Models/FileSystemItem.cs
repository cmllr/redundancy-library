using System;
using System.Runtime.Serialization;

namespace RedundancyLibrary.Models
{
    [DataContract]
    public class FileSystemItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public int OwnerID { get; set; }

        [DataMember]
        public int ParentID { get; set; }

        [DataMember]
        public DateTime CreateDateTime { get; set; }

        [DataMember]
        public DateTime LastChangeDateTime { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public string MimeType { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Thumbnail { get; set; }

        [DataMember]
        public dynamic SizeInBytes { get; set; }

        [DataMember]
        public string SizeWithUnit { get; set; }

        [DataMember]
        public string UsedUserAgent { get; set; }
    }
}