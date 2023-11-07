using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogPlatformMVC.Models;
using Newtonsoft.Json;

// Project made by 00011270
// For CC module level 6 WIUT
namespace BlogPlatformMVC.Controllers
{
    public class PostController : Controller
    {
        // Declaring and initializing string of BaseUrl
        // This variable will be used to connect to API request and get the data from it
        private readonly string BaseURL = "http://ec2-54-234-4-13.compute-1.amazonaws.com/";
        // GET: PostController
        // Controller method that sends asynchronously request to API with BaseUrl and fetches api/Post
        // And lists all categories
        public async Task<ActionResult> Index()
        {
            List<Post>? posts = new List<Post>();

            // It is a block where all the opened connections will be automatically disposed 
            
            using (var client = new HttpClient())
            {
                // Establishing connection with API by setting the BaseUrl of API
                client.BaseAddress = new Uri(BaseURL);

                // Removing default Request header that HttpClient provides
                // And adding Application/json header
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                // Sending GET request to API to get Posts from Db
                HttpResponseMessage response = await client.GetAsync("api/Post");
                
                // If the response is success
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

        // Post Controller that sends async request to API to get the details of the specific post
        public async Task<ActionResult> Details(int id)
        {
            var post = new Post();

            // Opening Http connection and assigning the API url to it
            using(var client = new HttpClient())
            {
                client.BaseAddress=new Uri(BaseURL);

                //Changing default requst header to application/json
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Sending async GET request to API 
                var response = await client.GetAsync($"api/Post/{id}");

                // Deserializing the response to Post object
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
        // Post Contorller that gets the information that user inserted to create post
        // And sends the info to API
        public async Task<ActionResult> Create(Post post)
        {
            var newPost = new Post
            {
                CategoryId = post.CategoryId,
                Content = post.Content,
                Title = post.Title,
            };

            // Openning connection and connecting to API
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                // Changing header to application/json
                client.DefaultRequestHeaders.Clear();

                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                // Sending Async POST request by serializing the object to JSON and storing Response
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
        // Post Controller method for updating the fields of specific Post
        // It gets the Id of post, and the new information in Post object
        public async Task<ActionResult> Edit(int id, Post post)
        {

            
            var updatePost = new Post
            {
                Id = id,
                Content = post.Content,
                Title = post.Title,
                CategoryId = post.CategoryId
            };


            // Establishing connection between API in using block
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));


                // Sending Async PUT request to API and getting response of it
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
        // Post Controller that gets the ID of the post and deletes
        public async Task<ActionResult> Delete(int id, Post post)
        {
            // Establishing connection between API in using block
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));


                // Sending Async DELETE request to API and getting response of it
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
