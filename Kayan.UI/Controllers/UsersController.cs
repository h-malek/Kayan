using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.AppDbContext;
using Domain.Models;
using Application.Customer.Interface;
using Application.Customer.Service;
using Domain.ViewModels;
using Domain.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Kayan.UI.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pageSize;
            return View(_userService.SelectAllAsync(page, pageSize).Result.Data);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer.Data);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Salt,ImagePath,ImageFile")] UserView user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if (ModelState.IsValid)
            {
                //var decryptedId = int.Parse(AesHelper.Decrypt(user.Id));
                var userModel = new User()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Id = user.Id,
                    Password = user.Password,
                    Salt = user.Salt,
                    ImagePath = user.ImagePath,
                };

                // Image handling
                if (user.ImageFile != null)
                {
                    if (user.ImageFile.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageFile", "Invalid file type or size");
                        return View(user);
                    }

                    string fileName = Guid.NewGuid() + Path.GetExtension(user.ImageFile.FileName);
                    string filePath = Path.Combine("wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await user.ImageFile.CopyToAsync(stream);
                    }

                    user.ImagePath = fileName;
                    userModel.ImagePath = fileName;
                }
                await _userService.CreateAsync(userModel);

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer.Data);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Salt,ImagePath,ImageFile")] UserView user)
        {
            //var decryptedId = int.Parse(AesHelper.Decrypt(user.Id));
            if (id != user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var userModel = new User()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        Id = user.Id,
                        Password = user.Password,
                        Salt = user.Salt,
                        ImagePath = user.ImagePath,
                        ImageFile = user.ImageFile,
                    };

                    await _userService.ModifyAsync(id, userModel);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userService.SelectByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer.Data);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _userService.SelectByIdAsync(id);
            if (customer != null)
            {
                await _userService.RemoveAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
