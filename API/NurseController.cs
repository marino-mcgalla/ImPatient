using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PatientApp.Services;
using Microsoft.AspNetCore.Authorization;
using PatientApp.Models;
using PatientApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PatientApp.API
{
    [Route("api/[controller]")]
    public class NurseController : Controller
    {
        private INurseServices _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public NurseController(INurseServices service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var nurses = _service.GetNurses();
            return Ok(nurses);
        }

        [HttpGet]
        [Route("getassignedpatients")]
        //[Authorize(Policy = "NurseOnly")]
        public IActionResult GetAssignedPatients(int id)
        {
            var patients = _service.GetAssignedPatients(id);
            return Ok(patients);
        }

        [HttpGet]
        [Route("getnursekeybyloginnurse")]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult GetNurseKeyByLoginNurse()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                ModelState.AddModelError("", "You are not authorized.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var nurse = _service.GetNurseKeyByLoginNurse(userId);
            return Ok(nurse);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int nurseId)
        {
            var nurse = _service.GetNurse(nurseId);
            return Ok(nurse);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        //this calls the Nurse services to save the nurse in application user table and nurse table
        // POST: /nurse/register
        [HttpPost("registernurse")]
        [Authorize]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RegisterNurse([FromBody]RegisteredNurseVm model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true,
                    DateModifiedIfActive = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("IsNurse", "true"));
                    var userViewModel = _service.CreateRegisteredNurse(model);
                    return Ok(userViewModel);
                }
                AddErrors(result);
            }
            // If we got this far, something failed
            return BadRequest(this.ModelState);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
