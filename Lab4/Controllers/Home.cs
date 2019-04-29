using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab4.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab4.Controllers
{
    public class Home : Controller
    {

        private MoviesContext _moviesContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Home(MoviesContext context)
        {
            _moviesContext = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_moviesContext.Movie.ToList());
        }

        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMovie(Movie movie)
        {          
                _moviesContext.Movie.Add(movie);
                _moviesContext.SaveChanges();

                return RedirectToAction("Index");
        }

        public IActionResult DeleteMovie(int id)
        {
            var movieToDelete = (from m in _moviesContext.Movie where m.MovieId == id select m).FirstOrDefault();
            _moviesContext.Movie.Remove(movieToDelete);
            _moviesContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            var movieToUpdate = (from m in _moviesContext.Movie where m.MovieId == id select m).FirstOrDefault();

            return View(movieToUpdate);
        }

        [HttpPost]
        public IActionResult ModifyMovie(Movie movie)
        {
            var id = Convert.ToInt32(Request.Form["MovieId"]);

            var movieToUpdate = (from m in _moviesContext.Movie where m.MovieId == id select m).FirstOrDefault();


            try
            {
                movieToUpdate.Title = movie.Title;

                movieToUpdate.SubTitle = movie.SubTitle;

                movieToUpdate.Description = movie.Description;
            
                movieToUpdate.Year = movie.Year;

                movieToUpdate.Rating = movie.Rating;

                if (movieToUpdate.Rating > 5 || movieToUpdate.Rating < 1)
                    throw new Exception();

                _moviesContext.SaveChanges();
            }
            catch
            {
                return StatusCode(500);
            }

            return RedirectToAction("Index");
        }


    }
}
