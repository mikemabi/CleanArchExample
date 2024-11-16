using CleanArchExample.Application.Dto;
using CleanArchExample.Application.Interfaces;
using CleanArchExample.Core.Entities;
using CleanArchExample.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchExample.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                Category = productDto.Category,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
            };
            await _productRepository.AddProduct(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProduct(id);
        }

        public async Task EditProductAsync(ProductDto productDto)
        {
            var product = await _productRepository.GetProductById(productDto.Id);
            if (product == null)
            {
                return;
            }

            product.Category = productDto.Category;
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            await _productRepository.EditProduct(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {

            var products = await _productRepository.GetAll();
            var productList = products.Select(p => new ProductDto
            {
                Category = p.Category,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Id = p.Id
            });
            return productList;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return null;
            }
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
            };
        }
    }
}
