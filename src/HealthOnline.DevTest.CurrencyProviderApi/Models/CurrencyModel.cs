using Entities.Models;

namespace HealthOnline.DevTest.CurrencyProviderApi.Models
{
    public class CurrencyModel : IRateModel
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
}