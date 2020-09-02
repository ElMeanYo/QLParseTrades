using System;
using System.Collections.Generic;
using System.Text;

namespace QLParseTrades.Main.Logging
{
    public enum LogEventLevel
    {
        Fatal = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 5
    }

    /// <summary>
    /// For event logging.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        public string Message;
        public Exception Ex;
        public LogEventLevel Level;

        public LogEventArgs(string msg, LogEventLevel level, Exception ex = null)
        {
            Message = msg;
            Ex = ex;
            if (ex != null)
                Message += $@"
Details: {ex.Message}";
            Level = level;
        }
    }
}
