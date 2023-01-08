
using Azure;
using E_Cart_WebApp.DTOs;
using E_Cart_WebApp.Models;
using ECartProductAPIClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;

namespace E_Cart_WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }




        public async Task<IActionResult> Product(string productSearch = "all")
        {
            var response = await _httpClient.GetAsync($"https://localhost:7185/api/products?search={productSearch}");

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
            var response = await _httpClient.DeleteAsync($"https://localhost:7185/api/products/{id}");
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
            // return RedirectToAction("Product");
        }
        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {

            var categories = await _httpClient.GetAsync("https://localhost:7185/api/Categories");
            var suppliers = await _httpClient.GetAsync("https://localhost:7185/api/Suppliers");
            if (categories.IsSuccessStatusCode && suppliers.IsSuccessStatusCode)
            {
                var categoriesContent = await categories.Content.ReadAsStringAsync();
                var categoriesList = JsonConvert.DeserializeObject<List<Models.Category>>(categoriesContent);
                var suppliersContent = await suppliers.Content.ReadAsStringAsync();
                var suppliersList = JsonConvert.DeserializeObject<List<Models.Supplier>>(suppliersContent);

                var dropDownDTO = new ProductDTO();

                dropDownDTO.CategoriesSelectList = new List<SelectListItem>();
                dropDownDTO.SupplierSelectList = new List<SelectListItem>();

                foreach (var category in categoriesList)
                {
                    dropDownDTO.CategoriesSelectList.Add(new SelectListItem { Text = category.CategoryName, Value = category.CategoryID.ToString() });
                }

                foreach (var supplier in suppliersList)
                {
                    dropDownDTO.SupplierSelectList.Add(new SelectListItem { Text = supplier.ContactName, Value = supplier.SupplierID.ToString() });
                }
                //ViewBag.categoryDropDown = new SelectList(categoriesList, "CategoryID", "CategoryName");
                //ViewBag.supplierDropDown = new SelectList(suppliersList, "SupplierID", "CompanyName");
                return View(dropDownDTO);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO product)
        {
            var newProduct = new ProductDTO()
            {
                ProductName = product.ProductName,
                SupplierId = Int32.Parse(product.SelectedSupplier),
                CategoryId = Int32.Parse(product.SelectedCategory),
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
            };


            var response = await _httpClient.PostAsJsonAsync<ProductDTO>($"https://localhost:7185/api/Products/", newProduct);
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

        public async Task<IActionResult> ProductDetail(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7185/api/products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                var product = JsonConvert.DeserializeObject<Models.Product>(responseContent);

                return View(product);
            }
            else
            {
                // Log the error
                _logger.LogError($"Failed to get product with ID {id}: {response.ReasonPhrase}");
                return RedirectToAction("Error", new { message = response.ReasonPhrase });
            }

        }

        //public async Task<IActionResult> ProductDetail(int id)
        //{
        //    var client = new swaggerClient("https://localhost:7185/", _httpClient);

        //    var productInfo = await client.GetProductAsync(id);

        //    return View(productInfo);
        //}

        //public IActionResult ProductDetail(int id)
        //{
        //    var productInfo = northwindDb.Products.
        //        Where(p => p.ProductID == id).
        //        FirstOrDefault();

        //    return View(productInfo);
        //}
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
