using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventCore
{
    public static class EventManager
    {
        public static void ShowEvent(this Dictionary<int, Event> EventDict, int id)
        {
            Event e = EventDict[id];
            e.ShowBasicInfo();
            if (e.Options != null && e.Options.Length > 0)
            {
                Console.WriteLine("可选选项：");
                for (int i = 0; i < e.Options.Length; ++i)
                {
                    // 故意起名 num1
                    int num1 = e.Options[i];
                    if (EventDict.ContainsKey(num1))
                    {
                        Console.WriteLine(EventDict[num1].MinForm);
                    }
                    else
                    {
                        Console.WriteLine(num1 + "\t" + "未知事件");
                    }
                }
            }
            if (e.JumpInfo != null)
            {
                Console.WriteLine(e.JumpInfo + ((e.JumpToId > 0) ? "\n" + EventDict[e.JumpToId].MinForm : ""));
            }
        }
        public static List<int> GetFromEventID(this Dictionary<int, Event> EventDict, int id)
        {
            List<int> srcs = new List<int>();
            foreach(Event e in EventDict.Values)
            {
                if(e.Options != null && e.Options.Contains(id))
                {
                    srcs.Add(e.ID);
                }
                else if(e.JumpTo.Length > 0 && int.TryParse(e.JumpTo, out int result))
                {
                    if(result == id)
                        srcs.Add(e.ID);
                }
            }
            return srcs;
        }
        public static List<int> GetToEventID(this Dictionary<int, Event> EventDict, int id)
        {
            List<int> dsts = new List<int>();
            Event e = EventDict[id];
            if(e.Options != null)
            {
                foreach(int t in e.Options)
                    dsts.Add(t);
            }
            if(int.TryParse(e.JumpTo, out int jumptoID) && jumptoID > 0)
                dsts.Add(jumptoID);
            return dsts;
        }
        public static int[] Append(this Dictionary<int, Event> EventDict, Dictionary<int, Event> patch)
        {
            int[] cnts = new int[3] { 0, 0, 0 };
            foreach (int key in patch.Keys)
            {
                if (EventDict.ContainsKey(key))
                {
                    if (!EventDict[key].Equals(patch[key]))
                    {
                        EventDict[key] = patch[key];
                        ++cnts[0];
                    }
                    else
                        ++cnts[1];
                }
                else
                {
                    EventDict.Add(key, patch[key]);
                    ++cnts[2];
                }
            }
            return cnts;
        }
    }
}
