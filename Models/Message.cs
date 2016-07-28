using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public string MessageString { get; set; }
        public DateTime TimeRequested { get; set; }
        public DateTime TimeResponded { get; set; }
    }
}
