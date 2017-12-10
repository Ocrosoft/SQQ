using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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
                sendCost = 9999.99;
            }
            /// <summary>
            /// The name of this sender.
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// The OPENID of this sender's WeChat.
            /// </summary>
            public string open_id { get; set; }
            /// <summary>
            /// The number of qq this sender has sent.
            /// </summary>
            public int sent { get; set; }
            /// <summary>
            /// The number of qq this sender is sending.
            /// </summary>
            public int sending { get; set; }
            /// <summary>
            /// Second this sender cost.
            /// </summary>
            public double sendCost { get; set; }
        }
        public class ProblemSolved : IEqualityComparer<ProblemSolved>
        {
            public ProblemSolved()
            {
                team_id = string.Empty;
                num = -1;
                sender = null;
                time = DateTime.Now;
            }
            /// <summary>
            /// The ID of team which solved this problem.
            /// </summary>
            public string team_id { get; set; }
            /// <summary>
            /// The position of this problem in contest. Start with 0.
            /// </summary>
            public int num { get; set; }
            /// <summary>
            /// The sender' OPENID of this problem and team.
            /// </summary>
            public string sender { get; set; }
            /// <summary>
            /// The time of reportback.
            /// </summary>
            public DateTime time { get; set; }

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

        /* problems */
        /// <summary>
        /// Get all problems' AC status.
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSolved()
        {
            // list
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
                ret.Add(ps);
            }

            // return the list
            return ret;
        }
        /// <summary>
        /// Get all problems which qq not send.
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSaved()
        {
            // list
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num FROM problem_saved";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString())
                });
            }

            // return the list
            return ret;
        }
        /// <summary>
        /// Get all problems which qq has sent.
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSent()
        {
            // list
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num, sender, time FROM sent";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    time = DateTime.Parse(row.ItemArray[3].ToString())
                });
            }

            // return the list
            return ret;
        }
        /// <summary>
        /// Get all problems which qq had sent by open_id.
        /// </summary>
        /// <param name="open_id">The OPENID of this sender's WeChat.</param>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSent(string open_id)
        {
            // list
            List<ProblemSolved> ret = new List<ProblemSolved>();
            string sql = "SELECT team_id, num, sender, time FROM sent WHERE sender = ?open_id ORDER BY time DESC";

            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    time = DateTime.Parse(row.ItemArray[3].ToString())
                });
            }

            // return
            return ret;
        }
        /// <summary>
        /// Get problem which qq had sent.
        /// </summary>
        /// <param name="team_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static ProblemSolved GetProblemSent(string team_id, int num)
        {
            string sql = "SELECT team_id, num, sender, time FROM sent WHERE  team_id = ?team_id AND num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            var ds = MySQLHelper.ExecuteDataSet(sql, para);
            DataRow row = ds.Tables[0].Rows[0];
            return new ProblemSolved()
            {
                team_id = row.ItemArray[0].ToString(),
                num = int.Parse(row.ItemArray[1].ToString()),
                sender = row.ItemArray[2].ToString(),
                time = DateTime.Parse(row.ItemArray[3].ToString())
            };
        }
        /// <summary>
        /// Get all problems which qq is sending.
        /// </summary>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSending()
        {
            // list
            List<ProblemSolved> ret = new List<ProblemSolved>();

            string sql = "SELECT team_id, num, sender, time FROM sending";

            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    time = DateTime.Parse(row.ItemArray[3].ToString())
                });
            }

            // return the list
            return ret;
        }
        /// <summary>
        /// Get all problems which qq is sending by open_id.
        /// </summary>
        /// <param name="open_id">The OPENID of this sender's WeChat.</param>
        /// <returns></returns>
        public static List<ProblemSolved> GetsProblemSending(string open_id)
        {
            // list
            List<ProblemSolved> ret = new List<ProblemSolved>();
            string sql = "SELECT team_id, num, sender, time FROM sending WHERE sender = ?open_id";

            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ProblemSolved()
                {
                    team_id = row.ItemArray[0].ToString(),
                    num = int.Parse(row.ItemArray[1].ToString()),
                    sender = row.ItemArray[2].ToString(),
                    time = DateTime.Parse(row.ItemArray[3].ToString())
                });
            }

            // return
            return ret;
        }
        /// <summary>
        /// Get problem which qq is sending.
        /// </summary>
        /// <param name="team_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static ProblemSolved GetProblemSending(string team_id, int num)
        {
            string sql = "SELECT team_id, num, sender, time FROM sending WHERE team_id = ?team_id AND num = ?num";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            var ds = MySQLHelper.ExecuteDataSet(sql, para);
            DataRow row = ds.Tables[0].Rows[0];
            return new ProblemSolved()
            {
                team_id = row.ItemArray[0].ToString(),
                num = int.Parse(row.ItemArray[1].ToString()),
                sender = row.ItemArray[2].ToString(),
                time = DateTime.Parse(row.ItemArray[3].ToString())
            };
        }
        /// <summary>
        /// Add a record, means this problem need to send qq.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
        /// <returns></returns>
        public static bool AddProblemSaved(string team_id, int num)
        {
            string sql = "INSERT INTO problem_saved VALUES(?team_id, ?num)";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?team_id", team_id);
            para[1] = new MySqlParameter("?num", num);

            if(MySQLHelper.ExecuteNonQuery(sql,para) == 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Delete a problem from table which means need to send qq.
        /// Usually use with function AddProblemSending.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
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
        /// Add a record, means this problem's qq is being sent.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
        /// <param name="open_id">The OPENID of this sender's WeChat.</param>
        /// <returns></returns>
        public static bool AddProblemSending(string team_id, int num, string open_id)
        {
            string sql = "INSERT INTO sending(team_id, num, sender, time) VALUES(?team_id, ?num, ?sender, now())";
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
        /// Delete a problem from table which means being sent.
        /// Usually use with function AddProblemSent.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
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
        /// Add a record, means this problem's qq has sent.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
        /// <param name="open_id">The OPENID of this sender's WeChat.</param>
        /// <returns></returns>
        public static bool AddProblemSent(string team_id, int num, string open_id)
        {
            string sql = "INSERT INTO sent VALUES(?team_id, ?num, ?sender, now())";
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
        /// Delete a problem from table which means has sent.
        /// </summary>
        /// <param name="team_id">The ID of team which solved this problem.</param>
        /// <param name="num">The position of this problem in contest. Start with 0.</param>
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
        /* senders */
        /// <summary>
        /// Get all senders added to this contest.
        /// </summary>
        /// <returns>a list of senders.</returns>
        public static List<Sender>  GetsSender()
        {
            // list
            List<Sender> ret = new List<Sender>();

            string sql = "SELECT name, open_id, send_rate FROM sender";
            var ds = MySQLHelper.ExecuteDataSet(sql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var sender = new Sender()
                {
                    name = row.ItemArray[0].ToString(),
                    open_id = row.ItemArray[1].ToString(),
                    sendCost = double.Parse(row.ItemArray[2].ToString())
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

            // return
            return ret;
        }
        /// <summary>
        /// Add a new sender to current contest.
        /// </summary>
        /// <param name="open_id">OPENID of sender's WeChat.</param>
        /// <param name="name">Name of sender.</param>
        /// <returns>true if success, false otherwise.</returns>
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
        /// Delete the sender whose open_id exactly equaled to the parameter.
        /// </summary>
        /// <param name="open_id">OPENID of sender's WeChat.</param>
        /// <returns>true if success, false otherwise.</returns>
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
        /// Get sender with openID.
        /// </summary>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static Sender GetSender(string open_id)
        {
            string sql = "SELECT * FROM sender WHERE open_id = ?open_id";
            var ds = MySQLHelper.ExecuteDataSet(sql, new MySqlParameter("?open_id", open_id));
            return new Sender()
            {
                open_id = ds.Tables[0].Rows[0].ItemArray[0].ToString(),
                name = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                sendCost = double.Parse(ds.Tables[0].Rows[0].ItemArray[2].ToString())
            };
        }
        /// <summary>
        /// Auto select operation with sender's total cost.
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="open_id"></param>
        /// <returns></returns>
        public static bool UpdateSendRate(double cur, string open_id)
        {
            var sender = GetSender(open_id);
            if (sender.sendCost >= 99999) 
            {
                sender.sendCost = cur;
            }
            else
            {
                sender.sendCost += cur;
            }
            string sql = "UPDATE sender SET send_rate = ?send_rate where open_id = ?oid";
            MySqlParameter[] para = new MySqlParameter[2];
            para[0] = new MySqlParameter("?send_rate", sender.sendCost);
            para[1] = new MySqlParameter("?oid", open_id);

            if(MySQLHelper.ExecuteNonQuery(sql,para) == 1)
            {
                return true;
            }
            return false;
        }
        /* system */
        /// <summary>
        /// Initiation, delete all rows in all tables.
        /// </summary>
        /// <returns>true if success, false otherwise.</returns>
        public static bool Init()
        {
            string sql_clear_problem_saved = "DELETE FROM problem_saved";
            string sql_clear_sender = "DELETE FROM sender";
            string sql_clear_sending = "DELETE FROM sending";
            string sql_clear_sent = "DELETE FROM sent";

            return MySQLHelper.ExecuteNoQueryTran(new List<string>
            {
                sql_clear_problem_saved,
                sql_clear_sender,
                sql_clear_sending,
                sql_clear_sent
            });
        }
    }
}