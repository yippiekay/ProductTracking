using AutoMapper;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private readonly IUnitOfWork service;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork db, IMapper mapper)
        {
            service = db;
            this.mapper = mapper;
        }

        public void Create(UserDTO userDTO)
        {
            if (userDTO == null)
                throw new Exception("Impossible to create a new user");
            
            service.Users.Create(MapUserDtoToUser(userDTO));
            service.Save();
        }

        public void Update(UserDTO userDTO)
        {
            if (!service.Users.Any(userDTO.Id)) 
                throw new Exception("There is no such user");

            service.Users.Update(MapUserDtoToUser(userDTO));
            service.Save();
        }

        public void Delete(int id)
        {
            if (!service.Users.Any(id))
                throw new Exception("There is no such user");
            
            service.Users.Delete(id);
            service.Save();
        }

        public UserDTO Get(int id)
        {
            var user = service.Users.Get(id);

            if (user == null)
                throw new Exception("There is no such user");
            
            return mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(service.Users.GetAll());
        }

        public bool Any(int id)
        {
            return service.Users.Any(id);
        }

        private User MapUserDtoToUser(UserDTO userDTO)
        {
            var user = mapper.Map<UserDTO, User>(userDTO);
            var role = service.Roles.Find(r => r.Name.ToLower() == userDTO.Role.ToLower()).FirstOrDefault();
            user.Role = role ?? throw new Exception("There is no such user");
            return user;
        }
    }
}
