using AutoMapper;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.DAL.Interfaces;
using ProductTracking.DAL.Models;
using System;
using System.Collections.Generic;

namespace ProductTracking.BLL.Services
{
    public class TaskService : IService<TaskDTO>
    {
        private readonly IUnitOfWork service;
        private readonly IMapper mapper;

        public TaskService(IUnitOfWork db, IMapper mapper)
        {
            service = db;
            this.mapper = mapper;
        }

        public void Create(TaskDTO taskDTO)
        {
            if(taskDTO == null)
                throw new Exception("Impossible to create a new taks");

            var task = mapper.Map<TaskDTO, Task>(taskDTO);
            task.User = null;
            service.Tasks.Create(task);
            service.Save();
        }

        public void Update(TaskDTO taskDTO)
        {
            if (taskDTO == null || !service.Tasks.Any(taskDTO.Id))
                throw new Exception("There is no such task");

            service.Tasks.Update(mapper.Map<TaskDTO, Task>(taskDTO));
            service.Save();
        }

        public void Delete(int id)
        {
            if (!service.Tasks.Any(id))
                throw new Exception("There is no such task");

            service.Tasks.Delete(id);
            service.Save();
        }

        public TaskDTO Get(int id)
        {
            var task = service.Tasks.Get(id);

            if (task == null)
                throw new Exception("There is no such task");

            return mapper.Map<Task, TaskDTO>(task);
        }

        public IEnumerable<TaskDTO> GetAll()
        {
            return mapper.Map<IEnumerable<Task>, IEnumerable<TaskDTO>>(service.Tasks.GetAll());
        }

        public bool Any(int id)
        {
            return service.Tasks.Any(id);
        }
    }
}
