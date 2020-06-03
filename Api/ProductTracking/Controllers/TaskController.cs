using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.Controllers
{
    [ApiController,Authorize]
    [Route("api/Account/[controller]")]
    public class TaskController : ControllerBase
    {
        readonly IService<TaskDTO> taskService;
        readonly IService<UserDTO> userService;
        readonly IMapper mapper;

        public TaskController(IService<TaskDTO> taskService, IService<UserDTO> userService, IMapper mapper)
        {
            this.taskService = taskService;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<TaskViewModel> GetAll()
        {
            return new ObjectResult(GetUser().Tasks);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskViewModel> Get(int id)
        {
            try
            {
                var taskDTO = taskService.Get(id);

                return mapper.Map<TaskDTO, TaskViewModel>(taskDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/Account/admin/Task"), Authorize(Roles = "admin")]
        public ActionResult<TaskViewModel> GetAllTasks()
        {
            return new ObjectResult(mapper.Map<IEnumerable<TaskDTO>, IEnumerable<TaskViewModel>>(taskService.GetAll()));
        }

        [HttpPost]
        public ActionResult<TaskViewModel> Create(CreateTaskModel taskModel)
        {
            try
            {
                var task = mapper.Map<CreateTaskModel, TaskDTO>(taskModel);
                task.User = mapper.Map<UserViewModel, UserDTO>(GetUser());
                taskService.Create(task);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<TaskViewModel> Update(TaskViewModel taskModel)
        {
            try
            {
                if (taskModel == null || !GetUser().Tasks.Any(t => t.Id == taskModel.Id))
                    return BadRequest();

                var task = mapper.Map<TaskViewModel, TaskDTO>(taskModel);
                task.User = mapper.Map<UserViewModel, UserDTO>(GetUser());
                taskService.Update(task);

                return Ok(taskModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<TaskViewModel> Delete(int id)
        {
            try
            {
                if (!GetUser().Tasks.Any(t => t.Id == id))
                    return NotFound();

                taskService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private UserViewModel GetUser()
        {
            var claims = User.Claims.ToArray();
            var email = claims[0].Value;
            var user = userService.GetAll().Where(u => u.Email == email).FirstOrDefault();

            return mapper.Map<UserDTO, UserViewModel>(user);
        }
    }
}