using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Departments : BaiseEntity
    {
        [Display(Name = "الادارة")]
        public string DepartmentName { get; set; }

        [ForeignKey("MainDepartments")]
        [Display(Name = "الادارة العامة")]
        public int MainDepartment_id { get; set; }
        public virtual MainDepartments MainDepartments { get; set; }

        [ForeignKey("emp_main")]
        [Display(Name = "المدير")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }
    } 


}

