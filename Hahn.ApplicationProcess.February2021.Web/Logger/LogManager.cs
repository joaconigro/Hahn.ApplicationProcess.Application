using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace Hahn.Web.Log
{
    public class LogManager : ILogManager
    {
        private static Logger _logger;

        public static void Init(Logger logger)
        {
            _logger = logger;
        }

        public LogManager() : base()
        {
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogError(Exception ex)
        {
            LogError(ex.Message);
            LogError(ex.StackTrace);
        }

        public void LogInfo(string message)
        {
            _logger.Information(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warning(message);
        }

        public void Log(string message, TraceEventType severity)
        {
            switch (severity)
            {
                case TraceEventType.Information:
                    LogInfo(message);
                    break;
                case TraceEventType.Warning:
                    LogWarn(message);
                    break;
                case TraceEventType.Error:
                case TraceEventType.Critical:
                    LogError(message);
                    break;
                default:
                    LogDebug(message);
                    break;
            }
        }


        public static void LogException(Exception ex)
        {
            _logger.Error(ex.Message);
            _logger.Error(ex.StackTrace);
        }

        public static void LogInformation(string message)
        {
            _logger.Information(message);
        }

        public static void CloseAndFlush()
        {
            _logger.Dispose();
        }
    }
}
