using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestingApi.MyLogger
{
    public enum LogLevel
    {
        Error,
        Warn,
        Info,
        Debug
    }
    public static class Logger
    {
        readonly static string fileName = "logs\\TestApi.log";
        private const string DIRECTORY_NAME = "logs";

        public static void Init()
        {
            Directory.CreateDirectory(DIRECTORY_NAME);
        }

        public static void WriteMessage(string methodName, string logMessage, LogLevel logLevel)
        {
            string log = $"[{methodName}] | [{logLevel}] | [{DateTime.Now}] | {logMessage}";

            using var fs = File.Open(fileName, FileMode.Append);
            using var sw = new StreamWriter(fs);

            sw.WriteLine(log);
        }
    }
}
