
using E_Cart_WebApp.Models;
using ECartAPIClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace E_Cart_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminController> _logger;
        NorthwindContext northwindDb = new NorthwindContext();

        private readonly swaggerClient _swaggerClient;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _swaggerClient = new swaggerClient("https://localhost:7185/", _httpClient);
        }
        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Product()
        {
            var response = await _httpClient.GetAsync("https://localhost:7185/api/products");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<List<Models.Product>>(responseContent);

                return View(products);
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get products: {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }

            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7185/api/Products/{id}");
            //var response = _swaggerClient.DeleteProductsAsync(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Product");
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get delete the product with id {id}: {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }
            return RedirectToAction("Product");
        }

        public IActionResult ProductCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(Models.Product product)
        {
            var response = await _httpClient.PostAsJsonAsync<Models.Product>($"https://localhost:7185/api/Products/",product);
            //var response = _swaggerClient.DeleteProductsAsync(id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Product");
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get create the product : {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }
        }



        public IActionResult ProductDetail(int id)
        {
            var productInfo = northwindDb.Products.
                Where(p => p.ProductID == id).
                FirstOrDefault();

            return View(productInfo);
        }
    }
}
