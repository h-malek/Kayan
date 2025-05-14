using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public interface IService<M,V>
    {
        Task<ResponseBehavior<int>> CreateAsync(M model);
        Task<ResponseBehavior<int>> ModifyAsync(int id, M model);
        Task<ResponseBehavior<int>> RemoveAsync(int id);
        Task<ResponseBehavior<IEnumerable<V>>> SelectAllAsync(int page = 1, int pageSize = 10);
        Task<ResponseBehavior<V>> SelectByIdAsync(int id);
    }
}
