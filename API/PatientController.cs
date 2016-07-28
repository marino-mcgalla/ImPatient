using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PatientApp.Services;
using PatientApp.Models;
using Microsoft.AspNetCore.Authorization;
using PatientApp.ViewModels;
using PatientApp.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PatientApp.API
{
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        private IPatientService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(IPatientService service, UserManager<ApplicationUser> userManager)

        {
            _service = service;
            this._userManager = userManager;
        }


        // GET: api/values
        [HttpGet]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult Get()
        {
            var patients = _service.GetPatients();
            return Ok(patients);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult Get(int id)
        {
            var patient = _service.GetPatient(id);
            return Ok(patient);
        }

        // GET api/values/5
        [HttpGet]
        [Route("getregisteredpatient/{id}")]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult GetRegisteredPatient(int id)
        {
            var patient = _service.GetRegisteredPatient(id);
            return Ok(patient);
        }

        //register patients
        // POST: /patient/register
        [HttpPost("registerpatient")]
        [Authorize(Policy = "NurseOnly")]
        public async Task<IActionResult> RegisterPatient([FromBody]RegisterPatientVm model)
        {
            //checks if all required fields are filled in
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
                //saves user to application user table
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //creates claim for patient
                    await _userManager.AddClaimAsync(user, new Claim("IsPatient", "true"));
                    var userViewModel = _service.CreateRegisteredPatient(model);
                    return Ok(userViewModel);
                }
                AddErrors(result);
            }
            // If we got this far, something failed
            return BadRequest(this.ModelState);
        }

        // POST api/values
        [HttpPost]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult Post([FromBody] PatientAppUserVm vm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }
            var patient = _service.SavePatient(vm.AssignedNurseId, vm.ApplicationUser, vm.Patient);
            return Ok(patient);
        }

        //GET: api/values/5 custom
        [HttpGet]
        [Route("getassignednurse")]
        //[Authorize(Policy = "NurseOnly")]
        public IActionResult GetAssignedNurse(int id)
        {
            var nurse = _service.GetAssignedNurse(id);
            return Ok(nurse);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int id)
        {
            var patient = _service.DeletePatient(id);
            return Ok();
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
