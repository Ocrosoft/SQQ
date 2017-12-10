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
                // left is full task, but right not, so right first.
                if (left.sending == maxTask && right.sending != maxTask)
                {
                    return 1; // positive, right first.
                }
                // right is full task, but left not, so left first.
                else if (left.sending != maxTask && right.sending == maxTask)
                {
                    return -1; // negative, left first.
                }
                // sender who send slower first(rate smaller first).
                else
                {
                    return (right.sendCost / right.sent).CompareTo(left.sendCost / left.sent);
                }
            });
            var dispatchCount = Math.Min(problemsNeedSend.Count, senders.Count);
            for (int i = 0; i < dispatchCount; i++)
            {
                // this task will dispatch to this sender.
                var task = problemsNeedSend[i];
                var sender = senders[i];

                // this sender and following are full task.
                if (sender.sending >= maxTask)
                {
                    break;
                }

                list.Add(new Return()
                {
                    team_id = task.team_id,
                    num = task.num,
                    open_id = sender.open_id,
                    name = sender.name
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
    }

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
}
