using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class RiskList : BaiseEntity
    {
        //قائمة
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }

        [Display(Name = "الخطر")]
        public string Risk { get; set; }
   
        [Display(Name = "نوع الخطر")]
        public string Risk_Type { get; set; }
   
        [Display(Name = "احتمالية وقوع الخطر")]
        public string Risk_class { get; set; }

        [Display(Name = "الاجراءات الاحترازية")]
        public string Risk_Plan { get; set; }


    }


}

