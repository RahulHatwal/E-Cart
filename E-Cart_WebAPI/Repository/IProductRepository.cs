using E_Cart_WebAPI.Models;

namespace E_Cart_WebAPI.Repository
{
    public interface IProductRepository
    {
        //IEnumerable<Product> GetAll();
        //Product GetById(int id);
        //void Add(Product product);
        ////void Update(Product product);
        //void Delete(int id);


        Task<List<Product>> GetAllBySearchQueryAsync(string searchQuery);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
