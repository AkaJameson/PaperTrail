namespace PaperTrail.Api.Filter
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using Si.CoreHub.Logs;

    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogHub _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionFilter(ILogHub logHub, IHostEnvironment env)
        {
            _logger = logHub;
            _env = env;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 记录错误日志
            _logger.Error(context.Exception+context.Exception.StackTrace);

            // 生产环境返回简化信息，开发环境返回详细信息
            object error = _env.IsDevelopment()
                ? new
                {
                    Message = context.Exception.Message,
                    StackTrace = context.Exception.StackTrace
                }
                : new { Message = "An error occurred. Please try again later." };

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
