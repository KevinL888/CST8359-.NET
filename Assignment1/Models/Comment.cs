using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.Models
{
    public class Comment
    {
        public int CommentId
        {
            get;
            set;
        }

        [ForeignKey("BlogPostId")]
        public BlogPost BlogPost
        {
            get;
            set;
        }
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

        public string Content
        {
            get;
            set;
        }
    }
}
