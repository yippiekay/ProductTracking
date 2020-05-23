using AutoMapper;
using ProductTracking.BLL.Infrastructure;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.BLL.Services
{
    public class UserService : IService<UserDTO>
    {
        private readonly IUnitOfWork database;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork context, IMapper mapper)
        {
            database = context;
            this.mapper = mapper;
        }

        public void Create(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ValidationException("user is not found", "");
            }

            database.Users.Create(mapper.Map<UserDTO, User>(userDTO));
            database.Save();
        }

        public void Update(UserDTO userDTO)
        {
            var user = mapper.Map<UserDTO, User>(userDTO);
            if (user == null)
            {
                throw new ValidationException("user is not found", "");
            }

            database.Users.Update(user);
            database.Save();
        }

        public void Delete(UserDTO userDTO)
        {
            var user = database.Users.GetAll().FirstOrDefault(u => u.UserId == userDTO.UserId);
            if (user != null)
            {
                throw new ValidationException("user is not found", "");
            }

            database.Users.Delete(user.UserId);
            database.Save();
        }

        public UserDTO Get(int id)
        {
            var user = database.Users.GetAll().FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                throw new ValidationException("user is not found", "");
            }

            return mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            var users = database.Users.GetAll();
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
        }
    }
}
