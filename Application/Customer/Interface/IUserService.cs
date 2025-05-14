using Domain.Shared;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Customer.Interface
{
    public interface IUserService : IService<Domain.Models.User, UserView>
    {
        Task<ResponseBehavior<UserView>> Login(string email, string password);
    }
}
