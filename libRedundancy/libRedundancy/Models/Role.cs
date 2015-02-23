using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedundancyAccessLibrary.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<int> Permissions { get; set; }
        public int IsDefault { get; set; }
    }
}
