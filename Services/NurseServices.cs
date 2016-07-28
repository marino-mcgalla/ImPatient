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
    public class NurseServices : INurseServices
    {
        private IGenericRepository _repo;

        public NurseServices(IGenericRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// returns list of nurses
        /// </summary>
        /// <returns></returns>
        public List<Nurse> GetNurses()
        {
            var nurses = _repo.Query<Nurse>().Include(n=>n.ApplicationUser).Include(n=>n.Messages).ToList();
            return nurses;
        }

        /// <summary>
        /// gets nurse info
        /// </summary>
        /// <param name="nurseId"></param>
        /// <returns></returns>
        public Nurse GetNurse(int nurseId)
        {
            var nurse = _repo.Query<Nurse>().Where(n => n.Id == nurseId).Include(n => n.ApplicationUser).FirstOrDefault();
            return nurse;
        }

        /// <summary>
        /// gets nurse info based on login user
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        public Nurse GetNurseKeyByLoginNurse(string appUserId)
        {
            var nurse = _repo.Query<Nurse>().Where(n => n.ApplicationUser.Id == appUserId).Include(n => n.Messages).FirstOrDefault();
            return nurse;
        }

        /// <summary>
        /// gets the assigned patients based on nurseId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PatientAppUserVm> GetAssignedPatients(int id)
        {
            var patients = _repo.Query<NursePatient>().Where(np => np.NurseId == id).Select(np => new PatientAppUserVm {
                Patient = np.Patient,
                ApplicationUser = np.Patient.ApplicationUser
            }).ToList();

            return patients;
        }

        /// <summary>
        /// creates the registered nurse based on the vm info passed from client side
        /// </summary>
        /// <param name="registeredUser"></param>
        /// <returns></returns>
        public Nurse CreateRegisteredNurse(RegisteredNurseVm registeredUser)
        {
            var newNurse = new Nurse();
            var appUser = _repo.Query<ApplicationUser>().Where(au => au.Email == registeredUser.Email).FirstOrDefault();

            newNurse.IsActive = true;
            newNurse.DateModifiedIfActive = DateTime.UtcNow;
            newNurse.ApplicationUser = appUser;
            newNurse.ApplicationUserId = appUser.Id;
            //newNurse.FirebaseNurseKey = registeredUser.FirebaseNurseKey;
            _repo.Add(newNurse);
            _repo.SaveChanges();

            return newNurse;
        }
    }
}
