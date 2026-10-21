using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_DB.Models
{
    class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;

        public override string ToString()
        {
            return $" |{GroupId}| группа {GroupName} ";
        }
    }
}

