using System;
using System.Collections.Generic;

namespace SQQ
{
    public class DispatchAlgorithm
    {
        public List<Return> ShellExecute(List<Sender> senders, List<ProblemSolved> problemsNeedSend, int maxTask)
        {
            List<Return> list = new List<Return>();

            /* 简单调度算法 */ /* 在此处实现自己的分配算法即可 */
            senders.Sort(delegate (Sender left, Sender right)
            {
                // left正在配送数量已经最多，right没有
                if (left.sending == maxTask && right.sending != maxTask)
                {
                    return 1; // 返回正数，right优先
                }
                // right正在配送数量已经最多，left没有
                else if (left.sending != maxTask && right.sending == maxTask)
                {
                    return -1; // 返回负数，left优先
                }
                // 比较小的优先
                else
                {
                    return right.sent.CompareTo(left.sent);
                }
            });
            var dispatchCount = Math.Min(problemsNeedSend.Count, senders.Count);
            for (int i = 0; i < dispatchCount; i++)
            {
                // task分配给sender
                var task = problemsNeedSend[i];
                var sender = senders[i];

                // sender和后面的都是满任务的
                if (sender.sending >= maxTask)
                {
                    break;
                }

                list.Add(new Return()
                {
                    team_id = task.team_id,
                    num = task.num,
                    open_id = sender.open_id,
                    name = sender.name,
                    floor = task.floor
                });
            }
            /* -------------- */

            return list;
        }
    }

    public class Return
    {
        public string team_id;
        public int num;
        public string open_id;
        public string name;
        public string floor;
    }

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
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string open_id { get; set; }
        /// <summary>
        /// 已配送气球数
        /// </summary>
        public int sent { get; set; }
        /// <summary>
        /// 正在配送气球数
        /// </summary>
        public int sending { get; set; }
        /// <summary>
        /// 配送花费的总时间
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
            floor = string.Empty;
        }
        /// <summary>
        /// 队伍ID
        /// </summary>
        public string team_id { get; set; }
        /// <summary>
        /// 题目ID
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 配送员微信OPENID
        /// </summary>
        public string sender { get; set; }
        /// <summary>
        /// 任务分配时间
        /// </summary>
        public DateTime timeStart { get; set; }
        /// <summary>
        /// 任务完成时间
        /// </summary>
        public DateTime timeSent { get; set; }
        /// <summary>
        /// 气球要配送到的楼层/考场
        /// </summary>
        public string floor { get; set; }

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
}
