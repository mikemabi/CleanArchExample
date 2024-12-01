using CleanArchExample.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchExample.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductById(int id);
        Task<IEnumerable<Product>> GetAll();
        Task AddProduct(Product product);
        Task EditProduct(Product product);
        Task DeleteProduct(int id);
        IQueryable<Product> GetProducts();

    }
}
