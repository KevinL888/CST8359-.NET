using Assignment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.ViewModel
{
    public class BlogPostViewModel
    {
        public List<BlogPosts> BlogPosts { get; set; }
        
    }

    public class BlogPosts
    {
        public Models.BlogPost BlogPost { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public Models.User User { get; set; }
    }

    public class PhotoViewModel
    {
        public Photo Photo { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }

    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string AuthorName { get; set; }
        public string EmailAddress { get; set; }
        public int Rating { get; set; }
    }
}
