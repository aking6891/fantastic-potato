using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        public List<RateModel> Rates { get; set; }
        public int? ApiId { get; set; }
        public IEnumerable<SelectListItem> ApiDropdown { get; set; }

        public IndexViewModel()
        {
            Rates = new List<RateModel>();
            ApiDropdown = new List<SelectListItem>();
        }
    }
}