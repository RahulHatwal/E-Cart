using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Cart_WebAPI.Models;
using E_Cart_WebAPI.Repository;
using Swashbuckle.AspNetCore;
using E_Cart_WebAPI.Helpers;
using System.Net;

namespace E_Cart_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }




        // GET: api/Products
        /// <summary>
        /// Gets a list of all products by search query.
        /// </summary>
        /// 
        [Route("search/{search}")]
        [HttpGet]
        public async Task<IActionResult> GetProductsBySearch(string search)
        {
            var products = await _repository.GetAllBySearchQueryAsync(search);
            if (products == null)
            {
                //throw new ApplicationException("Something went wrong...");
                return new CustomResult(404, null, false, "No products found for the given search query");
            }
            return new CustomResult(200, products, true, "Successfully retrieved products");
        }

        // GET: api/Products
        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            if (products == null)
            {
                //throw new ApplicationException("Something went wrong...");
                return new CustomResult(404, null, false, "Product is not found with given Id");
            }
            _logger.LogInformation("Product with given id found");
            return new CustomResult(200, products, true, "Successfully retrieved products");
        }



        // GET: api/Products
        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        /// 
        //[Route("desc")]
        //[HttpGet]
        //public async Task<IActionResult> GetProductsDesc()
        //{
        //    try
        //    {
        //        var products = await _repository.GetAllAsync();
        //        if (products == null)
        //        {
        //            throw new ApplicationException("Something went wrong...");
        //        }
        //        var productsDesc = products.OrderByDescending(p => p.ProductName).ToList();
        //        return 
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}



        // GET: api/Products/5

        /// <summary>
        /// Gets a specific product by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
               // _logger.LogWarning("Product is not found with given Id");
                return new CustomResult(404, null, false, "Product is not found with given Id");
            }
            _logger.LogInformation("Product with given id found");
            return new CustomResult(200, product, true, "Successfully retrieved product");
        }


        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creating a product.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            product = await _repository.AddAsync(product);

            if (product != null)
            {
                return new CustomResult(201, product, true, "Product successfully added");
            }
            else
            {
                return new CustomResult(500, null, false, "An error occurred while saving the product. Please try again.");
            }
        }


        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updating a product.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return new CustomResult((int)HttpStatusCode.NotFound, null, false, "Product not found");
            }

            try
            {
                await _repository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id))
                {
                    return new CustomResult((int)HttpStatusCode.NotFound,null,false,"Product not found");
                }
                else
                {
                    return new CustomResult((int)HttpStatusCode.Conflict,null,false,"Conflict Occured");
                }
            }
            var productInfo = await _repository.GetByIdAsync(id);
            return new CustomResult((int)HttpStatusCode.OK, productInfo, true,"Update Successful" );
        }



        // DELETE: api/Products/5
        /// <summary>
        /// Deleting a product.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return new CustomResult(404, null, false, "Product not found with given Id");
            }
            await _repository.DeleteAsync(id);
            //await _repository.ReseedProductIds();
            return new CustomResult(200, null, true, "Product successfully deleted");
        }


        private async Task<bool> ProductExists(int id)
        {
            return await _repository.GetByIdAsync(id) is Product;
        }
    }
}
