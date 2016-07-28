using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PatientApp.Services;
using Microsoft.AspNetCore.Identity;
using PatientApp.Models;
using PatientApp.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PatientApp.API
{
    [Route("api/[controller]")]
    public class MyMessageController : Controller
    {
        private IMyMessagesService _service;
        private INurseServices _nurseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyMessageController(IMyMessagesService service, INurseServices nurseService, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._nurseService = nurseService;
            this._userManager = userManager;
        }

        //this is used for patient's my messages page
        [HttpGet]
        [Route("getmymessages")]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult GetMyMessages()
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

            var nurse = this._service.GetMyMessages(userId);
            return Ok(nurse);
        }

        //this is for nurse to retrieve their own patients messages info
        [HttpGet]
        [Route("getpatientmessages")]
        //[Authorize(Policy = "NurseOnly")]
        public IActionResult GetPatientMessages()
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

            var patientsMessages = _service.GetPatientsMessages(userId);
            return Ok(patientsMessages);
        }

        //this is for admin to retrieve the selected nurse messages activities
        [HttpGet("{nurseAppUserId}")]
        [Route("getnursepatientsmessages/{nurseAppUserId}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetNursePatientsMessages(string nurseAppUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var patientsMessages = _service.GetPatientsMessages(nurseAppUserId);
            return Ok(patientsMessages);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public void Get(int id)
        {
        }

        // POST api/myMessage/dismissincident
        [HttpPost]
        [Route("dismissincident")]
        [Authorize(Policy = "NurseOnly")]
        public IActionResult DismissIncident([FromBody]int messageId)
        {
            var userId = _userManager.GetUserId(User);
            var nurse = _service.DismissIncident(userId, messageId);

            return Ok(nurse);
        }

        //POST custom sendMessage
        [HttpPost("sendmessage")]
        public IActionResult SendMessage([FromBody]NewMessageVM VM)
        {
            //var userId = _userManager.GetUserId(User);
            _service.SendMessage(VM.UserId, VM.MessageString);
            return Ok();
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
