using E_Cart_WebApp.Data;
using E_Cart_WebApp.DTOs;
using E_Cart_WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace E_Cart_WebApp.Controllers
{
    //[Authorize(Roles = "admin,user")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindContext _context;
        public HomeController(ILogger<HomeController> logger, NorthwindContext context)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Cart()
        {
            int userId = 1;
            var tPriceRes = await _httpClient.GetAsync("https://localhost:7185/api/cart/totalamount");
            var response = await _httpClient.GetAsync($"https://localhost:7185/api/cart/byuserid?userId={userId}");
            var responseCartCount = await _httpClient.GetAsync("https://localhost:7185/api/cart/count");

            if (tPriceRes.IsSuccessStatusCode && responseCartCount.IsSuccessStatusCode)
            {
                var responseContent = await responseCartCount.Content.ReadAsStringAsync();
                var cartCount = JsonConvert.DeserializeObject<int>(responseContent);
                if(cartCount == 0)
                {
                    ViewBag.message = "Cart is Empty";
                    return RedirectToAction("Product");
                }
             
                
                var totalPrice = await tPriceRes.Content.ReadAsStringAsync();
                //var totalPrice = JsonConvert.DeserializeObject<double>(responseContent);
                ViewBag.totalPrice = totalPrice;
            }

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                var cart = JsonConvert.DeserializeObject<List<ProductCartDTO>>(responseContent);
                return View(cart);
            }
            else
            {
                return RedirectToAction("Product");
            }
        }

        // Controller for showing list of Products
        public async Task<IActionResult> Product(string productSearch)
        {
            var responseCartCount = await _httpClient.GetAsync("https://localhost:7185/api/cart/count");
            if (responseCartCount.IsSuccessStatusCode)
            {
                var responseContent = await responseCartCount.Content.ReadAsStringAsync();
                var cartCount = JsonConvert.DeserializeObject<int>(responseContent);
                ViewBag.ProductsInCartCount = cartCount;
            }
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NoContent);
            if (productSearch == null)
            {
                response = await _httpClient.GetAsync("https://localhost:7185/api/products");
            }
            else
            {
                response = await _httpClient.GetAsync($"https://localhost:7185/api/products/search/{productSearch}");

            }

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<GetProductsAPIResponse>(responseContent);


                return View(products.Data);


            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get products: {response.ReasonPhrase}");
                return RedirectToAction("Product");
                //return RedirectToAction("Error", new { message = response.ReasonPhrase });
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

                var product = JsonConvert.DeserializeObject<GetProductByIdAPIResponse>(responseContent);

                return View(product.Data);
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get product with ID {id}: {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }

            return View();
        }

        //public async Task<IActionResult> Privacy()
        //{
          
        //}

        // Controller to clear all items in the cart
        public async Task<IActionResult> ClearCart()
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7185/api/Cart/clear/all");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = responseContent;
                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Cart");
            }
        }


        // Controller to remove the product with an id
        public async Task<IActionResult> ClearCartById(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7185/api/Cart/{id}");
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                ViewBag.Message = "Item removed from the cart";
                return RedirectToAction("Cart");
            }
            else
            {
                return RedirectToAction("Cart");
            }
        }



        // Quantity Increment

        [HttpPost]
        public int Increment(int qty)
        {
            if (qty < 100)
                return qty + 1;
            return qty;
        }

        // Quantity Decrement

        [HttpPost]
        public int Decrement(int qty)
        {
            if (qty > 0)
            {
                return qty - 1;
            }
            return qty;
        }


        [HttpPost]
        public async Task<string> AddToCart(ProductsCart product)
        {

            var responseCartCount = await _httpClient.GetAsync("https://localhost:7185/api/cart/count");
            if (responseCartCount.IsSuccessStatusCode)
            {
                var responseContent = await responseCartCount.Content.ReadAsStringAsync();
                var cartCount = JsonConvert.DeserializeObject<int>(responseContent);
            ViewBag.ProductsInCartCount = cartCount;
            }
            var productInCart = new ProductsCart()
            {
                ProductId = product.ProductId,
                UserId = product.UserId,
                CartQuantity = product.CartQuantity,
                Product = null,
                User = null
            };

            var response = await _httpClient.PostAsJsonAsync<ProductsCart>($"https://localhost:7185/api/cart/", productInCart);

            if (response != null)
            {
                return "Item is added to the Cart";
            }
            else
            {
                // Log the error
                return "Failed to add the product into the Cart";
            }
        }




        [HttpGet]
        public async Task<int> CartCount()
        {
            var responseCartCount = await _httpClient.GetAsync("https://localhost:7185/api/cart/count");
            if (responseCartCount.IsSuccessStatusCode)
            {
                var responseContent = await responseCartCount.Content.ReadAsStringAsync();
                var cartCount = JsonConvert.DeserializeObject<int>(responseContent);
                return cartCount;
            } else
            {
                return 0;
            }
        }

        public async Task<IActionResult> PlaceOrder()
        {
            // Retrieve all products in the ProductCart table
            var response = await _httpClient.GetAsync("https://localhost:7185/api/cart/placeorder");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return View();
            }
            else 
            {
                return RedirectToAction("cart");
            }
       
        }







        // Global Error Page for all kinds of Exception

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}