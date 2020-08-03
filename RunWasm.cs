using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Wasmtime;

namespace Thinktecture.Samples
{
    public static class RunWasm
    {
        [FunctionName("RunWasm")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using var engine = new Engine();
            using var module = Module.FromTextFile(engine, "gcd.wat");

            using var host = new Host(engine);
            using dynamic instance = host.Instantiate(module);

            return new OkObjectResult(instance.gcd(27, 6));
        }
    }
}
