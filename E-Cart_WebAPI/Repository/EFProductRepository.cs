using E_Cart_WebAPI.Data;
using E_Cart_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Cart_WebAPI.Repository
{
    public class EFProductRepository : IProductRepository
    {
        private readonly NorthwindContext _context;

        public EFProductRepository(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllBySearchQueryAsync(string searchQuery)
        {   

            var result = await _context.Products.Where(x => x.ProductName.ToLower().StartsWith(searchQuery.ToLower())).ToListAsync();
            if(result.Count > 0)
            {
             return result;   
            }
            else
            {
                result = await _context.Products.ToListAsync();
                return result;
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var result = await _context.Products.ToListAsync();
            return result;
        }



        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task ReseedProductIds()
        {
            // Get the current max ProductId
            var maxProductId = await _context.Products.MaxAsync(p => p.ProductId);

            // Set the identity seed to the current max ProductId + 1
            await _context.Database.ExecuteSqlInterpolatedAsync($"DBCC CHECKIDENT('Products', RESEED, {maxProductId + 1})");
        }



        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.ProductId == id);
        }

    }
}
