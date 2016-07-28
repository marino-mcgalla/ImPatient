using Microsoft.EntityFrameworkCore;
using PatientApp.Models;
using PatientApp.Repositories;
using PatientApp.ViewModels;
using PatientApp.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Services
{
    public class PatientService : IPatientService
    {
        private IGenericRepository _repo;

        public PatientService(IGenericRepository repo)
        {
            _repo = repo;
        }

        public List<Patient> GetPatients()
        {
            var patients = _repo.Query<Patient>().Where(p => p.IsActive == true).Include(p => p.ApplicationUser).ToList();
            return patients;
        }

        public ApplicationUser GetApplicationUser(string userName)
        {
            var appUser = _repo.Query<ApplicationUser>().Where(au => au.UserName == userName).FirstOrDefault();
            return appUser;
        }

        /// <summary>
        /// returns a single patient and the associated app user class
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Patient GetPatient(int id)
        {
            var patient = _repo.Query<Patient>().Where(p => p.Id == id).Include(p => p.ApplicationUser).FirstOrDefault();
            patient.CheckInDate = patient.CheckInDate.ToLocalTime();
            patient.CheckOutDate = patient.CheckOutDate.ToLocalTime();
            return patient;
        }

        /// <summary>
        /// used to retrieve patient info for edit patient
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public PatientAppUserVm GetRegisteredPatient(int patientId)
        {
            var registeredPatient = new PatientAppUserVm();
            var patient = _repo.Query<Patient>().Where(p => p.Id == patientId).Include(p => p.ApplicationUser).FirstOrDefault();
            patient.CheckInDate = patient.CheckInDate.ToLocalTime();
            patient.CheckOutDate = patient.CheckOutDate.ToLocalTime();

            var nurse = _repo.Query<Nurse>().Where(n => n.Id == patient.NurseId).Include(n => n.ApplicationUser).FirstOrDefault();

            if (nurse == null)
            {
                registeredPatient.Patient = patient;
                registeredPatient.ApplicationUser = patient.ApplicationUser;
            }
            else
            {
                registeredPatient.Patient = patient;
                registeredPatient.Patient.Dependency = patient.Dependency;
                registeredPatient.AssignedNurseId = patient.NurseId;
                registeredPatient.PrimaryNurseFullName = nurse.ApplicationUser.FirstName + " " + nurse.ApplicationUser.LastName;
                registeredPatient.ApplicationUser = patient.ApplicationUser;
            }
            return registeredPatient;
        }

        /// <summary>
        /// Creates the patient 
        /// </summary>
        /// <param name="registeredUser"></param>
        /// <returns></returns>
        public Patient CreateRegisteredPatient(RegisterPatientVm registeredUser)
        {
            var newPatient = new Patient();
            var appUser = _repo.Query<ApplicationUser>().Where(au => au.Email == registeredUser.Email).FirstOrDefault();

            newPatient.CheckInDate = registeredUser.CheckInDate.ToUniversalTime();
            newPatient.RoomNumber = registeredUser.RoomNumber;
            newPatient.BedNumber = registeredUser.BedNumber;
            newPatient.Notes = registeredUser.Notes;
            newPatient.NurseId = registeredUser.NurseId;
            newPatient.Dependency = registeredUser.Dependency;
            newPatient.IsActive = true;
            newPatient.DateModifiedIfActive = DateTime.UtcNow;
            newPatient.ApplicationUser = appUser;
            newPatient.ApplicationUserId = appUser.Id;
            newPatient.FirebasePatientKey = registeredUser.FirebasePatientKey;
            _repo.Add(newPatient);
            _repo.SaveChanges();
            //adds record to join table
            if (registeredUser.NurseId != 0)
            {
                var assignedNurse = _repo.Query<Nurse>().Include(n => n.ApplicationUser).Where(n => n.Id == newPatient.NurseId).FirstOrDefault();

                var patientAssignedNurse = new NursePatient();
                patientAssignedNurse.Patient = newPatient;
                patientAssignedNurse.PatientId = newPatient.Id;
                patientAssignedNurse.Nurse = assignedNurse;
                patientAssignedNurse.NurseId = assignedNurse.Id;

                _repo.Add<NursePatient>(patientAssignedNurse);
            }
            _repo.SaveChanges();
            return newPatient;
        }

        /// <summary>
        /// This handles the Edit patient 
        /// </summary>
        /// <param name="assignedNurseId"></param>
        /// <param name="patientAccountUser"></param>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Patient SavePatient(int assignedNurseId, ApplicationUser patientAccountUser, Patient patient)
        {
            var appUser = _repo.Query<ApplicationUser>().Where(au => au.Email == patientAccountUser.Email).FirstOrDefault();

            if (patient.Id == 0)
            {
                patient.CheckInDate = patient.CheckInDate.ToUniversalTime();
                patient.Dependency = patient.Dependency;
                patient.IsActive = true;
                patient.DateModifiedIfActive = DateTime.UtcNow;
                patient.ApplicationUser = appUser;
                patient.ApplicationUserId = appUser.Id;
                patient.NurseId = assignedNurseId;
                patient.FirebasePatientKey = patient.FirebasePatientKey;
                _repo.Add(patient);
            }
            else
            {
                var patientToEdit = _repo.Query<Patient>().Where(p => p.Id == patient.Id).Include(p => p.ApplicationUser).FirstOrDefault();

                patientToEdit.ApplicationUser.FirstName = patient.ApplicationUser.FirstName;
                patientToEdit.ApplicationUser.LastName = patient.ApplicationUser.LastName;
                patientToEdit.RoomNumber = patient.RoomNumber;
                patientToEdit.BedNumber = patient.BedNumber;
                patientToEdit.Dependency = patient.Dependency;
                patientToEdit.IsActive = true;  
                patientToEdit.CheckInDate = patient.CheckInDate.ToUniversalTime();
                patientToEdit.CheckOutDate = patient.CheckOutDate.ToUniversalTime();
                patientToEdit.NurseId = assignedNurseId;
                patientToEdit.Notes = patient.Notes;
                patientToEdit.FirebasePatientKey = patient.FirebasePatientKey;

            }

            //saves in the nurse patient join table if a nurse is selected
            if (assignedNurseId != 0 || patient.NurseId != 0)
            {
                //adds to the join table for new or redit
                var assignedNurse = _repo.Query<Nurse>().Include(n => n.ApplicationUser).Where(n => n.Id == assignedNurseId).FirstOrDefault();

                var patientAssignedNurse = new NursePatient();
                patientAssignedNurse.Patient = patient;
                patientAssignedNurse.PatientId = patient.Id;
                patientAssignedNurse.Nurse = assignedNurse;
                patientAssignedNurse.NurseId = assignedNurseId;

                var nursePatient = _repo.Query<NursePatient>().Where(np => np.PatientId == patient.Id).Include(np => np.Nurse).ThenInclude(n => n.ApplicationUser).FirstOrDefault();
                if (nursePatient == null)
                {
                    _repo.Add<NursePatient>(patientAssignedNurse);
                }
                else
                {
                    _repo.Delete<NursePatient>(nursePatient);
                    _repo.Add<NursePatient>(patientAssignedNurse);
                }

            }
            _repo.SaveChanges();
            return patient;
        }

        /// <summary>
        /// Returns the primary assigned nurse based on patient id
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public Nurse GetAssignedNurse(int patientId)
        {
            var query = _repo.Query<NursePatient>().Where(n => n.PatientId == patientId).Select(n => n.Nurse).FirstOrDefault();
           
            if (query == null)
            {
                return query;
            }
            var fullNurse = _repo.Query<Nurse>().Where(n => n.Id == query.Id).Include(n => n.ApplicationUser).FirstOrDefault();
            return fullNurse;
        }

        /// <summary>
        /// Soft deletes the patient and appUser patient record, removes the record in nurse patient join table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Patient DeletePatient(int id)
        {
            var patient = _repo.Query<Patient>().Where(p => p.Id == id).Include(p => p.ApplicationUser).FirstOrDefault();
            var patientAppUser = _repo.Query<ApplicationUser>().Where(au => au.Id == patient.ApplicationUser.Id).FirstOrDefault();
            var assignedNurses = _repo.Query<NursePatient>().Where(np => np.PatientId == patient.Id).ToList();

            foreach (var record in assignedNurses)
            {
                _repo.Delete<NursePatient>(record);
            }
            patient.IsActive = false;
            patient.DateModifiedIfActive = DateTime.UtcNow;
            patientAppUser.IsActive = false;
            patientAppUser.DateModifiedIfActive = DateTime.UtcNow;
            _repo.SaveChanges();
            return patient;
        }
    }
}
