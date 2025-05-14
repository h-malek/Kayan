using Application.Customer.Interface;
using Domain.Shared;
using Domain.ViewModels;
using Infrastructure.Customer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customer.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerService(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }
        public async Task<ResponseBehavior<int>> CreateAsync(Domain.Models.Customer model)
        {
           return await _customerRepo.CreateAsync(model);
        }

        public async Task<ResponseBehavior<int>> ModifyAsync(int id, Domain.Models.Customer model)
        {
            return await _customerRepo.ModifyAsync(id, model);
        }

        public async Task<ResponseBehavior<int>> RemoveAsync(int id)
        {
            return await _customerRepo.RemoveAsync(id);
        }

        public async Task<ResponseBehavior<IEnumerable<CustomerView>>> SelectAllAsync(int page = 1, int pageSize = 10)
        {
            return await _customerRepo.SelectAllAsync(page,pageSize);
        }

        public async Task<ResponseBehavior<CustomerView>> SelectByIdAsync(int id)
        {
            return await _customerRepo.SelectByIdAsync(id);
        }
    }
}
