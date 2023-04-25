using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class HomeController : Controller
    {
        private IStoreRepository _repository;
        public int pageSize = 4;

        public HomeController(IStoreRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index(string? category, int productPage = 1)
        {
            return View(new ProductsListViewModel
            {
                Products = _repository.Products
                                                .Where(p => category == null || p.Category == category)
                                                .OrderBy(p => p.ProductId)
                                                .Skip((productPage - 1) * pageSize)
                                                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? _repository.Products.Count() : _repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            });
        }
    }
}
