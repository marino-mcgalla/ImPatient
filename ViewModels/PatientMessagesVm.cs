using PatientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.ViewModels
{
    public class PatientMessagesVm
    {
        public Nurse Nurse { get; set; }
        public Patient Patient{ get; set; }
        public List<Message> Messages { get; set; }
        public int MessageCount { get; set; }
        public int UnresolvedMessagesCount { get; set; }
        public string ImagePath { get; set; }
    }
}
