using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.ViewModels;
using System;
using System.Collections.Generic;

namespace ProductTracking.Controllers
{
    [ApiController, Authorize(Roles = "admin")]
    [Route("api/Account/admin/[controller]")]
    public class UserController : Controller
    {
        readonly IService<UserDTO> service;
        readonly IMapper mapper;
        
        public UserController(IService<UserDTO> context, IMapper mapper)
        {
            service = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> GetAll()
        {
            var user = service.GetAll();

            return new ObjectResult(mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(user));
        }
      
        [HttpGet("{id}")]
        public ActionResult<UserViewModel> Get(int id)
        {
            try
            {
                var user = service.Get(id);

                return new ObjectResult(mapper.Map<UserDTO, UserViewModel>(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<RegistrationModel> Create(RegistrationModel user)
        {
            try
            {
                var userDTO = mapper.Map<RegistrationModel, UserDTO>(user);
                service.Create(userDTO);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<UserViewModel> Update(UserViewModel user)
        {
            try
            {
                service.Update(mapper.Map<UserViewModel, UserDTO>(user));

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<UserViewModel> Delete(int id)
        {
            try
            {
                if (!service.Any(id))
                    return NotFound();

                service.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
