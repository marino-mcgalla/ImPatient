using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int RoomNumber { get; set; }
        public int BedNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Notes { get; set; }
        public int NurseId { get; set; }
        public ICollection<NursePatient> NursePatients { get; set; }
        public int Dependency { get; set; }
        public bool IsActive { get; set;}
        public DateTime DateModifiedIfActive { get; set; }
        public string FirebasePatientKey { get; set; }
    }
}
