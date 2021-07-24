using System;

namespace Dws.Challenge.Infrastructure.Logging.Interfaces
{
    public interface ILoggerWrapper<T>
    {
        void LogInfo(string info);

        void LogError(string errMessage, Exception exception = null);
    }
}