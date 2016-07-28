using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Models
{
    public class Nurse
    {
        public Nurse()
        {
            Messages = new List<Message>();
            NursePatients = new List<NursePatient>();
        }

        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public List<Message> Messages { get; set; }
        public ICollection<NursePatient> NursePatients { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateModifiedIfActive { get; set; }
        public string FirebaseNurseKey { get; set; }
    }
}
