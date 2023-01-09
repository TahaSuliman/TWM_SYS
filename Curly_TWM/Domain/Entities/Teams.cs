using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Teams : BaiseEntity
    {
        //قائمة
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }
        //قائمة
        [ForeignKey("emp_main")]
        [Display(Name = "الموظف")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }

    }


}

