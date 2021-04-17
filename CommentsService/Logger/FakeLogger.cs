using LoggingClassLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace TheSocialBaz.FakeLoggerService
{
    public class FakeLogger : Logger
    {
        public FakeLogger(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Log(LogLevel logLevel, string requestId, string previousRequestId, string message, Exception exception)
        {
            Thread.Sleep(500);
        }
    }
}
