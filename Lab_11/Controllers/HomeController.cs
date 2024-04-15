using Lab_11.Filters;
using Lab_11.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_11.Controllers
{
    [ServiceFilter(typeof(UniqueUserFilter))]
    [ServiceFilter(typeof(LoggingFilter))]
    public class HomeController : Controller
    {
        private readonly string _logFilePath;
        public HomeController()
        {
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log.txt");
        }

        public IActionResult Index()
        {
            ViewBag.UniqueUsersCount = GetUniqueUsersCount(); 
            ViewBag.LogEntries = GetActionLogs(); 
            return View();
        }
        private List<ActionLog> GetActionLogs()
        {
            var logs = new List<ActionLog>();

            try
            {
                using (var fileStream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var parts = line.Split('-');
                        var log = new ActionLog
                        {
                            TimeStamp = DateTime.Parse(parts[0].Trim()),
                            MethodName = parts[1].Trim()
                        };
                        logs.Add(log);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading log file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return logs;
        }

        private int GetUniqueUsersCount()
        {
            HashSet<string> uniqueUsers = new HashSet<string>();

            if (System.IO.File.Exists(_logFilePath))
            {
                using (var fileStream = new FileStream(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var parts = line.Split('-');
                        if (parts.Length >= 3)
                        {
                            uniqueUsers.Add(parts[2].Trim()); 
                        }
                    }
                }
            }
            return uniqueUsers.Count;
        }
    }
}
