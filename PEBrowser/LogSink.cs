using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEBrowser
{
    internal class LogSink
    {
        public LogSink() { }

        private static readonly object s_instanceLock = new object();
        private static LogSink s_instance;

        public LogSink GlobalLogSink {
            get {
                lock (s_instanceLock) {
                    return s_instance ?? (s_instance = new LogSink());
                }
                
            }
        }

        public void LogLine(string line) {
            var e = LineLogged;
            if (e == null) return;

            e(null, new LogLineEventArgs(line));
        }
        
        public void LogLineFormat(string format, params object[] args) {
            LogLine(String.Format(format, args));
        }

        public event EventHandler<LogLineEventArgs> LineLogged;
    }

    internal class LogLineEventArgs : EventArgs
    {
        public string Line { get; private set; }
        public LogLineEventArgs(string line) {
            Line = line;
        }
    }
}
