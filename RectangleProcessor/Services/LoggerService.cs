namespace RectangleProcessor.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly bool _logToFile;
        private readonly string _logFilePath;

        public LoggerService(bool logToFile = false, string logFilePath = "log.txt")
        {
            _logToFile = logToFile;
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            if (_logToFile)
            {
                File.AppendAllText(_logFilePath, message + Environment.NewLine);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
