using AutoMapper;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.Models;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class QueryService : IQueryService
    {
        private IQueryRepository _repository;
        private readonly IMapper _mapper;

        public QueryService(IQueryRepository repository)
        {
            _repository = repository;
            _mapper = MapperConfigurator.Mapper;
        }

        public async Task AddQueryAsync(QueryDTO query)
        {
            await _repository.AddQueryAsync(_mapper.Map<Query>(query));
        }

        public async Task DeleteQueryAsync(int id)
        {
            await _repository.DeleteQueryAsync(id);
        }

        public async Task<ICollection<QueryDTO>> GetQueriesAsync()
        {
            return (await this._repository.GetQueriesAsync()).Select(c => this._mapper.Map<QueryDTO>(c)).ToList();
        }

        public async Task<QueryDTO> GetQueryByIdAsync(int id)
        {
            return (await this._repository.GetQueriesAsync()).Select(c => this._mapper.Map<QueryDTO>(c)).First(i => i.Id == id);
        }

        public async Task<ICollection<QueryDTO>> GetQueriesByUserIdAsync(int userId)
        {
            return (await this._repository.GetQueriesAsync()).Select(c => this._mapper.Map<QueryDTO>(c)).Where(u => u.UserId == userId).ToList();
        }

        public async Task UpdateQueryNameAsync(int id, string name)
        {
            var existingUser = await _repository.GetQueriesAsync();
            var userToUpdate = existingUser.FirstOrDefault(u => u.Id == id);

            if (userToUpdate != null)
            {
                await _repository.UpdateQueryNameAsync(id, name);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
