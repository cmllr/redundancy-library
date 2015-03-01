using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RedundancyLibrary.Models
{
    [DataContract]
    public sealed class FileSystemChangeInfo
    {
        #region properties

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [DataMember]
        public string Day { get; set; }

        [DataMember]
        public string DayTime { get; set; }

        [DataMember(Name = "Source")]
        public string Type { get; set; }

        [DataMember]
        public string Hash { get; set; }

        #endregion
    }
}