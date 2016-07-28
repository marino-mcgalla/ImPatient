using PatientApp.Models;

namespace PatientApp.Services
{
    public interface IUserServices
    {
        ApplicationUser GetUser(string id);
        Patient GetPatient(string id);
        Nurse GetNurse(string id);
    }
}