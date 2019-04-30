using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab6.Controllers
{
    public class Home : Controller
    {
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            // get the tweets from the cloud
            var client = new HttpClient();
            var result = await client.GetAsync("http://cst8359.hopto.org/lab6server/api/twitter/getlast100tweets");

            // the get the string content from the clients call to the server
            var content = await result.Content.ReadAsStringAsync();

            // convert the content into a list of tweet objects
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(content);

            // pass them to the view
            return View(tweets.OrderByDescending(o => o.TweetId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostTweet()
        {
            // collect the tweet data from the form
            var tweet = new Tweet();
            tweet.Username = HttpContext.Request.Form["username"];
            tweet.Content = HttpContext.Request.Form["content"];

            // turn the object into a string
            var json = JsonConvert.SerializeObject(tweet);

            // create the httppost header of type stringcontent
            var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // post the result to the cloud
            var client = new HttpClient();
            await client.PostAsync("http://cst8359.hopto.org/lab6server/api/twitter/add", httpContent);

            // return to the view
            return RedirectToAction("Index");
        }
    }
}
