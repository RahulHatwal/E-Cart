using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Cart_WebAPI.Data;
using E_Cart_WebAPI.Models;
using E_Cart_WebAPI.DTOs;

namespace E_Cart_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CartController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/cart/1
        [Route("byuserid")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems(int userId)
        {
            var cartItems = await _context.ProductsCarts.Where(c => c.UserId == userId).ToListAsync();
            var cartItemDtos = new List<CartItemDto>();
            foreach (var item in cartItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == product.CategoryId);
                var cartItemDto = new CartItemDto
                {
                    ProductId = (int)item.ProductId,
                    ProductCardId = item.ProductCardId,
                    CategoryName = category.CategoryName,
                    ProductName = product.ProductName,
                    UnitPrice = (decimal)product.UnitPrice,
                    Picture = category.Picture,
                    Quantity = (int)item.CartQuantity

                };
                cartItemDtos.Add(cartItemDto);
                
            }

            return cartItemDtos;
        }
        [Route("all")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsCart>>> GetCartItems()
        {
            var cartItems = await _context.ProductsCarts.ToListAsync();
    
       

            return cartItems;
        }



        // GET: api/cart/totalamount
        [Route("totalamount")]
        [HttpGet]
        public async Task<IActionResult> TotalAmount()
        {

            var totalPrice = await _context.ProductsCarts
                .Join(_context.Products
                , cart => cart.ProductId
                , product => product.ProductId
                , (cart, product) => new
                {
                    UnitPrice = product.UnitPrice
                ,
                    Quantity = cart.CartQuantity
                }
                ).SumAsync(x => x.UnitPrice * x.Quantity);
            return Ok(totalPrice);
        }


        // Delete: api/cart/clearAll
        [Route("clear/all")]
        [HttpDelete]
        public async Task<ActionResult<IEnumerable<ProductsCart>>> ClearCart()
        {
            var cartItems =await _context.ProductsCarts.ToListAsync();
            _context.ProductsCarts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return Ok("Removed all items from the cart");
        }

        // GET: api/carts/count
        [Route("count")]
        [HttpGet]
        public async Task<IActionResult> GetProductCountInCart()
        {
            var productCount = await _context.ProductsCarts.CountAsync();
            return Ok(productCount);
        }


        // GET: api/cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsCart>> GetProductInCart(int id)
        {
            var productsCart = await _context.ProductsCarts.FindAsync(id);

            if (productsCart == null)
            {
                return NotFound();
            }

            return productsCart;
        }

        // PUT: api/cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductInCart(int id, ProductsCart productsCart)
        {
            if (id != productsCart.ProductCardId)
            {
                return BadRequest();
            }

            _context.Entry(productsCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductsCart>> PostProductsCart(ProductsCart productsCart)
        {
            if(productsCart != null) { 
            _context.ProductsCarts.Add(productsCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductsCart", new { id = productsCart.ProductCardId }, productsCart);
            }
            else
            {
                throw new ApplicationException("An error occurred while adding the product in the cart. Please try again.");
            }
        }


        // POST: api/cart
        // API for adding list of products to ProductCart table

        [Route("list")]
        [HttpPost]
        public async Task<ActionResult<ProductsCart>> AddListOfProductsInCart(List<ProductsCart> productsCart)
        {
            foreach (var item in productsCart)
            {
                _context.ProductsCarts.Add(item);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/cart/5
        [HttpDelete("{ProductId}")]
        public async Task<IActionResult> DeleteProductInCart(int ProductId)
        {
            var productsCart = await _context.ProductsCarts.Where(p=> p.ProductId == ProductId).FirstOrDefaultAsync();
            if (productsCart == null)
            {
                return NotFound();
            }

            _context.ProductsCarts.Remove(productsCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }





        [Route("placeorder")]
        [HttpGet]
        public async Task<IActionResult> PlaceOrder()
        {
            // Retrieve all products in the ProductCart table
            var productsInCart = _context.ProductsCarts.ToList();

            // Check if there are any products in the cart
            if (productsInCart.Count == 0)
            {
                return BadRequest("Cart is empty");
            }

            // Iterate through each product in the ProductCart table
            foreach (var product in productsInCart)
            {
                var unitPrice = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
                // Create a new OrderDetail object with the product's information
                var orderDetail = new ProductOrderDetail
                {
                    ProductId = (int)product.ProductId,
                    Quantity = (short)product.CartQuantity,
                    UnitPrice = (decimal)unitPrice.UnitPrice,
                    Discount = 0,
                };

                // Add the new OrderDetail object to the OrderDetails table
                _context.ProductOrderDetails.Add(orderDetail);
            }

            // Remove all products in the ProductCart table
            _context.ProductsCarts.RemoveRange(productsInCart);

            // Save changes to the database
            _context.SaveChanges();

            // Return a message indicating that the order was placed successfully
            return Ok("Order placed successfully");
        }


        private bool ProductsCartExists(int id)
        {
            return _context.ProductsCarts.Any(e => e.ProductCardId == id);
        }
    }
}
