using BlogPlatformMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BlogPlatformMVC.Controllers
{
    public class CategoryController : Controller
    {
        // Declaring and initializing string of BaseUrl
        // This variable will be used to connect to API request and get the data from it
        private readonly string BaseURL = "http://ec2-54-234-4-13.compute-1.amazonaws.com/";
        // GET: CategoryController

        // Controller method that sends request to API with BaseUrl and fetches api/Category 
        // And lists all categories
        public async Task<ActionResult> Index()
        {
            List<Category> categories = new List<Category>();

            // It is a block where all the opened connections will be automatically disposed 
            using (var client = new HttpClient())
            {
                // Sets the address of the deployed API that has to be called to the connection
                client.BaseAddress = new Uri(BaseURL);
                // Removes all Headers from the request that will be sent
                client.DefaultRequestHeaders.Clear();

                // Sets the header to be application/json
                // The request will understand json formats
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                // Sending GET request to the API asynchronosly and storing the response in HttpResponseMessage type
                HttpResponseMessage ResultMessage = await client.GetAsync("api/Category");

                // Checks the Status code
                // If it is 200
                if (ResultMessage.IsSuccessStatusCode)
                {
                    // Converting the reponse content to string
                    var Response = ResultMessage.Content.ReadAsStringAsync().Result;

                    // And deserializing the json objects that was received from Request to List of Categories
                    categories = JsonConvert.DeserializeObject<List<Category>>(Response);
                }
                return View(categories);
            }

            
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        // Controller method that gets the category object from the form the user inserted
        // And Serializes it to Json and sends it to API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {

            var newCat = new Category
            {
                Id = category.Id,
                Name = category.Name,
            };

            // Created a block for auto disposing connections
            using (var client = new HttpClient())
            {
                // Establishing connection with API
                client.BaseAddress = new Uri(BaseURL);

                // Changing header of the connection to application/json
                // by first removing default header and then adding 
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                // Sending POST request asynchronously by serializing the object to JSON format
                // to the API
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Category", newCat);
                
                // If response is sucess than the Index page is loaded
                // If not Error page
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }


            //var response = await client.PostAsync("api/Category/Post", new StringContent(JsonConvert.SerializeObject(newCat), System.Text.Encoding.UTF8, applicationJson));


        }

        //GET: CategoryController/2/Posts

        //Controller method that gets the related posts to the specific category
        public async Task<ActionResult> GetPostsByCategoryId(int categoryId)
        {
            // Creating post list because we will show posts
            List<Post> posts = new List<Post>();

            // Creating block that auto disposes connections 
            using(var client = new HttpClient())
            {
                // Connecting to the API by BaseURL
                client.BaseAddress = new Uri(BaseURL);

                // Changing default header to Application/JSon
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Sending GET request to the API to posts related to category
                HttpResponseMessage response = await client.GetAsync($"api/Category/{categoryId}/posts");


                // If response is success the response is deserialized to c# object and shown to the user
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<Post>>(content);
                }
                return View(posts);
            }
                
            
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Controller method for editing the name of the specific Category
        public async Task<ActionResult> Edit(int id, string Name)
        {
            var updateCat = new Category
            {
                Id = id,
                Name = Name
            };
            // Establishing connection between API in block that automatically disposes connections at the end
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                // Changing request header to Application/json
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));


                // Sending async PUT request to the API to update the record 
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/Category/{id}", updateCat);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Controller method for deleting specific category
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            // Block that establishes connection with API by creating HttpClient connection
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);

                // Changing header of the request to application/json
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                // Sending DELETE request to the API and getting the Response of it
                HttpResponseMessage response = await client.DeleteAsync($"api/Category/{id}");

                // If success then load Index
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }
        }
    }
}
