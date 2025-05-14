using Domain.Shared;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customer.Interface
{
    public interface  ICustomerService :IService<Domain.Models.Customer, CustomerView>
    {

    }
}
