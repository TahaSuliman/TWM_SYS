using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Curly_TWM.Domain.Entities
{
    public class InitiativesTasksUploas : BaiseEntity
    {
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }     
        [ForeignKey("Team_Tasks")]
        [Display(Name = "المهمة")]
        public int TeamTasksid { get; set; }
        public virtual Team_Tasks Team_Tasks { get; set; }
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