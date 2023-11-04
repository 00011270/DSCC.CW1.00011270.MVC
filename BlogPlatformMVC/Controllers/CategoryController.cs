using BlogPlatformMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlogPlatformMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly string BaseURL = "https://localhost:7278/";
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            List<Category> categories = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage ResultMessage = await client.GetAsync("api/Category");
                if (ResultMessage.IsSuccessStatusCode)
                {
                    var Response = ResultMessage.Content.ReadAsStringAsync().Result;

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            var newCat = new Category
            {
                Id = category.Id,
                Name = category.Name,
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();

                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                HttpResponseMessage response = await client.PostAsJsonAsync("api/Category", newCat);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error");
            }


            //var response = await client.PostAsync("api/Category/Post", new StringContent(JsonConvert.SerializeObject(newCat), System.Text.Encoding.UTF8, applicationJson));


        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            var updateCat = new Category
            {
                Id = id,
                Name = category.Name
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                HttpResponseMessage response = await client.PutAsJsonAsync("api/Category", updateCat);

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
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var applicationJson = "application/json";

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(applicationJson));

                HttpResponseMessage response = await client.DeleteAsync($"api/Category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return Redirect("Index");
                }
                return Redirect("Error");
            }
        }
    }
}
