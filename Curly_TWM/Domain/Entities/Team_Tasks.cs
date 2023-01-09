using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curly_TWM.Domain.Entities
{
    public class Team_Tasks : BaiseEntity
    {
        [Display(Name = "المهمة")]
        public string TaskName { get; set; }

        [Display(Name = "التفاصيل")]
        public string TaskDeatails { get; set; }

        [Display(Name = "اخر موعد للانجاز")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Task_Edt { get; set; }

        [Display(Name = "تاريخ الاستجابة  ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Task_Ractdt { get; set; }

        [Display(Name = "تاريخ الادخال  ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Initiative_Entry { get; set; }
        //قائمة
        [ForeignKey("Initiatives")]
        [Display(Name = "المبادرة / المشروع")]
        public int Initiativeid { get; set; }
        public virtual Initiatives Initiatives { get; set; }
        //قائمة
        [ForeignKey("Teams")]
        [Display(Name = "الموظف")]
        public int empid { get; set; }
        public virtual Teams Teams { get; set; }

        public string TaskStat { get; set; }
        //public string Comments { get; set; }
    }


}

