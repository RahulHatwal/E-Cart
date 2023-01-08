using E_Cart_WebApp.DTOs;
using E_Cart_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Diagnostics;
using System.Net;

namespace E_Cart_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
     
        // Controller for showing list of Products
        public async Task<IActionResult> Product(string productSearch="all")
        {
            var response = await _httpClient.GetAsync($"https://localhost:7185/api/products?search={productSearch}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var products =  JsonConvert.DeserializeObject<List<Product>>(responseContent);
          
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

        // Controller for showing a specific product by using id
        public async Task<IActionResult> ProductDetail(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7185/api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var product = JsonConvert.DeserializeObject<Product>(responseContent);

                return View(product);
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get product with ID {id}: {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        

        // Quantity Increment

        [HttpPost]
        public int Increment(int qty)
        {
            if(qty < 100)
                return qty + 1;
            return qty;
        }
        
        // Quantity Decrement

        [HttpPost]
        public int Decrement(int qty)
        {
            if(qty > 0) { 
                return qty - 1;
            }
            return qty;
        }


        // Global Error Page for all kinds of Exception

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}