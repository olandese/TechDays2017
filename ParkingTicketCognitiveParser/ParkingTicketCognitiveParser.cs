using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Text;

namespace ParkingTicketCognitiveParser
{
    public static class ParkingTicketCognitiveParser
    {
        [FunctionName("ParkingTicketCognitiveParser")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("nl-NL");

            HttpResponseMessage resp = new HttpResponseMessage();
            // Get request body
            dynamic data = await req.Content.ReadAsStringAsync();

            CognitiveModel mod = JsonConvert.DeserializeObject<CognitiveModel>((string) data);

            var allTexts = mod.regions.SelectMany(x => x.lines)
                                      .SelectMany(y => y.words)
                                      .Select(t => t.text)
                                      .ToList();

            var currencyText = allTexts.Where(c => c.Contains("€")).ToList();

            if (!currencyText.Any())
            {
                resp.StatusCode = HttpStatusCode.NotFound;
                resp.Content = new StringContent("No Currency found");
                return resp;
            }

            decimal decimalResult;
            var highestAmount = currencyText.Where(c => decimal.TryParse(c, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimalResult)).Select(c => decimal.Parse(c, NumberStyles.Currency)).Max();
            
            DateTime result;

            string format = @"dd/MM/yyyy";

            DateTime date = allTexts
                            .Where(d => DateTime.TryParseExact(d, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                            .Select(DateTime.Parse)
                            .FirstOrDefault();

            if (date == default(DateTime))
            {
                resp.StatusCode = HttpStatusCode.NotFound;
                resp.Content = new StringContent("No date found");
                return resp;
            }
            
            ResponseModel responseModel = new ResponseModel
            {
                Amount = $"{highestAmount:0.00}",
                Date = date.ToShortDateString()
            };

            var json = JsonConvert.SerializeObject(responseModel, Formatting.Indented);
            
            resp.StatusCode = HttpStatusCode.OK;
            resp.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return resp;
        }
    }
}
