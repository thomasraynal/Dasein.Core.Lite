using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dasein.Core.Lite.Shared
{
    public static class ICanLogEx
    {
        private static ILogger _logger;

        static ICanLogEx()
        {
            _logger = AppCore.Instance.Get<ILogger>();
        }

        public static ILogger Logger
        {
            get { return _logger; }
        }

        public static void LogCritical(this ICanLog @this, string message)
        {
            Logger.LogCritical(message);
        }

        public static void LogError(this ICanLog @this, string message)
        {
            Logger.LogError(message);
        }

        public static void LogWarning(this ICanLog @this, string message)
        {
            Logger.LogWarning(message);
        }

        public static void LogInformation(this ICanLog @this, string message)
        {
            Logger.LogInformation(message);
        }

        public static void LogCritical(this ICanLog @this, Exception ex)
        {
            Logger.LogCritical(ex, string.Empty);
        }

        public static void LogError(this ICanLog @this, Exception ex)
        {
            Logger.LogError(ex, string.Empty);
        }

        public static void LogWarning(this ICanLog @this, Exception ex)
        {
            Logger.LogWarning(ex.ToString());
        }

        public static void LogInformation(this ICanLog @this, Exception ex)
        {
            Logger.LogInformation(ex.ToString());
        }

        public static void LogCritical(this ICanLog @this, string message, Exception ex)
        {
            Logger.LogCritical(ex, message);
        }

        public static void LogError(this ICanLog @this, string message, Exception ex)
        {
            Logger.LogError(ex, message);
        }

        public static void LogWarning(this ICanLog @this, string message, Exception ex)
        {
            Logger.LogWarning(ex, message);
        }

        public static void LogInformation(this ICanLog @this, string message, Exception ex)
        {
            Logger.LogInformation(ex, message);
        }

    }

    public interface ICanLog
    {
    }
}
