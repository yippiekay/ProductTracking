using AutoMapper;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;

namespace ProductTracking.BLL.Services
{
    public class RoleService : IService<RoleDTO>
    {
        private readonly IUnitOfWork service;
        private readonly IMapper mapper;

        public RoleService(IUnitOfWork db, IMapper mapper)
        {
            service = db;
            this.mapper = mapper;
        }

        public void Create(RoleDTO roleDTO)
        {
            if (roleDTO == null)
                throw new Exception("impossible to create a new role");

            service.Roles.Create(mapper.Map<RoleDTO, Role>(roleDTO));
            service.Save();
        }

        public void Update(RoleDTO roleDTO)
        {
            var role = service.Roles.Get(roleDTO.Id);

            if (role == null)
                throw new Exception("there is no such role");

            service.Roles.Update(mapper.Map<RoleDTO, Role>(roleDTO));
            service.Save();
        }

        public void Delete(int id)
        {
            if (!service.Roles.Any(id))
                throw new Exception("there is no such role");
            
            service.Roles.Delete(id);
            service.Save();
        }

        public RoleDTO Get(int id)
        {
            var role = service.Roles.Get(id);

            if (role == null)
                throw new Exception("there is no such role");
            
            return mapper.Map<Role, RoleDTO>(role);
        }

        public IEnumerable<RoleDTO> GetAll()
        {
            return mapper.Map<IEnumerable<Role>, IEnumerable<RoleDTO>>(service.Roles.GetAll());
        }

        public bool Any(int id)
        {
            return service.Roles.Any(id);
        }
    }
}