using Domain.AppDbContext;
using Domain.Shared;
using Domain.ViewModels;
using Infrastructure.Customer.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Metadata;

namespace Infrastructure.Customer.Core
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly KayanDbContext _kayanDbContext;

        public CustomerRepo(KayanDbContext kayanDbContext)
        {
            _kayanDbContext = kayanDbContext;
        }

        public async Task<ResponseBehavior<int>> CreateAsync(Domain.Models.Customer model)
        {
            try
            {
                await _kayanDbContext.Customers.AddAsync(model);
                await _kayanDbContext.SaveChangesAsync();
                return new ResponseBehavior<int>() { Success = true, Data = model.Id };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<int>() { Success = false, Data = 0, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Inserting Model" } } };
            }

        }

        public async Task<ResponseBehavior<int>> ModifyAsync(int id, Domain.Models.Customer model)
        {
            try
            {
                _kayanDbContext.Customers.Update(model);
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

                var model = await _kayanDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
                _kayanDbContext.Customers.Remove(model);
                await _kayanDbContext.SaveChangesAsync();

                return new ResponseBehavior<int>() { Success = true, Data = model.Id };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<int>() { Success = false, Data = 0, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<IEnumerable<CustomerView>>> SelectAllAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var CustomersModel = await _kayanDbContext.Customers.ToListAsync();

                IEnumerable<CustomerView> result = CustomersModel.Select(x => new CustomerView()
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    LastName = x.LastName,
                    Orders = x.Orders,
                    PhoneNumber = x.PhoneNumber,
                }).OrderByDescending(x=>x.Id) .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
                return new ResponseBehavior<IEnumerable<CustomerView>>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<IEnumerable<CustomerView>>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }

        public async Task<ResponseBehavior<CustomerView>> SelectByIdAsync(int id)
        {
            try
            {
                var x = await _kayanDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
                CustomerView result = new CustomerView()
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    Id = x.Id,
                    LastName = x.LastName,
                    Orders = x.Orders,
                    PhoneNumber = x.PhoneNumber
                };

                return new ResponseBehavior<CustomerView>() { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseBehavior<CustomerView>() { Success = false, Data = null, Messages = new List<ResponseMessagesBehavior>() { new ResponseMessagesBehavior() { Key = "501", Value = "Faild Modify Model" } } };
            }
        }
    }
}
