using AutoMapper;
using BLL.Interfaces;
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
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<User, UserDTO>();
            }).CreateMapper();
        }

        public async Task AddUserAsync(UserDTO user)
        {
            await _repository.AddUserAsync(_mapper.Map<User>(user));
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repository.DeleteUserAsync(id);
        }

        public async Task<ICollection<UserDTO>> GetUsersAsync()
        {
            return (await this._repository.GetUsersAsync()).Select(c => this._mapper.Map<UserDTO>(c)).ToList();
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            return (await this._repository.GetUsersAsync()).Select(c => this._mapper.Map<UserDTO>(c)).First(e => e.Email == email);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            return (await this._repository.GetUsersAsync()).Select(c => this._mapper.Map<UserDTO>(c)).First(i => i.Id == id);
        }

        public async Task<UserDTO> GetUserByLoginAsync(string login)
        {
            return (await this._repository.GetUsersAsync()).Select(c => this._mapper.Map<UserDTO>(c)).First(l => l.Login == login);
        }

        public async Task<UserDTO> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            return (await this._repository.GetUsersAsync()).Select(c => this._mapper.Map<UserDTO>(c)).First(lp => lp.Login == login && lp.Password == password);
        }

        public async Task UpdateUserAsync(int id, UserDTO user)
        {
            var existingUser = await _repository.GetUsersAsync();
            var userToUpdate = existingUser.FirstOrDefault(u => u.Id == id);

            if (userToUpdate != null)
            {
                var updatedUser = _mapper.Map<User>(user);
                await _repository.UpdateUserAsync(id, updatedUser);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}
