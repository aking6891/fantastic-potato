using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class RateModel : IRateModel
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
}