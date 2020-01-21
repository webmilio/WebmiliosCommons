﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Webmilio.Commons.Logging
{
    public class Logger
    {
        private static readonly ConcurrentDictionary<string, Logger> _loggers = new ConcurrentDictionary<string, Logger>(new LoggerNameEqualityComparer());


        protected Logger(string loggerName, bool toSystemDiagnostics, bool toConsole)
        {
            LoggerName = loggerName;

            ToSystemDiagnostics = toSystemDiagnostics;
            ToConsole = toConsole;
        }


        public virtual void Log(LogLevel level, string message)
        {
            string formattedLog = $"{DateTime.Now.ToLongTimeString()} [{level.ToString()}] : {message}";

            if (ToSystemDiagnostics)
                System.Diagnostics.Debug.WriteLine(formattedLog);

            if (ToConsole)
                Console.WriteLine(message);
        }


        public virtual void Fatal(string message) => Log(LogLevel.Fatal, message);

        public virtual void Severe(string message) => Log(LogLevel.Severe, message);

        public virtual void Warning(string message) => Log(LogLevel.Warning, message);

        public virtual void Log(string message) => Log(LogLevel.Log, message);

        public virtual void Info(string message) => Log(LogLevel.Info, message);

        public virtual void Verbose(string message) => Log(LogLevel.Verbose, message);


        public string LoggerName { get; }

        public bool ToSystemDiagnostics { get; }
        public bool ToConsole { get; }


        #region Static

        public static Logger Get<T>(bool toSystemDiagnostics = true, bool toConsole = true) => Get(typeof(T).FullName, toSystemDiagnostics, toConsole);

        public static Logger Get(string loggerName, bool toSystemDiagnostics = true, bool toConsole = true)
        {
            if (!_loggers.ContainsKey(loggerName))
                _loggers.TryAdd(loggerName, new Logger(loggerName, toSystemDiagnostics, toConsole));

            if (!_loggers.TryGetValue(loggerName, out Logger logger))
                Get(loggerName);

            logger.Log($"Logger created under name {logger.LoggerName}.");

            return logger;
        }


        private class LoggerNameEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y) => x.Equals(y, StringComparison.CurrentCultureIgnoreCase);

            public int GetHashCode(string obj) => obj.GetHashCode();
        }

        #endregion
    }
}