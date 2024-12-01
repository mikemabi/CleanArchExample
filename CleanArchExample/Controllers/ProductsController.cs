using CleanArchExample.Application.Dto;
using CleanArchExample.Application.Interfaces;
using CleanArchExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchExample.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ProductsFromDb(int? page)
        {

            //var products = await _productService.GetAllProductsAsync();
            //ProductListModel listModel = new ProductListModel();
            //listModel.Products = products.ToList();
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var listModel = _productService.GetPaginatedProducts(pageNumber: pageNumber, pageSize: pageSize);
            var productModel = new ProductListModel { Products = listModel };
            return View(productModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductDto model)

        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _productService.AddProductAsync(model);
            return RedirectToAction("ProductsFromDb");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return RedirectToAction("index");
            }
            return View(product);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var product = await _productService.GetProductByIdAsync(model.Id);
            if (product == null)
            {
                return RedirectToAction("index");
            }
            await _productService.EditProductAsync(model);
            return RedirectToAction("ProductsFromDb");

        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Json(Url.Action("ProductsFromDb", "Products"));

        }
    }
}
