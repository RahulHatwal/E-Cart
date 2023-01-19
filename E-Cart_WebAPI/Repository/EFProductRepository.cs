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


        //public EFProductRepository(NorthwindContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<Product> GetAll()
        //{
        //    return _context.Products.ToList();
        //}

        //public Product GetById(int id)
        //{
        //    return _context.Products.FirstOrDefault(p => p.ProductId == id);
        //}

        //public void Add(Product product)
        //{
        //    _context.Products.Add(product);
        //    _context.SaveChanges();
        //}

        //public void Delete(int id)
        //{
        //    var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        //    _context.Products.Remove(product);
        //    _context.SaveChanges();
        //}





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

        //public async Task AddAsync(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();
        //}

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

        //public async Task UpdateAsync(Product product)
        //{
        //    _context.Products.Update(product);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<Product> UpdateAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.ProductId == id);
        }

    }
}
