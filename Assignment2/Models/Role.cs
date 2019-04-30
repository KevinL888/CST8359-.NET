using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class Role
    {   [Key]
        public int RoleId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

    }
}
