using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IQueryRepository
    {
        Task<IEnumerable<Query>> GetQueriesAsync();
        Task AddQueryAsync(Query query);
        Task UpdateQueryNameAsync(int id, string name);
        Task DeleteQueryAsync(int id);
    }
}
