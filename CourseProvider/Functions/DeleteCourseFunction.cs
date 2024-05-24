using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CourseProvider.Functions
{
    public class DeleteCourseFunction(ILogger<DeleteCourseFunction> logger, IGraphQLRequestExecutor executor)
    {
        private readonly ILogger<DeleteCourseFunction> _logger = logger;
        private readonly IGraphQLRequestExecutor _executor = executor;

        [Function("DeleteCourse")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "delete-course/{id}")] HttpRequest req, string id)
        {
            return await _executor.ExecuteAsync(req);
        }
    }
}
