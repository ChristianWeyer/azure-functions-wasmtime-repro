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
        [FunctionName("FibonacciWasm")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "fibonacci/{number}")]
            HttpRequest request,
            int number,
            ILogger log)
        {
            using var engine = new Engine();
            using var module = Module.FromFile(engine, "fibonacci.wasm");

            using var host = new Host(engine);
            using dynamic instance = host.Instantiate(module);

            return new OkObjectResult(instance.fib(number));
        }
    }
}
