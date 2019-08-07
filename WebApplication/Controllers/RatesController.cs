using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class RatesController : Controller
    {
        // GET: Rates
        public ActionResult Index(IndexViewModel viewModel)
        {
            Dictionary<int, ApiModel> apis = GetApiConnections();

            viewModel.ApiDropdown = SetUpApiDropDownList(apis);

            ApiModel apiUsed = GetApiDetails(viewModel, apis);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUsed.Api);

                var responseTask = client.GetAsync(apiUsed.Task);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RateModel>>();
                    readTask.Wait();

                    viewModel.Rates = readTask.Result.ToList();
                }
                else
                {
                    viewModel = new IndexViewModel();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            return View(viewModel);
        }

        public ActionResult AverageConversions(AverageConversionModel viewModel)
        {
            List<AverageConversionModel> results = new List<AverageConversionModel>();

            Dictionary<int, ApiModel> apis = GetApiConnections();

            List<RateModel> rates = new List<RateModel>();

            foreach (var item in apis)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(item.Value.Api);

                    var responseTask = client.GetAsync(item.Value.Task);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<RateModel>>();
                        readTask.Wait();

                        var rateResults = readTask.Result.ToList();

                        if (rates.Count == 0)
                        {
                            rates.AddRange(rateResults);
                        }
                        else
                        {
                            foreach (var rateResult in rateResults)
                            {
                                var obj = rates.FirstOrDefault(x => x.Iso == rateResult.Iso);
                                if (obj != null) obj.Rate = obj.Rate + rateResult.Rate;
                            }

                        }
                    }
                    else
                    {
                        viewModel = new AverageConversionModel();
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }

            foreach (var rate in rates)
            {
                AverageConversionModel averageConversionModel = new AverageConversionModel();

                var obj = rates.FirstOrDefault(x => x.Iso == rate.Iso);

                if (obj != null)
                {
                    averageConversionModel.Iso = obj.Iso;
                    averageConversionModel.Name = obj.Name;
                    averageConversionModel.Rate = obj.Rate / apis.Count;                    
                }

                results.Add(averageConversionModel);
            }            

            return View(results.ToList());
        }

        private IEnumerable<SelectListItem> SetUpApiDropDownList(Dictionary<int, ApiModel> apis)
        {
            List<SelectListItem> apiList = new List<SelectListItem>();

            foreach (var x in apis)
            {
                var apiItem = new SelectListItem
                {
                    Value = x.Key.ToString(),
                    Text = x.Value.Api
                };

                apiList.Add(apiItem);
            }

            return apiList;
        }

        private ApiModel GetApiDetails(IndexViewModel viewModel, Dictionary<int, ApiModel> apis)
        {
            ApiModel model = new ApiModel();

            if (viewModel.ApiId == null)
            {
                model.Api = apis[1].Api;
                model.Task = apis[1].Task;
            }
            else
            {
                model.Api = apis.FirstOrDefault(x => x.Key == viewModel.ApiId).Value.Api;
                model.Task = apis.FirstOrDefault(x => x.Key == viewModel.ApiId).Value.Task;
            }

            return model;
        }

        public ActionResult Convert(ConvertModel viewModel)
        {
            Dictionary<int, ApiModel> apis = GetApiConnections();

            viewModel.ApiDropdown = SetUpApiDropDownList(apis);

            ApiModel apiUsed = GetApiDetailsForConvert(viewModel, apis);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUsed.Api);

                var responseTask = client.GetAsync(apiUsed.Task);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<RateModel>>();
                    readTask.Wait();

                    ModelState.Clear();

                    viewModel.Rates = readTask.Result.ToList();
                    viewModel.RatesFromDropdown = SetupDropDownList(viewModel.Rates);
                    viewModel.RatesToDropdown = SetupDropDownList(viewModel.Rates);
                }
                else
                {
                    viewModel = new ConvertModel();
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            viewModel.Output = CalculatedOutput(viewModel);

            return View(viewModel);
        }

        private ApiModel GetApiDetailsForConvert(ConvertModel viewModel, Dictionary<int, ApiModel> apis)
        {
            ApiModel model = new ApiModel();

            if (viewModel.ApiId == null)
            {
                model.Api = apis[1].Api;
                model.Task = apis[1].Task;
            }
            else
            {
                model.Api = apis.FirstOrDefault(x => x.Key == viewModel.ApiId).Value.Api;
                model.Task = apis.FirstOrDefault(x => x.Key == viewModel.ApiId).Value.Task;
            }

            return model;
        }

        private Dictionary<int, ApiModel> GetApiConnections()
        {
            Dictionary<int, ApiModel> apis = new Dictionary<int, ApiModel>();
            apis.Add(1, new ApiModel { Api = "https://localhost:44382/api/", Task = "V1/Exchange" });
            apis.Add(2, new ApiModel { Api = "https://localhost:44374/api/", Task = "V1/Currency" });

            return apis;
        }

        private IEnumerable<SelectListItem> SetupDropDownList(List<RateModel> rates)
        {
            List<SelectListItem> apiList = new List<SelectListItem>();

            if (rates.Count > 0)
            {
                foreach (var x in rates)
                {
                    var apiItem = new SelectListItem
                    {
                        Value = x.Iso,
                        Text = x.Iso
                    };

                    apiList.Add(apiItem);
                }
            }

            return apiList;
        }

        private double CalculatedOutput(ConvertModel viewModel)
        {
            double output = 0;

            if (viewModel.RateFromId == viewModel.RatesToId)
            {
                return viewModel.Input;
            }

            var rate = viewModel.Rates.FirstOrDefault(x => x.Iso == viewModel.RatesToId).Rate;
            output = viewModel.Input / rate;

            return output;
        }
    }
}