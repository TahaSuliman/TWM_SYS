using System;
using System.ComponentModel.DataAnnotations;

namespace Curly_TWM.Domain.Entities
{
    public class Strategic_Plans : BaiseEntity
    {
        [Display(Name = "عنوان الخطة")]
        public string PlanName { get; set; }

        [Display(Name = "تاريخ بداية الخطة ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Plan_Sdt { get; set; }

        [Display(Name = "تاريخ نهاية الخطة ")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Plan_Edt { get; set; }

        public string Plan_Title { get; set; }
    }


}

