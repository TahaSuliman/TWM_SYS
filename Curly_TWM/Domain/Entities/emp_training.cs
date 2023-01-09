using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class emp_training : BaiseEntity
    {
        [ForeignKey("emp_main")]
        [Display(Name = "اسم الموظف")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }
        [Display(Name = "اسم الدورة / البرنامج")]
        public string training_name { get; set; }
        [Display(Name = "التاريخ ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime training_dt { get; set; }

    } 

}