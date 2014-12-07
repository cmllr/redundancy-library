using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libRedundancy.Models
{
    public class User
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string DisplayName { get; set; }
        public string MailAddress { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public string PasswordHash { get; set; }
        public int IsEnabled { get; set; }
        public long ContingentInByte { get; set; }
        public Role Role { get; set; }
        public int FailedLogins { get; set; }
    }
}
