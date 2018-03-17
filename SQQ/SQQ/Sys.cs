using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SQQ
{
    public class Sys
    {
        static Sys()
        {
            var settings = Database.GetSettings();
            DispatchEnabled = settings.dispatchEnabled;
            SignEnabled = settings.signEnabled;
            ContestID = settings.contestId;
            ProcessingProblems = settings.processingProblems;
            Dispatching = settings.dispatching;
            Checking = settings.checking;
            MaxTask = settings.maxTask;
            floorList = Database.GetsFloor();
            var colorList = Database.GetsColor();
            foreach(var color in colorList)
            {
                colorMap.Add(color.num, color.color);
            }
        }
        public static void Reset()
        {
            Database.SetSettings(new Database.Settings());
            var settings = Database.GetSettings();
            DispatchEnabled = settings.dispatchEnabled;
            SignEnabled = settings.signEnabled;
            ContestID = settings.contestId;
            ProcessingProblems = settings.processingProblems;
            Dispatching = settings.dispatching;
            Checking = settings.checking;
            MaxTask = settings.maxTask;
            floorList = Database.GetsFloor();
            colorMap.Clear();
        }

        public static void SaveSettings()
        {
            Database.SetSettings(new Database.Settings()
            {
                dispatchEnabled = DispatchEnabled,
                signEnabled = SignEnabled,
                contestId = ContestID,
                maxTask = MaxTask
            });
        }
        /// <summary>
        /// 分配是否开启
        /// </summary>
        public static bool DispatchEnabled { get; set; }
        /// <summary>
        /// 注册是否开启
        /// </summary>
        public static bool SignEnabled { get; set; }
        /// <summary>
        /// 比赛ID
        /// </summary>
        public static int ContestID { get; set; }
        /// <summary>
        /// 最大配送中数量
        /// </summary>
        public static int MaxTask { get; set; }
        /// <summary>
        /// 是否正在处理题目
        /// Should not add new solved problems to datanase if this is true.
        /// </summary>
        public static bool ProcessingProblems { get; set; }
        /// <summary>
        /// 是否正在进行分配任务
        /// </summary>
        public static bool Dispatching { get; set; }
        /// <summary>
        /// 是否正在验证配送任务超时
        /// </summary>
        public static bool Checking { get; set; }
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="content">日志内容</param>
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
        /// 打印错误日志
        /// </summary>
        /// <param name="content">错误信息</param>
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
        /// 清除所有日志
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
        /// 获取今天的日志
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
        /// <summary>
        /// 楼层/考场列表
        /// </summary>
        public static List<Database.Floor> floorList;
        /// <summary>
        /// 气球颜色表
        /// </summary>
        public static Hashtable colorMap = new Hashtable();
    }
}