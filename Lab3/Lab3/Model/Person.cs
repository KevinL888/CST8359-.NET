using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Core.Models
{
    public class Person
    {
        [Required]
        [StringLength(100)]
        public string FirstName
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string LastName
        {
            get;
            set;
        }    
        public string Age
        {
            get;
            set;
        }
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string EmailAddress
        {
            get;
            set;
        }

        [Required]
        public string DateofBirth
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string Password
        {
            get;
            set;
        }

        [Required]
        [StringLength(100)]
        public string Description
        {
            get;
            set;
        }


    }
}

