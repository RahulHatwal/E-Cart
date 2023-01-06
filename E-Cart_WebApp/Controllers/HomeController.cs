using E_Cart_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
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
        NorthwindContext northwindDb = new NorthwindContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
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

                var products = JsonConvert.DeserializeObject<List<Product>>(responseContent);

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


        //public IActionResult Product()
        //{   
        //    return View(northwindDb.Products);
        //}

        //public IActionResult ProductDetail(int id)
        //{
        //    var productInfo = northwindDb.Products.
        //        Where(p => p.ProductID == id).
        //        FirstOrDefault();

        //    return View(productInfo);
        //}


        [HttpPost]
        public int Increment(int qty)
        {
            if(qty < 100)
                return qty + 1;
            return qty;
        }
        
        
        [HttpPost]
        public int Decrement(int qty)
        {
            if(qty > 0) { 
                return qty - 1;
            }
            return qty;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}