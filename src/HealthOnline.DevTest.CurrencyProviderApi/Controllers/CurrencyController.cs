using HealthOnline.DevTest.CurrencyProviderApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HealthOnline.DevTest.CurrencyProviderApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private static readonly Random RandomNumberGenerator = new Random();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<CurrencyModel>> Get()
        {
            return new CurrencyModel[]
            {
                new CurrencyModel {Iso = "EUR", Name = "Euro", Rate = RandomNumberBetween(1.10327, 1.17025)},
                new CurrencyModel {Iso = "GBP", Name = "British Pound", Rate = RandomNumberBetween(0.85452, 0.90639)},
                new CurrencyModel {Iso = "USD", Name = "United States Dollar", Rate = RandomNumberBetween(1.24926, 1.33172)},
                new CurrencyModel {Iso = "JPY", Name = "Japanese Yen", Rate = RandomNumberBetween(134.485, 148.176)},
                new CurrencyModel {Iso = "INR", Name = "Indian Rupee", Rate = RandomNumberBetween(87.7147, 148.176)}
            };
        }

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = RandomNumberGenerator.NextDouble();

            return Math.Round(minValue + (next * (maxValue - minValue)), 6);
        }
    }
}