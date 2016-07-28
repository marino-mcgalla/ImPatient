using System.Collections.Generic;
using PatientApp.Models;
using PatientApp.ViewModels;

namespace PatientApp.Services
{
    public interface IPatientService
    {
        List<Patient> GetPatients();
        Patient GetPatient(int id);
        Nurse GetAssignedNurse(int id);
        Patient DeletePatient(int id);
        //Patient SavePatient(ApplicationUser patientAccountUser, Patient patient);
        Patient SavePatient(int assignedNurseId, ApplicationUser patientAccountUser, Patient patient);
        ApplicationUser GetApplicationUser(string userName);
        PatientAppUserVm GetRegisteredPatient(int id);
        //RegisterPatientVm CreateRegisteredPatient(ApplicationUser patientAccountUser);
        Patient CreateRegisteredPatient(RegisterPatientVm registeredUser);
    }
}