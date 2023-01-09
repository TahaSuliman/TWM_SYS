using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Curly_TWM.Domain.Entities
{
    public class uploaddocs : BaiseEntity
    {
        [ForeignKey("emp_main")]
        [Display(Name = "اسم الموظف")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }
        [Display(Name = "نوع المستند")]
        public string docu_type { get; set; }    //List 
        [Display(Name = "الملف")]
        public string doc_url { get; set; }
        [Display(Name = "وصف المستند")]
        public string doc_discription { get; set; }
        [NotMapped]
        public HttpPostedFileBase doc_file { get; set; }

    } 

}