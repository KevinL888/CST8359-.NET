using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment1.Models;
using Microsoft.AspNetCore.Http;
using Assignment1.ViewModel;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment1.Controllers
{
    public class Home : Controller
    {

        private Assignment1DataContext _assign1Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Home(Assignment1DataContext context)
        {
            _assign1Context = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_assign1Context.BlogPost.ToList());
        }


        [HttpGet]
        public IActionResult DisplayFullBlogPost(int id)
        {
            var subset = _assign1Context.BlogPost.Include(x => x.User).Include(y => y.Comment).ThenInclude(z => z.User);
            var blogToDisplay = (from b in subset where b.BlogPostId == id select b).FirstOrDefault();

            return View(blogToDisplay);


            //var blogPost = (from item in _assign1Context.BlogPost where item.BlogPostId == id select item).FirstOrDefault();
            //if (blogPost != null)
            //{
            //    BlogPostViewModel model = new BlogPostViewModel();
            //    model.BlogPost = blogPost;
            //    model.Comments = new List<CommentViewModel>();

            //    var commentList = (from item in _assign1Context.Comment where item.BlogPostId == id select item).ToList();
            //    foreach (Comment com in commentList)
            //    {
            //        CommentViewModel c = new CommentViewModel();
            //        c.Comment = com;
            //        var author = (from user in _assign1Context.User where user.UserId == com.UserId select user).FirstOrDefault();
            //        c.AuthorName = author.FirstName + " " + author.LastName;
            //        c.EmailAddress = author.EmailAddress;
            //        model.Comments.Add(c);
            //    }

            //    model.User = (from user in _assign1Context.User where user.UserId == blogPost.UserId select user).FirstOrDefault();
            //    return View(model);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }

        [HttpPost]
        public IActionResult DisplayFullBlogPost()
        {
            Comment comment = new Models.Comment();
            comment.BlogPostId = Convert.ToInt32(Request.Form["BlogPostId"]);
            comment.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            comment.Content = Request.Form["Comment.Content"];

            _assign1Context.Comment.Add(comment);
            _assign1Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        public IActionResult EditBlogPost(int id)
        {
            var blogToEdit = (from b in _assign1Context.BlogPost where b.BlogPostId == id select b).FirstOrDefault();
            return View(blogToEdit);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAndRedirect(User user)
        {
            var existingEmail = (from u in _assign1Context.User where u.EmailAddress == user.EmailAddress select u).FirstOrDefault();

            if (existingEmail == null)
            {
                if (Request.Form["User.RoleId"] == "1")
                {
                    user.RoleId = 1;
                }
                else
                {
                    user.RoleId = 2;
                }

                _assign1Context.User.Add(user);
                _assign1Context.SaveChanges();

                return RedirectToAction("Login");
            }

            TempData["Register"] = "exist";
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LogoutAndRedirect()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult LoginAndRedirect(User user)
        {
            var User = (from u in _assign1Context.User where u.EmailAddress == Request.Form["EmailAddress"] select u).FirstOrDefault();
            if (User != null)
            {
                var validatePassword = (from u in _assign1Context.User where u.Password == Request.Form["Password"] select u).FirstOrDefault();
                if (validatePassword != null)
                {
                    HttpContext.Session.SetString("UserId", User.UserId.ToString());
                    HttpContext.Session.SetString("FirstName", User.FirstName);
                    HttpContext.Session.SetString("LastName", User.LastName);
                    HttpContext.Session.SetString("Role", User.RoleId.ToString());
                    return RedirectToAction("Index");
                }
            }
            HttpContext.Session.SetString("Email", Request.Form["EmailAddress"]);
            ModelState.AddModelError("Error!", "The email address and password combination not found.");
            return View(user);

        }

        public IActionResult SaveBlogPostAndRedirect(BlogPost blog)
        {
            blog.Posted = DateTime.Now;
            blog.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            _assign1Context.BlogPost.Add(blog);
            _assign1Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EditBlogPostAndRedirect(BlogPost blog)
        {
            var id = Convert.ToInt32(Request.Form["BlogPostId"]);

            var blogToUpdate = (from b in _assign1Context.BlogPost where b.BlogPostId == id select b).FirstOrDefault();


            try
            {
                blogToUpdate.Title = blog.Title;

                blogToUpdate.Content = blog.Content;

                _assign1Context.Entry(blogToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _assign1Context.SaveChanges();

            }
            catch
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index");
        }
    
        public IActionResult DeleteBlogPost(int id)
        {
            var BlogToDelete = (from b in _assign1Context.BlogPost where b.BlogPostId == id select b).FirstOrDefault();

            var commentList = (from item in _assign1Context.Comment where item.BlogPostId == id select item).ToList();

            foreach(Comment comment in commentList)
            {
                _assign1Context.Comment.Remove(comment);
            }
            _assign1Context.BlogPost.Remove(BlogToDelete);
            _assign1Context.Entry(BlogToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _assign1Context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
