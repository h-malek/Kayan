using Application.Customer.Interface;
using Application.Customer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kayan.UI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index(string storeName = "", int? minQty = null, int page = 1, int pageSize = 10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pageSize;
            ViewBag.StoreName = storeName;
            ViewBag.MinQty = minQty;
            return View(_dashboardService.SalesOrders(storeName, minQty, page, pageSize).Result.Data);
        }

        public async Task<IActionResult> Customers(string first = "", string last = "", string phone = "", string email = "", int page = 1, int pageSize = 10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pageSize;
            ViewBag.First = first;
            ViewBag.Last = last;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            return View(_dashboardService.Customers(first, last, phone, email, page, pageSize).Result.Data);
        }

        public async Task<IActionResult> TopProducts()
        {
            return View(_dashboardService.TopProducts().Result.Data);
        }
    }
}
