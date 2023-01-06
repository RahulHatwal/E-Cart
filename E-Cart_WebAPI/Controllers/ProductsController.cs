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

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var products = _repository.GetAll();
        //    if (products == null)
        //    {
        //        return NotFound("No products were found.");
        //    }
        //    return Ok(products);
        //}

        //[HttpGet("{id}")]
        //public ActionResult<Product> GetById(int id)
        //{
        //    var product = _repository.GetById(id);
        //    if (product == null)
        //    {
        //        return NotFound($"Product with ID {id} was not found.");
        //    }
        //    return Ok(product);
        //}

        //[HttpPost]
        //public ActionResult<Product> Create(Product product)
        //{
        //    _repository.Add(product);
        //    return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        //}

        //[HttpDelete("{id}")]
        //public ActionResult Delete(int id)
        //{
        //    var product = _repository.GetById(id);
        //    if (product == null)
        //    {
        //        return NotFound($"Product with ID {id} was not found.");
        //    }
        //    _repository.Delete(id);
        //    return Ok($"Product with ID {id} was deleted successfully.");
        //}


        // GET: api/Products
        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        [HttpGet]
    
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            if (products == null)
            {
                throw new ApplicationException("Something went wrong...");
            }
            return Ok(products);
        }


        // GET: api/Products/5

        /// <summary>
        /// Gets a specific product by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product is not found with given Id");
                throw new KeyNotFoundException("Product is not found with given Id");
            }
            _logger.LogInformation("Product with given id found");
            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          
            product = await _repository.AddAsync(product);

            if(product != null)
            {
                return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
            }
       
            else
            {
                // Log the error (uncomment ex variable name and write a log.
                throw new ApplicationException("An error occurred while saving the product. Please try again.");
            }
        }


        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id))
                {
                    throw new ApplicationException("Product Cannot be Updated because Product is not found with given Id");
                }
                else
                {
                    throw new ApplicationException("Something went wrong...");
                }
            }

            return NoContent();
        }



        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product is not found with given Id");
            }
            await _repository.DeleteAsync(id);
            return Ok($"Product with ID {id} was deleted successfully.");
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _repository.GetByIdAsync(id) is Product;
        }
    }
}
