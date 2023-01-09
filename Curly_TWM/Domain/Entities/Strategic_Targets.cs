using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Strategic_Targets : BaiseEntity
    {
        [Display(Name = "الهدف الاستراتيجي")]
        public string TargetName { get; set; }

        [ForeignKey("Strategic_Plans")]
        [Display(Name = "الخطة الاستراتيجية")]
        public int planid { get; set; }
        public virtual Strategic_Plans Strategic_Plans { get; set; }
    }
}
