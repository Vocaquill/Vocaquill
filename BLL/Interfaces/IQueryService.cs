using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IQueryService
    {
        Task<ICollection<QueryDTO>> GetQueriesAsync();
        Task<QueryDTO> GetQueryByIdAsync(int id);
        Task<ICollection<QueryDTO>> GetQueriesByUserIdAsync(int userId);
        Task AddQueryAsync(QueryDTO query);
        Task UpdateQueryNameAsync(int id, string name);
        Task DeleteQueryAsync(int id);
    }
}
