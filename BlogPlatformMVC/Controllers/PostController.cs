using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogPlatformMVC.Models;
using Newtonsoft.Json;

namespace BlogPlatformMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly string BaseURL = "http://ec2-54-234-4-13.compute-1.amazonaws.com/";
        // GET: PostController
        public async Task<ActionResult> Index()
        {
            //Will contain lists of posts from API
            List<Post>? posts = new List<Post>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Post");
                if (response.IsSuccessStatusCode)
                {
                    var Response =  await response.Content.ReadAsStringAsync();

                    // Assigns all the posts from Response by deserializng Json to Model object
                    posts = JsonConvert.DeserializeObject<List<Post>>(Response);
                }
                return View(posts);
            }
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var post = new Post();
            using(var client = new HttpClient())
            {
                client.BaseAddress=new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/Post/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<Post>(content);
                }
                return View(post);
            }
        }

        // GET: PostController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post)
        {
            var newPost = new Post
            {
                CategoryId = post.CategoryId,
                Content = post.Content,
                Title = post.Title,
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();

                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));


                HttpResponseMessage response = await client.PostAsJsonAsync("api/Post", newPost);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        // GET: PostController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Post post)
        {
            var updatePost = new Post
            {
                Id = id,
                Content = post.Content,
                Title = post.Title,
                CategoryId = post.CategoryId
            };

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Post/{id}", updatePost);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        // GET: PostController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Post post)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                HttpResponseMessage response = await client.DeleteAsync($"api/Post/{id}");

                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }
    }
}
