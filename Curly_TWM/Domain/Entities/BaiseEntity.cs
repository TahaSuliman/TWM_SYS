using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Curly_TWM.Domain.Entities
{
    public class BaiseEntity
    {
        public int Id { get; set; }
        [Display(Name = "ملاحظات")]
        public string remarks { get; set; }
    }
}