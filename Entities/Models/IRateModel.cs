using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models
{
    public interface IRateModel
    {
        string Iso { get; set; }
        string Name { get; set; }
        double Rate { get; set; }
    }
}
