using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class MainDepartments: BaiseEntity
    {
        [Display(Name = "الادارة العامة")]
        public string MainDepartmentName { get; set; }

        [ForeignKey("emp_main")]
        [Display(Name = "المدير")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }
    }


}

