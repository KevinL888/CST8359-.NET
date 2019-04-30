using Assignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment1.ViewModel
{
    public class BlogPostViewModel
    {
        public Models.BlogPost BlogPost { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public Models.User User { get; set; }
    }

    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public string AuthorName { get; set; }
        public string EmailAddress { get; set; }
    }
}
