using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Sections : BaiseEntity
    {
        [Display(Name = "القسم")]
        public string SectiontName { get; set; }

        [ForeignKey("Departments")]
        [Display(Name = "الادارة")]
        public int Department_id { get; set; }
        public virtual Departments Departments { get; set; }

        [ForeignKey("emp_main")]
        [Display(Name = "المدير")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }
    }


}

