using System;
using Dws.Challenge.Infrastructure.Logging.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dws.Challenge.Infrastructure.Logging
{
    public class LoggerWrapper<T> : ILoggerWrapper<T>
    {
        private readonly ILogger<T> logger;

        public LoggerWrapper(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogError(string errMessage, Exception exception = null)
        {
            if (exception != null)
            {
                this.logger.LogError(exception, errMessage);
            }
            else
            {
                this.logger.LogError(errMessage);
            }
        }

        public void LogInfo(string info)
        {
            this.logger.LogError(info);
        }
    }
}