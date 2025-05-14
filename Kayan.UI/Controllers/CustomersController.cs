using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Application.Customer.Interface;
using Domain.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Kayan.UI.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        //public CustomersController(KayanDbContext context)
        //{
        //    _context = context;
        //}
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages =  pageSize;
            return View( _customerService.SelectAllAsync(page,pageSize).Result.Data);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer.Data);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber")] CustomerView customer)
        {
            if (ModelState.IsValid)
            {
                var customerModel = new Customer()
                {
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    Id = customer.Id,
                    LastName = customer.LastName,
                    Orders = customer.Orders,
                    PhoneNumber = customer.PhoneNumber,
                };
                await _customerService.CreateAsync(customerModel);

                return RedirectToAction(nameof(Index));
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer.Data);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber")] CustomerView customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var customerModel = new Customer()
                    {
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        Id = customer.Id,
                        LastName = customer.LastName,
                        Orders = customer.Orders,
                        PhoneNumber = customer.PhoneNumber,
                    };
                   await _customerService.ModifyAsync(id, customerModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!CustomerExists(customer.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer.Data);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _customerService.SelectByIdAsync(id);
            if (customer != null)
            {
                await _customerService.RemoveAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
