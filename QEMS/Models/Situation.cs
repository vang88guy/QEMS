using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QEMS.Models
{
    public class Situation
    {
        [Key]
        public int SituationId { get; set; }
        [Display(Name = "Message")]
        public string Message { get; set; }
        [Display(Name = "Time")]
        public string Time { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
        [Display(Name = "Severity")]
        public int Severity { get; set; }
        [Display(Name = "Call Police Station")]
        public bool CallPoliceStation { get; set; }
        [Display(Name = "Call Fire Station")]
        public bool CallFireStation { get; set; }
        [Display(Name = "Call Ambulance")]
        public bool CallAmbulance { get; set; }
        [Display(Name = "In Process")]
        public bool InProcess { get; set; }
        [Display(Name = "Complete")]
        public bool Complete { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}