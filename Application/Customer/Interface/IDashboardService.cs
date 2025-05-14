using Domain.Shared;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customer.Interface
{
    public interface IDashboardService
    {
        Task<ResponseBehavior<IEnumerable<SalesOrderViewModel>>> SalesOrders(string storeName = "", int? minQty = null, int page = 1, int pageSize = 10);
        Task<ResponseBehavior<IEnumerable<CustomerViewModel>>> Customers(string first = "", string last = "", string phone = "", string email = "", int page = 1, int pageSize = 10);
        Task<ResponseBehavior<IEnumerable<TopProductSalesViewModel>>> TopProducts();
    }
}
