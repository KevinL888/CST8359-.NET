using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class BlogPost
    {
        public int BlogPostId
        {
            get;
            set;
        }

        [ForeignKey("UserId")]
        public User User
        {
            get;
            set;
        }
        public int UserId
        {
            get;
            set;
        }
        public List<Comment> Comment
        {
            get;
            set;
        }
        public List<Photo> Photo
        {
            get;
            set;
        }



        public string Title
        {
            get;
            set;
        }

        public string ShortDescription
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public DateTime Posted
        {
            get;
            set;
        }

        public bool IsAvailable
        {
            get;
            set;
        }
    }
}
