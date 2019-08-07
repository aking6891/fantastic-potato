using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Models
{
    public class ConvertModel
    {
        public double Input { get; set; }
        public double Output { get; set; }
        public List<RateModel> Rates { get; set; }
        public int? ApiId { get; set; }
        public IEnumerable<SelectListItem> ApiDropdown { get; set; }

        public string RateFromId { get; set; }
        public IEnumerable<SelectListItem> RatesFromDropdown { get; set; }

        public string RatesToId { get; set; }
        public IEnumerable<SelectListItem> RatesToDropdown { get; set; }

        public ConvertModel()
        {
            Rates = new List<RateModel>();
            ApiDropdown = new List<SelectListItem>();
            RatesFromDropdown = new List<SelectListItem>();
            RatesToDropdown = new List<SelectListItem>();
        }
    }
}