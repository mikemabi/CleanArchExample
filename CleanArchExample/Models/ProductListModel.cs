using CleanArchExample.Application.Dto;
using X.PagedList;

namespace CleanArchExample.Models
{
    public class ProductListModel
    {
        public IPagedList<ProductDto?> Products { get; set; }
       
    }
}
