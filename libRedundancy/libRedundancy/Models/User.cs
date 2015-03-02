using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RedundancyLibrary.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string LoginName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string MailAddress { get; set; }

        [DataMember]
        public DateTime RegistrationDateTime { get; set; }

        [DataMember]
        public DateTime LastLoginDateTime { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public int IsEnabled { get; set; }

        [DataMember]
        public long ContingentInByte { get; set; }

        [DataMember]
        public Role Role { get; set; }

        [DataMember]
        public int FailedLogins { get; set; }
    }
}
