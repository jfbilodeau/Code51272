// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DemoApp
{
    public class GenerateThumbNail
    {
        private readonly ILogger _logger;

        public GenerateThumbNail(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GenerateThumbNail>();
        }

        [Function("GenerateThumbNail")]
        public void Run([EventGridTrigger] MyEvent input)
        {
            _logger.LogInformation(input.Data.ToString());
        }
    }

    public class MyEvent
    {
        public string Id { get; set; }

        public string Topic { get; set; }

        public string Subject { get; set; }

        public string EventType { get; set; }

        public DateTime EventTime { get; set; }

        public object Data { get; set; }
    }
}
