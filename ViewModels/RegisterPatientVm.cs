using PatientApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.ViewModels
{
    public class RegisterPatientVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int BedNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Dependency { get; set; }
        public string Notes { get; set; }
        public int NurseId { get; set; }
        public ICollection<NursePatient> NursePatients { get; set; }
        public string PrimaryNurseFullName { get; set; }
        public string FirebasePatientKey { get; set; }
    }
}
