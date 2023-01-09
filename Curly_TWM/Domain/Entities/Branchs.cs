using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Branchs : BaiseEntity
    {
        [Display(Name = "الفرع")]
        public string BranchsName { get; set; }

        [ForeignKey("Sections")]
        [Display(Name = "القسم")]
        public int Section_id { get; set; }
        public virtual Sections Sections { get; set; }

        [Display(Name = "المدير")]
        public int empid { get; set; }
   
    }


}

