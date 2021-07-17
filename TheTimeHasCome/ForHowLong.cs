using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace TheTimeHasCome
{
    public static class ForHowLong
    {
        private const int ThresholdInSeconds = 15;

        private const int CorrectionInSeconds = 5;

        [FunctionName("for-how-long")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var intervalInMinute = name?.ToLowerInvariant() switch
            {
                "living" => 10,
                "office" => 10,
                "bathroom" => 20,
                "bedroom" => 10,
                "ala" => 15,
                _ => 15
            };

            var now = DateTimeOffset.UtcNow;
            var theTimeWillComeAt = new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, 0, 0, now.Offset);

            while (theTimeWillComeAt < now.AddSeconds(ThresholdInSeconds))
            {
                theTimeWillComeAt = theTimeWillComeAt.AddMinutes(intervalInMinute);
            }

            return new OkObjectResult(theTimeWillComeAt.ToUnixTimeSeconds() - now.ToUnixTimeSeconds() + CorrectionInSeconds);
        }
    }
}