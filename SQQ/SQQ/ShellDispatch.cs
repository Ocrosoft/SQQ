using System;
using System.Collections.Generic;

namespace SQQ
{
    public class ShellDispatch
    {
        public static Dictionary<string, Object> execute(int maxTask = 1)
        {
            Dictionary<string, Object> ret = new Dictionary<string, Object>();
            ret["error"] = false;
            ret["message"] = string.Empty;
            var sendingList = new List<string>();
            var floors = Database.GetsFloor();
            //var colors = Database.GetsColor();

            // 根据提交时间降序排列
            var problemsNeedSend = Database.GetsProblemSaved();
            List<ProblemSolved> _problemsNeedSend = new List<ProblemSolved>();
            foreach(var p in problemsNeedSend)
            {
                _problemsNeedSend.Add(new ProblemSolved()
                {
                    num = p.num,
                    sender = p.sender,
                    team_id = p.team_id,
                    timeStart = p.timeStart,
                    timeSent = p.timeSent,
                    floor = p.floor
                });
            }
            // 根据注册时间升序排列
            var senders = Database.GetsSender();
            List<Sender> _senders = new List<Sender>();
            foreach(var s in senders)
            {
                _senders.Add(new Sender()
                {
                    name = s.name,
                    open_id = s.open_id,
                    sending = s.sending,
                    sent = s.sent
                });
            }

            List<Return> returnList = new DispatchAlgorithm().ShellExecute(_senders, _problemsNeedSend, maxTask);
            foreach (var o in returnList)
            {
                // 添加在在送列表
                if (Database.AddProblemSending(o.team_id, o.num, o.open_id))
                {
                    if (Database.DeleteProblemSaved(o.team_id, o.num))
                    {
                        sendingList.Add(o.team_id + " " + (char)(o.num + 'A') + "题 分配给" + o.name);
                    }
                    else // 删除失败，从在送列表中删除
                    {
                        Database.DeleteProblemSending(o.team_id, o.num);
                        ret["error"] = true;
                        ret["message"] = "An error has occured, can't delete problem from saved.";
                        ret["detail"] = "problem(" + o.team_id + "," + o.num + ")";
                        return ret;
                    }
                }
                else
                {
                    ret["error"] = true;
                    ret["message"] = "An error has occured, can't add problem to sending.";
                    ret["detail"] = "problem(" + o.team_id + "," + o.num + "),sender(" + o.open_id + ")";
                    return ret;
                }
            }
            ret["sendingList"] = sendingList;
            return ret;
        }
    }
}
