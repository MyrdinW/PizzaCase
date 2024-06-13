using System;
using System.IO;

namespace PizzaCase.Security
{
    public static class SecureLogger
    {
        private static readonly string LogFilePath = "secure_log.txt";

        static SecureLogger()
        {
            // Controleer of het logbestand bestaat, zo niet, maak het aan
            if (!File.Exists(LogFilePath))
            {
                using (var stream = File.Create(LogFilePath)) { }
            }
        }

        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            try
            {
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het schrijven naar logbestand: {ex.Message}");
            }
        }

        public static void LogError(string message, Exception ex)
        {
            string logMessage = $"{DateTime.Now}: ERROR: {message} - Exception: {ex.Message}";
            try
            {
                File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Fout bij het schrijven naar logbestand: {logEx.Message}");
            }
        }
    }
}
