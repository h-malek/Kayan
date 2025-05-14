
using Domain.Models;
using Domain.Shared;
using Domain.ViewModels;


namespace Infrastructure.Customer.Interface
{
    public interface ICustomerRepo : IRepository<Domain.Models.Customer, CustomerView>
    {
    }
}
