using CleanArchExample.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchExample.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task  AddProductAsync(ProductDto product);
        Task EditProductAsync(ProductDto product);
        Task DeleteProductAsync(int id);
    }
}
