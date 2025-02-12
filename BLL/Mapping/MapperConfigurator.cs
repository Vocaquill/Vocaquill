using AutoMapper;
using BLL.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public static class MapperConfigurator
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<User, UserDTO>();

                cfg.CreateMap<QueryDTO, Query>();
                cfg.CreateMap<Query, QueryDTO>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => _mapper.Value;
    }
}
