using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QEMS.Models
{
    public class SituationsOperators
    {
        [Key, Column(Order = 1), ForeignKey("Situation")]
        public int SituationId { get; set; }
        public Situation Situation { get; set; }
        [Key, Column(Order = 2),ForeignKey("Operator")]
        public int OperatorId { get; set; }
        public Operator Operator { get; set; }
        [Display(Name = "Time")]
        public string Time { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}