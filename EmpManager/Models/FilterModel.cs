using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmpManager.Models
{
    public class FilterModel
    {
        public string Search { get; set; }
        [DisplayFormat( DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public string Sdate { get; set; }
        [DisplayFormat( DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public string Edate { get; set; }
        public int Pagenum { get; set; }
        public int Pagesize { get; set; }
        public IEnumerable<SelectListItem> RecordserPage = new List<SelectListItem>()
        {
           new SelectListItem
           {
               Value="5",
               Text="5",
               Selected=true
           },
           new SelectListItem
           {
               Value="10",
               Text="10",
               Selected=false
           }, new SelectListItem
           {
               Value="20",
               Text="20",
               Selected=false
           },new SelectListItem
           {
               Value="100",
               Text="100",
               Selected=false
           }
        };




    }
}
