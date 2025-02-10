using DAL.Context;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class QueryRepository : IQueryRepository
    {
        private VocaquillDbContext _context;

        public QueryRepository(VocaquillDbContext context)
        {
            _context = context;
        }

        public async Task AddQueryAsync(Query query)
        {
            await _context.AddAsync(query);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQueryAsync(int id)
        {
            var query = await _context.Queries.FindAsync(id);
            if (query != null)
            {
                _context.Queries.Remove(query);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Query not found");
            }
        }

        public async Task<IEnumerable<Query>> GetQueriesAsync()
        {
            return await _context.Queries.ToListAsync();
        }

        public async Task UpdateQueryNameAsync(int id, string name)
        {
            var existingQuery = await _context.Queries.FindAsync(id);
            if (existingQuery != null)
            {
                existingQuery.Name = name;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Query not found");
            }
        }
    }
}
