using Microsoft.AspNetCore.Mvc.Filters;

namespace Lab_11.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly string _logFilePath;

        public LoggingFilter(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            
            var logMessage = $"{DateTime.Now} - '{actionName}' спрацював";
            
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            
            var logMessage = $"{DateTime.Now} - '{actionName}' завершив";
            
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}