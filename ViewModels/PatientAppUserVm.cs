using PatientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.ViewModels
{
    public class PatientAppUserVm
    {
        public ApplicationUser ApplicationUser { get; set; }
        public Patient Patient { get; set; }
        public int AssignedNurseId { get; set; }
        public string PrimaryNurseFullName { get; set; }
        public string FirebasePatientKey { get; set; }
    }
}
