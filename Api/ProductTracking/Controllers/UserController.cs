using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductTracking.BLL.Interfase;
using ProductTracking.BLL.Models;
using ProductTracking.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ProductTracking.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        readonly IService<UserDTO> db;
        readonly IMapper mapper;
        public UserController(IService<UserDTO> context, IMapper mapper)
        {
            db = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> Get()
        {
            return new ObjectResult(mapper.Map<IEnumerable<UserDTO>, IEnumerable<UserViewModel>>(db.GetAll()));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ActionResult<UserViewModel> Get(int id)
        {
            var user = db.GetAll().FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            return new ObjectResult(mapper.Map<UserDTO, UserViewModel>(user));
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<UserViewModel> Post(RegisterModel user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            db.Create(mapper.Map<RegisterModel, UserDTO>(user));
            return Ok(user);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public ActionResult<UserViewModel> Put(UserViewModel user)
        {
            if (user == null || !db.GetAll().Any(u => u.UserId == user.UserId))
            {
                return BadRequest();
            }
           
            db.Update(mapper.Map<UserViewModel, UserDTO>(user));
           
            return Ok(user);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public ActionResult<UserViewModel> Delete(int id)
        {
            var user = db.GetAll().FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            db.Delete(user);
           
            return Ok(user);
        }
    }
}
