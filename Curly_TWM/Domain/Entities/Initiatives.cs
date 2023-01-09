using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Initiatives : BaiseEntity
    {
        [Display(Name = "عنوان المبادرة / المشروع")]
        public string InitiativeName { get; set; }

        [Display(Name = "التفاصيل")]
        public string InitiativeDeatails { get; set; }

        [Display(Name = "تاريخ البداية  ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Initiative_Sdt { get; set; }

        [Display(Name = "تاريخ النهاية  ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Initiative_Edt { get; set; }

        [Display(Name = "تاريخ الادخال  ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Initiative_Entry { get; set; }
        //قائمة
        [ForeignKey("Strategic_Targets")]
        [Display(Name = "الهدف الاستراتيجي")]
        public int Targetid { get; set; }
        public virtual Strategic_Targets Strategic_Targets { get; set; }
        //قائمة
        [ForeignKey("emp_main")]
        [Display(Name = "المسؤول")]
        public int empid { get; set; }
        public virtual emp_main emp_main { get; set; }

        public string InitiativeStat { get; set; }
    }


}

