﻿using Entities.Models;

namespace HealthOnline.DevTest.AnotherCurrencyProviderApi.Models
{
    public class ExchangeModel : IRateModel
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
    }
}