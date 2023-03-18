namespace AstolfoBot
{
    public static class Logger
    {
        private static string FileLogBuffer = "";
        public static LogSeverity LogLevel = LogSeverity.Info;
        public static void Log(string message, object source, LogSeverity severity = LogSeverity.Info, Exception? exception = null)
        {
            if (severity < LogLevel)
                return;
            Console.ForegroundColor = severity switch
            {
                LogSeverity.Debug => ConsoleColor.DarkGray,
                LogSeverity.Verbose => ConsoleColor.Gray,
                LogSeverity.Info => ConsoleColor.White,
                LogSeverity.Warning => ConsoleColor.Yellow,
                LogSeverity.Error => ConsoleColor.Red,
                LogSeverity.Critical => ConsoleColor.DarkRed,
                _ => ConsoleColor.White
            };

            if (source.GetType() == typeof(Type))
                source = ((Type)source).Name;
            else if (source.GetType() == typeof(string))
                source = (string)source;
            else
                source = source.GetType().Name;

            string logmsg = $"{DateTime.Now} [{severity}]{new string(' ', 9 - severity.ToString().Length)}[{source}] {message}" +
                $"{(exception == null ? "" : " \t" + exception + "\n" + exception.StackTrace)}";
            Console.WriteLine(logmsg);
            try
            {
                var latestLog = new FileInfo("Logs/latest.log");
                if (latestLog.Exists && latestLog.Length > 1000000)
                {
                    latestLog.MoveTo($"Logs/{latestLog.LastWriteTime:yyyy-MM-dd_HH-mm-ss}.log");
                }
                File.AppendAllText("Logs/latest.log", FileLogBuffer + logmsg + "\n");
                FileLogBuffer = "";
            }
            catch (Exception e)
            {
                FileLogBuffer += logmsg + "\n";
                Console.WriteLine(e.Message);
            }

            Console.ResetColor();
        }
        public static Task Log(Discord.LogMessage logMessage)
        {
            Log(
                logMessage.Message,
                logMessage.Source,
                logMessage.Severity switch
                {
                    Discord.LogSeverity.Debug => LogSeverity.Debug,
                    Discord.LogSeverity.Verbose => LogSeverity.Verbose,
                    Discord.LogSeverity.Info => LogSeverity.Info,
                    Discord.LogSeverity.Warning => LogSeverity.Warning,
                    Discord.LogSeverity.Error => LogSeverity.Error,
                    Discord.LogSeverity.Critical => LogSeverity.Critical,
                    _ => LogSeverity.Info
                },
                logMessage.Exception
                );
            return Task.CompletedTask;
        }
        public static void Debug(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Debug, exception);
        }
        public static void Verbose(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Verbose, exception);
        }
        public static void Info(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Info, exception);
        }
        public static void Warning(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Warning, exception);
        }
        public static void Error(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Error, exception);
        }
        public static void Critical(string message, object source, Exception? exception = null)
        {
            Log(message, source, LogSeverity.Critical, exception);
        }
        public enum LogSeverity
        {
            Debug,
            Verbose,
            Info,
            Warning,
            Error,
            Critical
        }
    }
}