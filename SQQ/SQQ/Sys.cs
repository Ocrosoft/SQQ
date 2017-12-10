using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SQQ
{
    public class Sys
    {
        public static void Reset()
        {
            DispatchEnabled = false;
            SignEnabled = false;
            ContestID = 0;
            ProcessingProblems = false;
            Dispatching = false;
            Checking = false;
            MaxTask = 1;
        }
        /// <summary>
        /// If auto dispatch is enabled.
        /// </summary>
        public static bool DispatchEnabled { get; set; }
        /// <summary>
        /// If allow sender sign to system.
        /// </summary>
        public static bool SignEnabled { get; set; }
        /// <summary>
        /// Which contest system should process.
        /// </summary>
        public static int ContestID { get; set; }
        /// <summary>
        /// If a sender whose sending task is equal to this, should not dispatch new task to him.
        /// </summary>
        public static int MaxTask { get; set; }
        /// <summary>
        /// If system is processing problems.
        /// Should not add new solved problems to datanase if this is true.
        /// </summary>
        public static bool ProcessingProblems { get; set; }
        /// <summary>
        /// If system is dispatching tasks.
        /// Should not dispatch new task to senders if this is true.
        /// </summary>
        public static bool Dispatching { get; set; }
        /// <summary>
        /// If system is checking tasks.
        /// Should not check tasks if this is true.
        /// </summary>
        public static bool Checking { get; set; }
        /// <summary>
        /// Print log.
        /// </summary>
        /// <param name="content">Infomation.</param>
        public static void Log(string content)
        {
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (!File.Exists(logFilePath))
            {
                var file = File.Create(logFilePath);
                file.Close();
            }
            StreamWriter fs = new StreamWriter(logFilePath, true);
            fs.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + content);
            fs.Close();
        }
        /// <summary>
        /// Print error log.
        /// </summary>
        /// <param name="content">Error infomation.</param>
        public static void Error(string content)
        {
            string errFilePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + DateTime.Now.ToString("yyyy-MM-dd") + ".err";
            if (!File.Exists(errFilePath))
            {
                var file = File.Create(errFilePath);
                file.Close();
            }
            StreamWriter fs = new StreamWriter(errFilePath, true);
            fs.WriteLine(DateTime.Now.ToString("HH:mm:ss ") + content);
            fs.Close();
        }
        /// <summary>
        /// Clear today's log.
        /// </summary>
        public static void ClearLogs()
        {
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string errFilePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + DateTime.Now.ToString("yyyy-MM-dd") + ".err";
            if (!File.Exists(logFilePath))
            {
                var file = File.Create(logFilePath);
                file.Close();
            }
            if (!File.Exists(errFilePath))
            {
                var file = File.Create(errFilePath);
                file.Close();
            }
            StreamWriter fs = new StreamWriter(logFilePath, false);
            fs.WriteLine("");
            fs.Close();
            fs = new StreamWriter(errFilePath, false);
            fs.WriteLine("");
            fs.Close();
        }
        /// <summary>
        /// Get today's log.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLog()
        {
            List<string> ret = new List<string>();
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + '\\' + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            if (!File.Exists(logFilePath))
            {
                return ret;
            }
            StreamReader sr = new StreamReader(logFilePath, Encoding.UTF8);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                ret.Add(line);
            }
            sr.Close();
            return ret;
        }
    }
}