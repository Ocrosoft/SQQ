using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace SQQ
{
    public class Database
    {
        public class Sender
        {
            public Sender()
            {
                name = string.Empty;
                open_id = string.Empty;
                sent = 0;
                sending = 0;
                timeSpent = 0;
            }
            /// <summary>
            /// 注册时填写的姓名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 微信OPENID
            /// </summary>
            public string open_id { get; set; }
            /// <summary>
            /// 已配送气球数量
            /// </summary>
            public int sent { get; set; }
            /// <summary>
            /// 正在配送的气球数量
            /// </summary>
            public int sending { get; set; }
            /// <summary>
            /// 花费的总时间，单位秒
            /// </summary>
            public double timeSpent { get; set; }
        }
        public class ProblemSolved : IEqualityComparer<ProblemSolved>
        {
            public ProblemSolved()
            {
                team_id = string.Empty;
                num = -1;
                sender = null;
                timeStart = DateTime.Now;
                timeSent = DateTime.Now;
            }
            /// <summary>
            /// 队伍ID
            /// </summary>
            public string team_id { get; set; }
            /// <summary>
            /// 题目ID，从0开始
            /// </summary>
            public int num { get; set; }
            /// <summary>
            /// 指定配送员的OPENID
            /// </summary>
            public string sender { get; set; }
            /// <summary>
            /// 开始配送时间
            /// </summary>
            public DateTime timeStart { get; set; }
            /// <summary>
            /// 确认送达的时间
            /// </summary>
            public DateTime timeSent { get; set; }
            /// <summary>
            /// 气球要送去的楼层，未设置时为空
            /// </summary>
            public string floor { get; set; }
            /// <summary>
            /// 气球颜色，与题目ID对应，未设置时为空
            /// </summary>
            public string color { get; set; }

            public bool Equals(ProblemSolved x, ProblemSolved y)
            {
                if (x.team_id == y.team_id && x.num == y.num)
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(ProblemSolved obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                return obj.ToString().GetHashCode();
            }
        }
        public class Settings
        {
            public Settings()
            {
                dispatchEnabled = false;
                signEnabled = false;
                contestId = 0;
                processingProblems = false;
                dispatching = false;
                checking = false;
                maxTask = 1;
            }
            public bool dispatchEnabled { get; set; }
            public bool signEnabled { get; set; }
            public int contestId { get; set; }
            public bool processingProblems { get; set; }
            public bool dispatching { get; set; }
            public bool checking { get; set; }
            public int maxTask { get; set; }
        }
        public class Floor
        {
            public string teamId { get; set; }
            public string floor { get; set; }
        }
        public class Color
        {
            public int num { get; set; }
            public string color { get; set; }
        }

        /* 题目 */
        /// <summary>
        /// 获取所有AC的题目
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSolved()
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql =
                "SELECT team_id, num, NULL FROM problem_saved " +
                "UNION " +
                "SELECT team_id, num, sender FROM sending " +
                "UNION " +
                "SELECT team_id, num, sender FROM sent";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString())
                };
                if (!string.IsNullOrEmpty(row.ItemArray[2].ToString()))
                {
                    ps.sender = row.ItemArray[2].ToString();
                }
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null) 
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }
                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取所有AC但没有分配的题目
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSaved()
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num FROM problem_saved";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString())
                };
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null)
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }
                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取所有AC且气球已经送达的题目
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSent()
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num, sender, timeStart, timeSent FROM sent";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    timeStart = DateTime.Parse(row.ItemArray[3].ToString()),
                    timeSent = DateTime.Parse(row.ItemArray[4].ToString())
                };
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null)
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }
                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取对应OPENID配送员配送完成的题目
        /// </summary>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSent(string open_id)
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();
            string sql = "SELECT team_id, num, sender, timeStart, timeSent FROM sent WHERE sender = ?open_id ORDER BY timeSent DESC";

            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    timeStart = DateTime.Parse(row.ItemArray[3].ToString()),
                    timeSent = DateTime.Parse(row.ItemArray[4].ToString())
                };
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null)
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }
                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取team_id队伍过的第num+1题配送完成的记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static ProblemSolved GetProblemSent(string team_id, int num)
        {
            string sql = "SELECT team_id, num, sender, timeStart, timeSent FROM sent WHERE  team_id = ?team_id AND num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            var ds = MySQLHelper.ExecuteDataSet(sql, para);
            DataRow row = ds.Tables[0].Rows[0];
            var ps = new ProblemSolved()
            {
                team_id = row.ItemArray[0].ToString(),
                num = int.Parse(row.ItemArray[1].ToString()),
                sender = row.ItemArray[2].ToString(),
                timeStart = DateTime.Parse(row.ItemArray[3].ToString()),
                timeSent = DateTime.Parse(row.ItemArray[4].ToString())
            };
            foreach (Floor floor in Sys.floorList)
            {
                if (Regex.IsMatch(ps.team_id, floor.teamId))
                {
                    ps.floor = floor.floor;
                    break;
                }
            }
            if (Sys.colorMap[ps.num] != null)
            {
                ps.color = (string)Sys.colorMap[ps.num];
            }

            return ps;
        }
        /// <summary>
        /// 获取所有正在配送气球的题目
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSending()
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num, sender, timeStart FROM sending";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    timeStart = DateTime.Parse(row.ItemArray[3].ToString())
                };
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null)
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }

                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取对应OPENID配送员正在配送的题目
        /// </summary>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSending(string open_id)
        {
            List<ProblemSolved> ret = new List<ProblemSolved>();
            string sql = "SELECT team_id, num, sender, timeStart FROM sending WHERE sender = ?open_id";

            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                foreach (Floor floor in Sys.floorList)
                {

                }
                var ps = new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    timeStart = DateTime.Parse(row.ItemArray[3].ToString()),
                    color = "",
                    floor = ""
                };
                foreach (Floor floor in Sys.floorList)
                {
                    if (Regex.IsMatch(ps.team_id, floor.teamId))
                    {
                        ps.floor = floor.floor;
                        break;
                    }
                }
                if (Sys.colorMap[ps.num] != null)
                {
                    ps.color = (string)Sys.colorMap[ps.num];
                }
                ret.Add(ps);
            }

            return ret;
        }
        /// <summary>
        /// 获取team_id队伍过的第num+1题正在配送的记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static ProblemSolved GetProblemSending(string team_id, int num)
        {
            string sql = "SELECT team_id, num, sender, timeStart FROM sending WHERE team_id = ?team_id AND num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            var ds = MySQLHelper.ExecuteDataSet(sql, para);
            DataRow row = ds.Tables[0].Rows[0];
            var ps =  new ProblemSolved()
            {
                team_id = row.ItemArray[0].ToString(),
                num = int.Parse(row.ItemArray[1].ToString()),
                sender = row.ItemArray[2].ToString(),
                timeStart = DateTime.Parse(row.ItemArray[3].ToString())
            };
            foreach (Floor floor in Sys.floorList)
            {
                if (Regex.IsMatch(ps.team_id, floor.teamId))
                {
                    ps.floor = floor.floor;
                    break;
                }
            }
            if (Sys.colorMap[ps.num] != null)
            {
                ps.color = (string)Sys.colorMap[ps.num];
            }

            return ps;
        }
        /// <summary>
        /// 添加一条待配送记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static bool AddProblemSaved(string team_id, int num)
        {
            string sql = "INSERT INTO problem_saved VALUES(?team_id, ?num)";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除一条待配送记录，通常与AddProblemSending()同时使用
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static bool DeleteProblemSaved(string team_id, int num)
        {
            string sql = "DELETE FROM problem_saved WHERE team_id = ?team_id and num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加一条配送中记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static bool AddProblemSending(string team_id, int num, string open_id)
        {
            string sql = "INSERT INTO sending(team_id, num, sender, timeStart) VALUES(?team_id, ?num, ?sender, now())";
            MySqlParameter[] para = new MySqlParameter[3];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);
            para[2] = new MySqlParameter("?sender", open_id);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除一条配送中记录，通常与AddProblemSent()同时使用
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static bool DeleteProblemSending(string team_id, int num)
        {
            string sql = "DELETE FROM sending WHERE team_id = ?team_id and num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 添加一条已配送记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static bool AddProblemSent(string team_id, int num, string open_id, DateTime timeStart)
        {
            string sql = "INSERT INTO sent VALUES(?team_id, ?num, ?sender, ?timeStart, now())";
            MySqlParameter[] para = new MySqlParameter[4];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);
            para[2] = new MySqlParameter("?sender", open_id);
            para[3] = new MySqlParameter("?timeStart", timeStart.ToString("yyyy-MM-dd hh:mm:ss"));

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除一条配送完成记录
        /// </summary>
        /// <param name="team_id">队伍ID</param>
        /// <param name="num">题目ID</param>
        /// <returns></returns>
        public static bool DeleteProblemSent(string team_id, int num)
        {
            string sql = "DELETE FROM sent WHERE team_id = ?team_id and num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }
            return false;
        }

        /* 配送员 */
        /// <summary>
        /// 获取所有配送员
        /// </summary>
        /// <returns>a list of senders.</returns>
        public static List<Sender> GetsSender()
        {
            List<Sender> ret = new List<Sender>();

            string sql = "SELECT name, open_id FROM sender";
            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var sender = new Sender()
                {
                    name = row.ItemArray[0].ToString(),
                    open_id = row.ItemArray[1].ToString()
                };
                sender.sending =
                    int.Parse(
                        MySQLHelper.ExecuteScalar(
                            "SELECT COUNT(*) FROM sending WHERE sender = ?open_id",
                            new MySqlParameter("?open_id", sender.open_id)
                            ).ToString());
                sender.sent =
                    int.Parse(
                        MySQLHelper.ExecuteScalar(
                            "SELECT COUNT(*) FROM sent WHERE sender = ?open_id",
                            new MySqlParameter("?open_id", sender.open_id)
                            ).ToString());
                ret.Add(sender);
            }

            return ret;
        }
        /// <summary>
        /// 添加一个配送员
        /// </summary>
        /// <param name="open_id">微信OPENID</param>
        /// <param name="name">姓名</param>
        /// <returns></returns>
        public static bool AddSender(string open_id, string name)
        {
            string sql = "INSERT INTO sender(open_id, name) VALUES(?open_id, ?name)";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?open_id", open_id);
            para[1] = new MySqlParameter("?name", name);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 删除一个配送员
        /// </summary>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static bool DeleteSender(string open_id)
        {
            string sql = "DELETE FROM sender WHERE open_id = ?open_id";
            MySqlParameter para = new MySqlParameter("?open_id", open_id);

            if (MySQLHelper.ExecuteNonQuery(sql, para) == 1)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 根据OPENID获取配送员
        /// </summary>
        /// <param name="open_id">微信OPENID</param>
        /// <returns></returns>
        public static Sender GetSender(string open_id)
        {
            string sql = "SELECT * FROM sender WHERE open_id = ?open_id";
            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            return new Sender()
            {
                open_id = ds.Tables[0].Rows[0].ItemArray[0].ToString(),
                name = ds.Tables[0].Rows[0].ItemArray[1].ToString()
            };
        }

        /* system */
        /// <summary>
        /// 初始化，清空数据库
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            string sql_clear_problem_saved = "DELETE FROM problem_saved";
            string sql_clear_sender = "DELETE FROM sender";
            string sql_clear_sending = "DELETE FROM sending";
            string sql_clear_sent = "DELETE FROM sent";
            string sql_clear_colors = "DELETE FROM colors";
            string sql_clear_floor = "DELETE FROM floorinfo";

            return MySQLHelper.ExecuteNoQueryTran(new List<string>
            {
                sql_clear_problem_saved,
                sql_clear_sender,
                sql_clear_sending,
                sql_clear_sent,
                sql_clear_colors,
                sql_clear_floor
            });
        }
        /// <summary>
        /// 获取系统设置
        /// </summary>
        /// <returns></returns>
        public static Settings GetSettings()
        {
            string sql = "SELECT * FROM settings";
            var ds = MySQLHelper.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count == 0)
            {
                SetSettings(new Settings());
                return GetSettings();
            }
            return new Settings()
            {
                dispatchEnabled = ds.Tables[0].Rows[0].ItemArray[0].ToString() == "1",
                signEnabled = ds.Tables[0].Rows[0].ItemArray[1].ToString() == "1",
                contestId = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                processingProblems = ds.Tables[0].Rows[0].ItemArray[3].ToString() == "1",
                dispatching = ds.Tables[0].Rows[0].ItemArray[4].ToString() == "1",
                checking = ds.Tables[0].Rows[0].ItemArray[5].ToString() == "1",
                maxTask = Int32.Parse(ds.Tables[0].Rows[0].ItemArray[6].ToString())
            };
        }
        /// <summary>
        /// 设置系统设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool SetSettings(Settings settings)
        {
            string sql = "DELETE FROM settings";
            MySQLHelper.ExecuteNonQuery(sql);
            sql = "INSERT INTO settings VALUES(?0, ?1, ?2, ?3, ?4, ?5, ?6)";
            MySqlParameter[] para = new MySqlParameter[7];
            para[0] = new MySqlParameter("?0", settings.dispatchEnabled ? 1 : 0);
            para[1] = new MySqlParameter("?1", settings.signEnabled ? 1 : 0);
            para[2] = new MySqlParameter("?2", settings.contestId );
            para[3] = new MySqlParameter("?3", settings.processingProblems ? 1 : 0);
            para[4] = new MySqlParameter("?4", settings.dispatching ? 1 : 0);
            para[5] = new MySqlParameter("?5", settings.checking ? 1 : 0);
            para[6] = new MySqlParameter("?6", settings.maxTask);
            return MySQLHelper.ExecuteNonQuery(sql, para) == 1;
        }
        /// <summary>
        /// 添加楼层/考场
        /// </summary>
        /// <param name="team_id"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public static bool AddFloor(string team_id, string floor)
        {
            string sql = "INSERT INTO floorinfo VALUES(?team_id, ?floor)";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?floor", floor);

            return MySQLHelper.ExecuteNonQuery(sql, para) == 1;
        }
        /// <summary>
        /// 删除楼层/考场
        /// </summary>
        /// <param name="team_id"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public static bool DeleteFloor(string team_id, string floor)
        {
            string sql = "DELETE FROM floorinfo WHERE teamId = ?team_id and floor = ?floor";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?floor", floor);

            return MySQLHelper.ExecuteNonQuery(sql, para) == 1;
        }
        /// <summary>
        /// 获取所有考场/楼层
        /// </summary>
        /// <returns></returns>
        public static List<Floor> GetsFloor()
        {
            List<Floor> ret = new List<Floor>();

            string sql = "SELECT teamId, floor FROM floorinfo";
            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var floor = new Floor()
                {
                    teamId = row.ItemArray[0].ToString(),
                    floor = row.ItemArray[1].ToString()
                };
                ret.Add(floor);
            }

            return ret;
        }
        /// <summary>
        /// 添加气球颜色
        /// </summary>
        /// <param name="num">题目ID</param>
        /// <param name="color">颜色</param>
        /// <returns></returns>
        public static bool AddColor(int num, string color)
        {
            string sql = "INSERT INTO colors VALUES(?num, ?color)";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?num", num);
            para[1] = new MySqlParameter("?color", color);

            return MySQLHelper.ExecuteNonQuery(sql, para) == 1;
        }
        /// <summary>
        /// 删除气球颜色
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool DeleteColor(int num)
        {
            string sql = "DELETE FROM colors WHERE num = ?num";

            return MySQLHelper.ExecuteNonQuery(sql, new MySqlParameter("?num", num)) == 1;
        }
        /// <summary>
        /// 获取所有气球颜色
        /// </summary>
        /// <returns></returns>
        public static List<Color> GetsColor ()
        {
            List<Color> ret = new List<Color>();

            string sql = "SELECT num, color FROM colors";
            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var color = new Color()
                {
                    num = Int32.Parse(row.ItemArray[0].ToString()),
                    color = row.ItemArray[1].ToString()
                };
                ret.Add(color);
            }

            return ret;
        }
    }
}