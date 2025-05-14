using Domain.AppDbContext;
using Domain.Shared;
using Domain.ViewModels;
using Infrastructure.Customer.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;

namespace Infrastructure.Customer.Core
{
    public class UserRepo : IUserRepo
    {
        private readonly KayanDbContext _kayanDbContext;

        public UserRepo(KayanDbContext kayanDbContext)
        {
            _kayanDbContext = kayanDbContext;
        }

        public async Task<ResponseBehavior<int>> CreateAsync(Domain.Models.User model)
        {
            try
            {
                await _kayanDbContext.Users.AddAsync(model);
                await _kayanDbContext.SaveChangesAsync();
                return new ResponseBehavior<int>() { Success = true, Data = model.Id };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<int>() { Success = false, Data = 0, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Inserting Model" } } };
            }

        }

        public async Task<ResponseBehavior<int>> ModifyAsync(int id, Domain.Models.User model)
        {
            try
            {
                _kayanDbContext.Users.Update(model);
                await _kayanDbContext.SaveChangesAsync();
                return new ResponseBehavior<int>() { Success = true, Data = model.Id };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<int>() { Success = false, Data = 0, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<int>> RemoveAsync(int id)
        {
            try
            {

                var model = await _kayanDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                _kayanDbContext.Users.Remove(model);
                await _kayanDbContext.SaveChangesAsync();

                return new ResponseBehavior<int>() { Success = true, Data = model.Id };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<int>() { Success = false, Data = 0, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<IEnumerable<UserView>>> SelectAllAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var UserModel = await _kayanDbContext.Users.ToListAsync();

                IEnumerable<UserView> result = UserModel.Select(x => new UserView()
                {
                    Email = x.Email,
                    Name = x.Name,
                    Id = x.Id,
                    ImagePath = x.ImagePath,
                }).ToList();
                return new ResponseBehavior<IEnumerable<UserView>>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<IEnumerable<UserView>>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<UserView>> SelectByIdAsync(int id)
        {
            try
            {
                var x = await _kayanDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
                UserView result = new UserView()
                {
                    Email = x.Email,
                    Name = x.Name,
                    Id = x.Id,
                    ImagePath = x.ImagePath
                };

                return new ResponseBehavior<UserView>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<UserView>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<UserView>> SelectById(int id)
        {
            try
            {
                var x = await _kayanDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                UserView result = new UserView()
                {
                    Email = x.Email,
                    Name = x.Name,
                    Id = x.Id,
                    Salt = x.Salt,
                    Password = x.Password,
                    ImagePath = x.ImagePath
                };

                return new ResponseBehavior<UserView>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<UserView>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<UserView>> SelectByEmailAsync(string email)
        {
            try
            {
                var x = await _kayanDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                UserView result = new UserView()
                {
                    Email = x.Email,
                    Name = x.Name,
                    Id = x.Id,
                    Password = x.Password,
                    Salt = x.Salt,
                    ImagePath = x.ImagePath
                };

                return new ResponseBehavior<UserView>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<UserView>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }
    }
}
