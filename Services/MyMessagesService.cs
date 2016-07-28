using Microsoft.EntityFrameworkCore;
using PatientApp.Models;
using PatientApp.Repositories;
using PatientApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Services
{
    public class MyMessagesService : IMyMessagesService
    {
        private IGenericRepository _repo;

        public MyMessagesService(IGenericRepository repo)
        {
            this._repo = repo;
        }

        /// <summary>
        /// This returns only the pending messages for the nurse
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public Nurse GetMyMessages(string appUserId)
        {
            var nurse = _repo.Query<Nurse>().Where(n => n.ApplicationUserId == appUserId).Include(n => n.ApplicationUser).Include(n => n.Messages).ThenInclude(m => m.Patient).ThenInclude(p => p.ApplicationUser).FirstOrDefault();
            if (nurse == null)
            {
                return nurse;
            }
            else
            {
                nurse.Messages = nurse.Messages.Where(m => m.TimeResponded == new System.DateTime(0001, 01, 01)).ToList();
                nurse = DisplayLocalTime(nurse);
            }
            return nurse;
        }

        /// <summary>
        /// returns the all patients messages and its unresolved messages count to determine the urgency level (bed color) for the nurse 
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public List<PatientMessagesVm> GetPatientsMessages(string appUserId)
        {
            var patientsMessages = new List<PatientMessagesVm>();
            var patients = new List<Patient>();

            //returns nurse messages records
            var nurse = _repo.Query<Nurse>().Where(n => n.ApplicationUserId == appUserId).Include(n => n.ApplicationUser).Include(n => n.Messages).ThenInclude(m => m.Patient).ThenInclude(p => p.ApplicationUser).FirstOrDefault();
            //returns the nurse info based on the param appuserid
            var appUserNurse = _repo.Query<Nurse>().Where(n => n.ApplicationUser.Id == appUserId).Include(n => n.ApplicationUser).FirstOrDefault();

            //if no patients assigned to this nurse
            if (nurse == null)
            {
                //if(assignedNursePatients == null)
                return patientsMessages;
            }
            else
            {
                nurse = DisplayLocalTime(nurse);

                //adds unique patient object to the patients list
                List<int> patientIds = new List<int>();
                if (nurse.Messages.Count() != 0)
                {
                    patients.Add(nurse.Messages[0].Patient);
                    patientIds.Add(nurse.Messages[0].Patient.Id);
                    //loops through nurse's messages
                    for (int i = 1; i < nurse.Messages.Count(); i++)
                    {
                        //if new patient, adds newly assigned patient
                        var patientId = nurse.Messages[i].Patient.Id;
                        if (!patientIds.Contains(patientId))
                        {
                            patientIds.Add(nurse.Messages[i].Patient.Id);
                            patients.Add(nurse.Messages[i].Patient);
                        }
                    }
                    //loops through unresolved messages 
                    for (int k = 0; k < patients.Count(); k++)
                    {
                        var messageCount = 0;
                        var UnresolvedMessagesCount = 0;
                        var vm = new PatientMessagesVm();
                        List<Message> messages = new List<Message>();
                        for (int l = 0; l < nurse.Messages.Count(); l++)
                        {
                            if (nurse.Messages[l].Patient.Id == patients[k].Id)
                            {
                                messageCount++;
                                if (nurse.Messages[l].TimeResponded == new System.DateTime(0001, 01, 01))
                                {
                                    UnresolvedMessagesCount++;
                                    messages.Add(nurse.Messages[l]);
                                }
                            }
                        }
                        //view model to display unresolved message bed color
                        vm.Nurse = nurse;
                        vm.Patient = patients[k];
                        vm.Messages = messages;
                        vm.MessageCount = messageCount;
                        vm.UnresolvedMessagesCount = UnresolvedMessagesCount;
                        vm.ImagePath = GetIdentifiedImage(UnresolvedMessagesCount);
                        var ifUrgent = CheckIfMessagesContainsEmergency(messages);
                        if (UnresolvedMessagesCount < 5 && UnresolvedMessagesCount > 0)
                        {
                            vm.ImagePath = GetIdentifiedImage(1); //yellow
                        }
                        if (ifUrgent)
                        {
                            vm.ImagePath = GetIdentifiedImage(2); //red
                        }
                        patientsMessages.Add(vm);
                    }
                }

                var assignednursepatients = _repo.Query<NursePatient>().Where(np => np.NurseId == appUserNurse.Id).Select(np => new
                {
                    Nurse = np.Nurse,
                    Patient = np.Patient,
                    PatientAppUser = np.Patient.ApplicationUser
                }).ToList();

                foreach (var item in assignednursepatients)
                {
                    var vm = new PatientMessagesVm();
                    if (!patientIds.Contains(item.Patient.Id))
                    {
                        vm.Nurse = item.Nurse;
                        vm.Messages = new List<Message>();
                        vm.MessageCount = 0;
                        vm.Patient = item.Patient;
                        vm.UnresolvedMessagesCount = 0;
                        vm.ImagePath = GetIdentifiedImage(0);
                        patientsMessages.Add(vm);
                    }
                }
            }
            return patientsMessages.ToList();
        }

        /// <summary>
        /// This updates the Time Responded for the message request and updates the view to only seeing the pending message
        /// </summary>
        /// <param name="appUserId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public Nurse DismissIncident(string appUserId, int messageId)
        {
            var nurse = _repo.Query<Nurse>().Where(n => n.ApplicationUserId == appUserId).Include(n => n.ApplicationUser).Include(n => n.Messages).ThenInclude(m => m.Patient).ThenInclude(p => p.ApplicationUser).FirstOrDefault();
            if (nurse == null)
            {
                return nurse;
            }
            else
            {
                foreach (var selectedMessage in nurse.Messages)
                {
                    if (selectedMessage.Id == messageId)
                    {
                        selectedMessage.TimeResponded = DateTime.UtcNow;
                        _repo.SaveChanges();
                    }
                }
                nurse.Messages = nurse.Messages.Where(m => m.TimeResponded == new System.DateTime(0001, 01, 01)).ToList();
                nurse = DisplayLocalTime(nurse);
            }
            return nurse;
        }

        /// <summary>
        /// updates the message table when a patient sends a message 
        /// </summary>
        /// <param name="appUserId"></param>
        /// <param name="message"></param>
        public void SendMessage(string appUserId, string message)
        {
            var patient = _repo.Query<Patient>().Where(p => p.ApplicationUserId == appUserId).Include(p => p.ApplicationUser).FirstOrDefault();
            var nurses = _repo.Query<NursePatient>().Where(np => np.PatientId == patient.Id).Include(np => np.Nurse).ThenInclude(n => n.Messages).ToList();

            foreach (var nurse in nurses)
            {
                Message messageTest = new Message();
                messageTest.MessageString = message;
                messageTest.Patient = patient;
                messageTest.PatientId = patient.Id;
                messageTest.TimeRequested = DateTime.UtcNow;
                nurse.Nurse.Messages.Add(messageTest);
            };
            _repo.SaveChanges();
        }

        //checks if messages contain any emergency request
        public bool CheckIfMessagesContainsEmergency(List<Message> messages)
        {
            bool urgent = false;
            foreach (var item in messages)
            {
                if (item.MessageString.ToLower().Trim() == "emergency")
                {
                    urgent = true;
                    break;
                }
            }

            return urgent;
        }

        //local method to identify the urgency bed color
        public string GetIdentifiedImage(int unresolvedMessage)
        {
            var imagePath = "";
            switch (unresolvedMessage)
            {
                case 0:
                    imagePath = "../../images/greenBedIcon.png";
                    break;
                case 1: //unresolved less than 5 counts
                    imagePath = "../../images/yellowBedIcon.png";
                    break;
                case 2:  //emergency
                    imagePath = "../../images/redBedIcon.png";
                    break;
                default: //more than 5 counts
                    imagePath = "../../images/redBedIcon.png";
                    break;
            }
            return imagePath;
        }

        //local method to update the utc time to local time for display in html
        public Nurse DisplayLocalTime(Nurse nurse)
        {
            foreach (var message in nurse.Messages)
            {
                message.TimeRequested = message.TimeRequested.ToLocalTime();
                message.TimeResponded = message.TimeResponded.ToLocalTime();
            }
            return nurse;
        }
    }

}

