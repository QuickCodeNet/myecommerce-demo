using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using System.Threading.Tasks;

namespace QuickCode.MyecommerceDemo.Common.Models
{
    public interface IBaseRepository<T> where T : class
    {
        Task<RepoResponse<T>> InsertAsync(T value);
        Task<RepoResponse<bool>> UpdateAsync(T value);
        Task<RepoResponse<bool>> DeleteAsync(T value);
        Task<RepoResponse<List<T>>> ListAsync(int? pageNumber = null, int? pageSize = null);
        Task<RepoResponse<int>> CountAsync();
    }
}