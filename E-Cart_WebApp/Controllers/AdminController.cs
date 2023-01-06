using E_Cart_WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Cart_WebApp.Controllers
{
    public class AdminController : Controller
    {
        NorthwindContext northwindDb = new NorthwindContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View(northwindDb.Products);
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
