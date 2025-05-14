
using Domain.Models;
using Domain.Shared;
using Domain.ViewModels;


namespace Infrastructure.Customer.Interface
{
    public interface IUserRepo : IRepository<Domain.Models.User, UserView>
    {
        Task<ResponseBehavior<UserView>> SelectByEmailAsync(string email);
        Task<ResponseBehavior<UserView>> SelectById(int id);
    }
}
