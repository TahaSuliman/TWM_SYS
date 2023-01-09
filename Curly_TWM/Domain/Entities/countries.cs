using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Curly_TWM.Domain.Entities
{
    public class countries:BaiseEntity
    {
        public string code { get; set; }
        public string name { get; set; }
        [Display(Name = "الجنسية")]
        public string ar_name { get; set; }
        public string en_name { get; set; }
        public string calling_code { get; set; }

    }
}