using Application.Customer.Interface;
using Domain.AppDbContext;
using Domain.Models;
using Domain.Shared;
using Domain.ViewModels;
using Infrastructure.Customer.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customer.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly KayanDbContext _dbContext;

        public DashboardService(ICustomerRepo customerRepo, KayanDbContext dbContext)
        {
            _customerRepo = customerRepo;
            _dbContext = dbContext;
        }

        public async Task<ResponseBehavior<IEnumerable<SalesOrderViewModel>>> SalesOrders(string storeName = "", int? minQty = null, int page = 1, int pageSize = 10)
        {
            var orders = _dbContext.OrderItems
           .Include(oi => oi.Order)
           .Include(oi => oi.Order.Store)
           .Include(oi => oi.Product)
           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(storeName))
                orders = orders.Where(o => o.Order.Store.StoreName.Contains(storeName));

            if (minQty.HasValue)
                orders = orders.Where(o => o.Quantity >= minQty.Value);

            var totalCount = orders.Count();

            var pagedOrders = orders
                .OrderBy(o => o.Order.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(oi => new SalesOrderViewModel
                {
                    OrderId = oi.Order.Id,
                    Store = oi.Order.Store.StoreName,
                    Product = oi.Product.ProductName,
                    Quantity = oi.Quantity,
                    Total = oi.Quantity * oi.UnitPrice
                })
                .ToList();

            return new ResponseBehavior<IEnumerable<SalesOrderViewModel>>() { Success = true, Data = pagedOrders };

        }
        
        public async Task<ResponseBehavior<IEnumerable<CustomerViewModel>>> Customers(string first = "", string last = "", string phone = "", string email = "", int page = 1, int pageSize = 10)
        {
            var query = _dbContext.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(first))
                query = query.Where(c => c.FirstName.Contains(first));

            if (!string.IsNullOrWhiteSpace(last))
                query = query.Where(c => c.LastName.Contains(last));

            if (!string.IsNullOrWhiteSpace(phone))
                query = query.Where(c => c.PhoneNumber.Contains(phone));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));

            var totalCount = query.Count();

            var result = query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.Id,
                    FullName = c.FirstName + " " + c.LastName,
                    Email = c.Email,
                    Phone = c.PhoneNumber
                }).ToList();

            return new ResponseBehavior<IEnumerable<CustomerViewModel>>() { Success = true, Data = result };

        }

        public async Task<ResponseBehavior<IEnumerable<TopProductSalesViewModel>>> TopProducts()
        {
            var topProducts = _dbContext.OrderItems
                 .Include(oi => oi.Product)
                 .Include(oi => oi.Order.Store)
                 .GroupBy(oi => new { oi.Order.Store.StoreName, oi.Product.ProductName })
                 .Select(g => new TopProductSalesViewModel
                 {
                     StoreName = g.Key.StoreName,
                     ProductName = g.Key.ProductName,
                     TotalQuantity = g.Sum(x => x.Quantity),
                     TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                 })
                 .OrderByDescending(x => x.TotalQuantity) // Sort by quantity sold
                 .ToList();

            // Group result into top 10 per store
            var result = topProducts
                .GroupBy(x => x.StoreName)
                .SelectMany(group => group.Take(10))
                .ToList();

            return new ResponseBehavior<IEnumerable<TopProductSalesViewModel>>() { Success = true, Data = result };

        }

    }
}
