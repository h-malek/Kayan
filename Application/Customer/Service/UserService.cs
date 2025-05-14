using Application.Customer.Interface;
using Azure.Core;
using Domain.Shared;
using Domain.ViewModels;
using Infrastructure.Customer.Core;
using Infrastructure.Customer.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Domain.Models;
using Domain.Shared.Helpers;

namespace Application.Customer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResponseBehavior<UserView>> Login(string email, string password)
        {
            var user = _userRepo.SelectByEmailAsync(email).Result.Data;

            if (user != null)
            {
                var hashed = HashPassword(password, user.Salt);
                if (hashed == user.Password)
                {
                    //var claims = new List<Claim>
                    //{
                    //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    //    new Claim(ClaimTypes.Name, user.Name),
                    //    new Claim(ClaimTypes.Email, user.Email)
                    // };

                    //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //var principal = new ClaimsPrincipal(identity);

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    //var claims = new[]
                    //{
                    //    new Claim(ClaimTypes.Name, user.Name),
                    //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    //};

                    //var identity = new ClaimsIdentity(claims, "ApplicationCookie");
                    //var ctx = Request.GetOwinContext();
                    //var authManager = ctx.Authentication;

                    //authManager.SignIn(identity);

                    return new ResponseBehavior<UserView>() { Success = true, Data = user };

                }
            }
            return new ResponseBehavior<UserView>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };

        }

        public async Task<ResponseBehavior<int>> CreateAsync(Domain.Models.User model)
        {
            var salt = Guid.NewGuid().ToString();
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Salt = salt,
                ImagePath = model.ImagePath,
                Password = PasswordHelper.HashPassword(model.Password, salt)
            };

            return await _userRepo.CreateAsync(user);
        }

        public async Task<ResponseBehavior<int>> ModifyAsync(int id, Domain.Models.User model)
        {
            var userView = _userRepo.SelectById(model.Id).Result.Data;

            if (userView == null)
                return null;

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Salt = userView.Salt,
                Password = userView.Password,
                ImagePath = userView.ImagePath,
                Id = id
            };

            // ✅ Handle image update
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedTypes.Contains(model.ImageFile.ContentType) || model.ImageFile.Length > 2 * 1024 * 1024)
                {
                    return null;
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(userView.ImagePath))
                {
                    string oldPath = Path.Combine("wwwroot/uploads", userView.ImagePath);
                    //var oldPath = Path.Combine(_env.WebRootPath, "uploads", user.ImagePath);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // Save new image
                var newFileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var newPath = Path.Combine( "wwwroot/uploads", newFileName);
                using var stream = new FileStream(newPath, FileMode.Create);
                await model.ImageFile.CopyToAsync(stream);

                user.ImagePath = newFileName;
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var salt = Guid.NewGuid().ToString();
                user.Salt = salt;
                user.Password = PasswordHelper.HashPassword(model.Password, salt);
            }
            return await _userRepo.ModifyAsync(id, user);
        }

        public async Task<ResponseBehavior<int>> RemoveAsync(int id)
        {
            return await _userRepo.RemoveAsync(id);
        }

        public async Task<ResponseBehavior<IEnumerable<UserView>>> SelectAllAsync(int page = 1, int pageSize = 10)
        {
            return await _userRepo.SelectAllAsync(page, pageSize);
        }

        public async Task<ResponseBehavior<UserView>> SelectByIdAsync(int id)
        {
            return await _userRepo.SelectByIdAsync(id);
        }


        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var combined = password + salt;
                var bytes = System.Text.Encoding.UTF8.GetBytes(combined);
                var hash = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hash);
            }
        }
    }
}
