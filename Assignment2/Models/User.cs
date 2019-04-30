using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class User
    {
        public int UserId
        {
            get;
            set;
        }
        public BlogPost BlogPost
        {
            get;
            set;
        }
        [ForeignKey("RoleId")]
        public int RoleId
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string EmailAddress
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public DateTime? DateOfBirth
        {
            get;
            set;
        }

        public string City
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string PostalCode
        {
            get;
            set;
        }
        public string Country
        {
            get;
            set;
        }
    }
}
