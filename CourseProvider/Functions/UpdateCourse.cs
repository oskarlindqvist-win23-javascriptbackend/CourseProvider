using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CourseProvider.Functions
{
    public class UpdateCourse(ILogger<UpdateCourse> logger, IGraphQLRequestExecutor executor)
    {
        private readonly ILogger<UpdateCourse> _logger = logger;
        private readonly IGraphQLRequestExecutor _executor = executor;

        [Function("UpdateCourse")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "update-course")] HttpRequest req)
        {
            return await _executor.ExecuteAsync(req);
        }
    }
}
