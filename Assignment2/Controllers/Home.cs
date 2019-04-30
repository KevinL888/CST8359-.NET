using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;
using Microsoft.AspNetCore.Http;
using Assignment2.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment2.Controllers
{
    public class Home : Controller
    {

        private Assignment2DataContext _assign2Context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Home(Assignment2DataContext context)
        {
            _assign2Context = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Role") == null)
            {
                HttpContext.Session.SetString("Role", "notLogged");
            }
            //BlogPostViewModel model = new BlogPostViewModel();
            //model.BlogPosts = new List<BlogPosts>();
            //var listOfblogs = _assign2Context.BlogPost.ToList();
            //foreach(BlogPost blog in listOfblogs)
            //{
            //    BlogPosts blogs = new BlogPosts();
            //    blogs.BlogPost = blog;

            //    blogs.Photos = new List<PhotoViewModel>();
            //    var photoList = (from p in _assign2Context.Photo where p.BlogPostId == blog.BlogPostId select p).ToList();
            //    foreach (Photo photo in photoList)
            //    {              
            //        PhotoViewModel pv = new PhotoViewModel();

            //        pv.Photo = photo;
            //        pv.FileName = photo.Filename;
            //        pv.Url = photo.Url;
            //        blogs.Photos.Add(pv);
            //    }
            //    model.BlogPosts.Add(blogs);
            //}

            return View(_assign2Context.BlogPost.Include(y => y.Photo).ToList());
        }
        [HttpGet]
        public IActionResult DisplayFullBlogPost(int id)
        {
            var subset = _assign2Context.BlogPost.Include(w => w.User).Include(x => x.Photo).Include(y => y.Comment).ThenInclude(z => z.User);
            var blogToDisplay = (from b in subset where b.BlogPostId == id select b).FirstOrDefault();

            return View(blogToDisplay);

            //var blogPost = (from item in _assign2Context.BlogPost where item.BlogPostId == id select item).FirstOrDefault();
            //if (blogPost != null)
            //{
            //    BlogPostViewModel model = new BlogPostViewModel();
            //    model.BlogPost = blogPost;

            //    model.Comments = new List<CommentViewModel>();

            //    var commentList = (from item in _assign2Context.Comment where item.BlogPostId == id select item).ToList();
            //    foreach (Comment com in commentList)
            //    {
            //        CommentViewModel c = new CommentViewModel();
            //        c.Comment = com;
            //        var author = (from user in _assign2Context.User where user.UserId == com.UserId select user).FirstOrDefault();
            //        c.AuthorName = author.FirstName + " " + author.LastName;
            //        c.EmailAddress = author.EmailAddress;
            //        c.Rating = com.Rating;
            //        model.Comments.Add(c);
            //    }

            //    model.Photos = new List<PhotoViewModel>();
            //    var photoList = (from p in _assign2Context.Photo where p.BlogPostId == id select p).ToList();

            //    foreach(Photo photo in photoList)
            //    {
            //        PhotoViewModel p = new PhotoViewModel();
            //        p.Photo = photo;
            //        p.FileName = photo.Filename;
            //        p.Url = photo.Url;
            //        model.Photos.Add(p);
            //    }

            //    model.User = (from user in _assign2Context.User where user.UserId == blogPost.UserId select user).FirstOrDefault();
            //    return View(model);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }

        [HttpPost]
        public IActionResult AddComment()
        {
            Comment comment = new Models.Comment();
            comment.BlogPostId = Convert.ToInt32(Request.Form["BlogPostId"]);
            comment.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            comment.Content = Request.Form["Comment.Content"];
            comment.Rating = Convert.ToInt32(Request.Form["Comment.Rating"]);

            var badWords =  _assign2Context.BadWord;         

            foreach (var word in badWords)
            {
                comment.Content = comment.Content.ToLower().Replace(word.Word.ToLower(), "*****");
               
            }

            _assign2Context.Comment.Add(comment);
            _assign2Context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AddBlogPost()
        {
            return View();
        }

        public IActionResult EditBlogPost(int id)
        {
            var subset = _assign2Context.BlogPost.Include(x => x.Photo).Include(y=> y.User);
            var blogToEdit = (from b in subset where b.BlogPostId == id select b).FirstOrDefault();
            HttpContext.Session.SetInt32("BlogPostId", id);
            return View(blogToEdit);
            //if (blogToEdit != null)
            //{
            //    BlogPostViewModel model = new BlogPostViewModel();
            //    model.BlogPost = blogToEdit;

            //    model.Photos = new List<PhotoViewModel>();

            //    var photoList = (from p in _assign2Context.Photo where p.BlogPostId == id select p).ToList();

            //    foreach (Photo photo in photoList)
            //    {
            //        PhotoViewModel p = new PhotoViewModel();
            //        p.Photo = photo;
            //        p.FileName = photo.Filename;
            //        p.Url = photo.Url;
            //        model.Photos.Add(p);
            //    }

            //    model.User = (from user in _assign2Context.User where user.UserId == blogToEdit.UserId select user).FirstOrDefault();

            //    return View(model);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var existingEmail = (from u in _assign2Context.User where u.EmailAddress == user.EmailAddress select u).FirstOrDefault();

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
                
                    if(user.FirstName == null || user.LastName == null || user.DateOfBirth == null || user.Address == null || user.PostalCode == null|| user.City == null || user.Country == null|| user.Password == null)
                    {
                    TempData["NullField"] = "exist";
                    ViewBag.FirstName = user.FirstName;
                    ViewBag.LastName = user.LastName;
                    ViewBag.Date = user.DateOfBirth;
                    ViewBag.Address = user.Address;
                    ViewBag.PostalCode = user.PostalCode;
                    ViewBag.City = user.City;
                    ViewBag.Country = user.Country;
                    ViewBag.EmailAddress = user.EmailAddress;
                    ViewBag.Role = user.RoleId;

                    return View(user);
                }
                   

                _assign2Context.User.Add(user);
                _assign2Context.SaveChanges();

                return RedirectToAction("Login");
            }

            TempData["Register"] = "exist";
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.Date = user.DateOfBirth;
            ViewBag.Address = user.Address;
            ViewBag.PostalCode = user.PostalCode;
            ViewBag.City = user.City;
            ViewBag.Country = user.Country;
            ViewBag.EmailAddress = user.EmailAddress;
            ViewBag.Role = user.RoleId;

            return View(user);
        }
        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            var ProfileToEdit = (from p in _assign2Context.User where p.UserId == id select p).FirstOrDefault();
            return View(ProfileToEdit);
        }
        [HttpPost]
        public IActionResult EditProfile(User user)
        {
            bool valid = false;

            var existingEmail = (from u in _assign2Context.User where u.EmailAddress == user.EmailAddress select u).FirstOrDefault();

            var id = Convert.ToInt32(Request.Form["UserId"]);

            var profileToUpdate = (from u in _assign2Context.User where u.UserId == id select u).FirstOrDefault();

            if (profileToUpdate.EmailAddress == user.EmailAddress)
            {
                valid = true;
            }
            else if (existingEmail == null)
            {
                valid = true;
            }

            if (valid)
            {
                try
                {
                    profileToUpdate.FirstName = user.FirstName;

                    profileToUpdate.LastName = user.LastName;

                    profileToUpdate.EmailAddress = user.EmailAddress;

                    profileToUpdate.Password = user.Password;

                    if (user.RoleId == 1)
                    {
                        profileToUpdate.RoleId = 1;
                    }
                    else
                    {
                        profileToUpdate.RoleId = 2;
                    }
                    HttpContext.Session.SetString("FirstName", profileToUpdate.FirstName);
                    HttpContext.Session.SetString("LastName", profileToUpdate.LastName);
                    HttpContext.Session.SetString("Role", profileToUpdate.RoleId.ToString());
                    _assign2Context.Entry(profileToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    _assign2Context.SaveChanges();

                }
                catch
                {
                    TempData["NullField"] = "exist";
                    return View(user);
                }

                return RedirectToAction("Index");
            }
            TempData["Edit"] = "exist";
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
            var User = (from u in _assign2Context.User where u.EmailAddress == Request.Form["EmailAddress"] select u).FirstOrDefault();
            if (User != null)
            {
                var validatePassword = (from u in _assign2Context.User where u.Password == Request.Form["Password"] select u).FirstOrDefault();
                if (validatePassword != null)
                {
                    HttpContext.Session.SetString("UserId", User.UserId.ToString());
                    HttpContext.Session.SetString("FirstName", User.FirstName);
                    HttpContext.Session.SetString("LastName", User.LastName);
                    HttpContext.Session.SetString("Role", User.RoleId.ToString());
                    HttpContext.Session.SetString("Email", User.EmailAddress);

                    return RedirectToAction("Index");
                }
            }
            HttpContext.Session.SetString("Email", Request.Form["EmailAddress"]);
            ModelState.AddModelError("Error!", "The email address and password combination not found.");
            return View(user);

        }

         public async Task<IActionResult> SaveBlogPostAndRedirect(BlogPost blog, ICollection<IFormFile> files)
        {
            blog.Posted = DateTime.Now;
            blog.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            _assign2Context.BlogPost.Add(blog);
            _assign2Context.SaveChanges();

            // get your storage accounts connection string
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=kevinlcst8359lab5;AccountKey=afeNQeUy/qzlOdTnKvVYbBYM0MsDjTgB0ipBl5aMUy/o6PZ+jpxEh2gDtB2nCk06AGj06jdMwPhREmVr1FP0MA==;EndpointSuffix=core.windows.net");

            // create an instance of the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // create a container to hold your blob (binary large object.. or something like that)
            // naming conventions for the curious https://msdn.microsoft.com/en-us/library/dd135715.aspx
            var container = blobClient.GetContainerReference("kevinsphotostorage");
            await container.CreateIfNotExistsAsync();

            // set the permissions of the container to 'blob' to make them public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            // for each file that may have been sent to the server from the client
            foreach (var file in files)
            {
                try
                {
                    // create the blob to hold the data
                    var blockBlob = container.GetBlockBlobReference(file.FileName);
                    if (await blockBlob.ExistsAsync())
                        await blockBlob.DeleteAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        // copy the file data into memory
                        await file.CopyToAsync(memoryStream);

                        // navigate back to the beginning of the memory stream
                        memoryStream.Position = 0;

                        // send the file to the cloud
                        await blockBlob.UploadFromStreamAsync(memoryStream);
                    }

                    // add the photo to the database if it uploaded successfully
                    var photo = new Photo();
                    photo.BlogPostId = blog.BlogPostId;
                    photo.Url = blockBlob.Uri.AbsoluteUri;
                    photo.Filename = file.FileName;

                    _assign2Context.Photo.Add(photo);
                    _assign2Context.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditBlogPostAndRedirect(BlogPost blog, ICollection<IFormFile> files)
        {
            var blogId = Convert.ToInt32(Request.Form["BlogPostId"]);

            var blogToUpdate = (from b in _assign2Context.BlogPost where b.BlogPostId == blogId select b).FirstOrDefault();


            try
            {
                blogToUpdate.Title = blog.Title;
                blogToUpdate.ShortDescription = blog.ShortDescription;
                blogToUpdate.Posted = blog.Posted;
                blogToUpdate.IsAvailable = blog.IsAvailable;
                blogToUpdate.Content = blog.Content;

                _assign2Context.Entry(blogToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _assign2Context.SaveChanges();


                // get your storage accounts connection string
                var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=kevinlcst8359lab5;AccountKey=afeNQeUy/qzlOdTnKvVYbBYM0MsDjTgB0ipBl5aMUy/o6PZ+jpxEh2gDtB2nCk06AGj06jdMwPhREmVr1FP0MA==;EndpointSuffix=core.windows.net");

                // create an instance of the blob client
                var blobClient = storageAccount.CreateCloudBlobClient();

                // create a container to hold your blob (binary large object.. or something like that)
                // naming conventions for the curious https://msdn.microsoft.com/en-us/library/dd135715.aspx
                var container = blobClient.GetContainerReference("kevinsphotostorage");
                await container.CreateIfNotExistsAsync();

                // set the permissions of the container to 'blob' to make them public
                var permissions = new BlobContainerPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                await container.SetPermissionsAsync(permissions);

                // for each file that may have been sent to the server from the client
                foreach (var file in files)
                {
                    try
                    {
                        // create the blob to hold the data
                        var blockBlob = container.GetBlockBlobReference(file.FileName);
                        if (await blockBlob.ExistsAsync())
                            await blockBlob.DeleteAsync();

                        using (var memoryStream = new MemoryStream())
                        {
                            // copy the file data into memory
                            await file.CopyToAsync(memoryStream);

                            // navigate back to the beginning of the memory stream
                            memoryStream.Position = 0;

                            // send the file to the cloud
                            await blockBlob.UploadFromStreamAsync(memoryStream);
                        }

                        // add the photo to the database if it uploaded successfully
                        var photo = new Photo();
                        photo.BlogPostId = blogId;
                        photo.Url = blockBlob.Uri.AbsoluteUri;
                        photo.Filename = file.FileName;

                        _assign2Context.Photo.Add(photo);
                        _assign2Context.SaveChanges();
                    }
                    catch
                    {

                    }
                }
                return RedirectToAction("EditBlogPost", new { id =  blogId});


            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult DeleteBlogPost(int id)
        {
            var BlogToDelete = (from b in _assign2Context.BlogPost where b.BlogPostId == id select b).FirstOrDefault();

            var commentList = (from c in _assign2Context.Comment where c.BlogPostId == id select c).ToList();

            var photoList = (from p in _assign2Context.Photo where p.BlogPostId == id select p).ToList();


            foreach (Comment comment in commentList)
            {
                _assign2Context.Comment.Remove(comment);
                _assign2Context.SaveChanges();
            }

            foreach (Photo photo in photoList)
            {
                _assign2Context.Photo.Remove(photo);
                _assign2Context.SaveChanges();
            }

            _assign2Context.BlogPost.Remove(BlogToDelete);
            _assign2Context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult BadWords()
        {
            return View(_assign2Context.BadWord.ToList());
        }

        [HttpPost]
        public IActionResult BadWords(BadWord bad)
        {
            bad.Word = Request.Form["BadWord.Word"];
            var existingWord = (from w in _assign2Context.BadWord where w.Word.ToLower() == bad.Word.ToLower() select w).FirstOrDefault();

            if(existingWord == null)
            {
                _assign2Context.BadWord.Add(bad);
                _assign2Context.SaveChanges();
                return View(_assign2Context.BadWord.ToList());
            }
            TempData["Bad"] = "exist";

            return View(_assign2Context.BadWord.ToList());
        }

        public IActionResult DeleteBadWord(int id)
        {
            var wordToDelete = (from b in _assign2Context.BadWord where b.BadWordId == id select b).FirstOrDefault();
            _assign2Context.BadWord.Remove(wordToDelete);
            _assign2Context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteImageNow(int id)
        {
            var photoToDelete = (from p in _assign2Context.Photo where p.PhotoId == id select p).FirstOrDefault();
            _assign2Context.Photo.Remove(photoToDelete);
            _assign2Context.SaveChanges();

            return RedirectToAction("EditBlogPost", new { id = HttpContext.Session.GetInt32("BlogPostId")});
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ICollection<IFormFile> files)
        {

            // get your storage accounts connection string
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=kevinlcst8359lab5;AccountKey=afeNQeUy/qzlOdTnKvVYbBYM0MsDjTgB0ipBl5aMUy/o6PZ+jpxEh2gDtB2nCk06AGj06jdMwPhREmVr1FP0MA==;EndpointSuffix=core.windows.net");

            // create an instance of the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // create a container to hold your blob (binary large object.. or something like that)
            // naming conventions for the curious https://msdn.microsoft.com/en-us/library/dd135715.aspx
            var container = blobClient.GetContainerReference("kevinsphotostorage");
            await container.CreateIfNotExistsAsync();

            // set the permissions of the container to 'blob' to make them public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            // for each file that may have been sent to the server from the client
            foreach (var file in files)
            {
                try
                {
                    // create the blob to hold the data
                    var blockBlob = container.GetBlockBlobReference(file.FileName);
                    if (await blockBlob.ExistsAsync())
                        await blockBlob.DeleteAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        // copy the file data into memory
                        await file.CopyToAsync(memoryStream);

                        // navigate back to the beginning of the memory stream
                        memoryStream.Position = 0;

                        // send the file to the cloud
                        await blockBlob.UploadFromStreamAsync(memoryStream);
                    }

                    // add the photo to the database if it uploaded successfully
                    var photo = new Photo();
                    photo.BlogPostId = Convert.ToInt32(Request.Form["BlogPostId"]);
                    photo.Url = blockBlob.Uri.AbsoluteUri;
                    photo.Filename = file.FileName;

                    _assign2Context.Photo.Add(photo);
                    _assign2Context.SaveChanges();
                }
                catch
                {

                }
            }
            return RedirectToAction("EditBlogPost", new { id = Convert.ToInt32(Request.Form["BlogPostId"])});
        }

    }
}

