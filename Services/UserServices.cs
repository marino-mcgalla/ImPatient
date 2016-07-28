using Microsoft.EntityFrameworkCore;
using PatientApp.Models;
using PatientApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Services
{
    public class UserServices : IUserServices
    {
        private IGenericRepository _repo;

        public UserServices(IGenericRepository repo)
        {
            _repo = repo;
        }

        public Patient GetPatient(string id)
        {
            var patient = _repo.Query<Patient>().Where(p => p.ApplicationUserId == id).Include(p => p.ApplicationUser).FirstOrDefault();
            return patient;
        }

        public Nurse GetNurse(string nurseAppUserId)
        {
            var nurse = _repo.Query<Nurse>().Where(n => n.ApplicationUserId == nurseAppUserId).Include(n => n.ApplicationUser).FirstOrDefault();
            return nurse;
        }

        public ApplicationUser GetUser(string id)
        {
            var user = _repo.Query<ApplicationUser>().Where(u => u.Id == id).FirstOrDefault();
            return user;
        }
    }
}
