using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Partners : BaiseEntity
    {
        //قائمة
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }

        [Display(Name = "الشريك")]
        public string Partner { get; set; }
   
        [Display(Name = "دور الشريك")]
        public string Partner_rol { get; set; }
   
        [Display(Name = "عنوان الشريك")]
        public string Partner_add { get; set; }
    }


}

