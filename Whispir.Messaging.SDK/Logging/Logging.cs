using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whispir.Messaging.SDK
{
    public class Logging
    {
        private static Logging _instance = null;

        private static Object _mutex = new Object();

        private static bool isLogging=false;
        private Logging(string PathToLog)
        {
            if (!String.IsNullOrEmpty(PathToLog))
            {
                isLogging = true;
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.LiterateConsole()
                    .WriteTo.RollingFile(PathToLog + "\\whispir.Messaging-{Date}.log")
                    .CreateLogger();
            }
        }

        public void LogInfo(string Info)
        {
            if(isLogging)
            {
                Log.Information(Info);
            }
        }
        public static Logging GetInstance(string PathToLog)
        {
            if (_instance == null)
            {
                lock (_mutex) // now I can claim some form of thread safety...
                {
                    if (_instance == null)
                    {
                        _instance = new Logging(PathToLog);
                    }
                }
            }

            return _instance;
        }
    }
}
