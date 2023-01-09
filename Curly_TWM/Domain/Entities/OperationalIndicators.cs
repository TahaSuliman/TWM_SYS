using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class OperationalIndicators : BaiseEntity
    {
        //قائمة
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }

        [Display(Name = "المؤشر")]
        public string Indicator { get; set; }

        [Display(Name = "المستهدف")]
        public string Indicator_Target { get; set; }

        [Display(Name = "المحقق")]
        public string Indicator_Result { get; set; }


   
    }


}

