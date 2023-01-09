using System.ComponentModel.DataAnnotations;

namespace Curly_TWM.Domain.Entities
{
    public class Jobs : BaiseEntity
    {
        [Display(Name = "المسمى الوظيفي")]
        public string JobName { get; set; }
    }


}

