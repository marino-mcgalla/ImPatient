using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PatientApp.Models;
using Microsoft.AspNetCore.Identity;
using PatientApp.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PatientApp.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserServices _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IUserServices repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        // GET: api/values
        [HttpGet("getcurrentuser")]
        public IActionResult GetCurrentUser()
        {
            var userId = _userManager.GetUserId(User);
            var user = _repo.GetNurse(userId);

            return Ok(user);
        }

        // GET: api/values
        [HttpGet("getcurrentpatient")]
        public IActionResult GetCurrentPatient()
        {
            var userId = _userManager.GetUserId(User);
            var user = _repo.GetPatient(userId);

            return Ok(user);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
