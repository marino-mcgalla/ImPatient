using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Models
{
    public class NursePatient
    {
        public Nurse Nurse { get; set; }
        public int NurseId { get; set; }
        public Patient Patient { get; set; }
        public int PatientId { get; set; }
    }
}
