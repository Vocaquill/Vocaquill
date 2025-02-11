using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DBService
    {
        public UserService UserService { get; private set; }
        public QueryService QueryService { get; private set; }
        public DBService()
        {
            _vocaquillDbContext = new VocaquillDbContext();
            
            _userRepository = new UserRepository(_vocaquillDbContext);
            _queryRepository = new QueryRepository(_vocaquillDbContext);
            
            UserService = new UserService(_userRepository);
            QueryService = new QueryService(_queryRepository);
        }

        private VocaquillDbContext _vocaquillDbContext;
        private IUserRepository _userRepository;
        private IQueryRepository _queryRepository;
    }
}
