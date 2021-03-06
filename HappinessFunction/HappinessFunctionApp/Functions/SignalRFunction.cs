using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HappinessFunctionApp.Functions
{
    public static class SignalRFunction
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalRConnectionInfo(HubName = "chat")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("SendMessage")]
        public static async Task SendMessage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalR(HubName = "chat")]IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            
            var data = await req.ReadAsStringAsync();

            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = "newMessage",
                Arguments = new[] { data }
            });
        }
    }
}
