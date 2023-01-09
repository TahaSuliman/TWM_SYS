using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Curly_TWM.Domain.Entities
{
    public class emp_main:BaiseEntity
    {
        //        TABLE
        [Display(Name = "الاسم باللغة العربية")]
        public string emp_arnm { get; set; }

        [Display(Name = "Full Name")]
        public string emp_ennm { get; set; }

        [Display(Name = "الجنسية")]
        public string nat { get; set; }

        [ForeignKey("Jobs")]
        [Display(Name = "المسمى الوظيفي")]
        public int? Job_id { get; set; }
        public virtual Jobs Jobs { get; set; }

        [Display(Name = "الرقم الوظيفي")]
        public int emp_code { get; set; }

        [Display(Name = "الوصف الوظيفي")]
        public string job_discription { get; set; }

        [ForeignKey("Branchs")]
        [Display(Name = "الفرع")]
        public int? Branch_id { get; set; }
        public virtual Branchs Branchs { get; set; }

        [Display(Name = "النوع")]
        public string gender_Id { get; set; }

        [Display(Name = "رقم الاتصال")]
        public string emp_mobile { get; set; }

        [Display(Name = "رقم الاتصال البديل")]
        public string emp_mobile2 { get; set; }

        [Display(Name = "العنوان")]
        public string address { get; set; }       
        
        [Display(Name = "التصنيف")]
        public string emp_class { get; set; }

    }
}

