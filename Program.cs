using System;
using System.Collections.Generic;
using System.Reflection;

namespace EventEditor
{
    class Program
    {
        public static Dictionary<string, string> _CmdDict = new Dictionary<string, string> {
            {"跳转", "go" }, {"列表", "list"}, { "查找", "find"}, 
            {"加载", "load"}, {"载入", "load"},
            {"追加", "append"}, { "退出", "exit"},
            {"帮助", "help"}
        };
        public static string CmdMap(string s) => (_CmdDict.ContainsKey(s) ? _CmdDict[s] : s);
        
        static readonly Dictionary<string, string> _CmdHelpDict = new Dictionary<string, string> {
            { "load", "输入路径来加载 Event_Date.txt"},
            { "append", "增量加载 Event_Date.txt"},
            { "list", "列出所有事件，也可以\"list 数量\"列出前若干项，例如 \"list 10\" 列出前十项" },
            { "go", "输入事件ID跳转到某一事件，也可以\"go 事件ID\"直接跳转，例如 \"go 10001\""},
            { "find", "查找符合条件的事件"},
            { "help", "显示帮助"},
            { "exit", "退出程序"},
        };
        static void Main(string[] args)
        {
            Dictionary<int, Event> EventDict = new Dictionary<int, Event>();
            while(true)
            {
                Prompt();
                string[] cmds = Console.ReadLine().Trim().ToLower().Split(" ");
                if (cmds.Length > 0 && cmds[0].Length > 0)
                {
                    Type t = typeof(Program);
                    MethodInfo mt = t.GetMethod("Command_" + CmdMap(cmds[0]));
                    object[] InvokeArgs = new object[] { EventDict, cmds};
                    if(mt != null)
                    {
                        if((bool) mt.Invoke(null, InvokeArgs))
                        {
                            EventDict = (Dictionary<int, Event>) InvokeArgs[0];
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Error("Illegal Command:\"" + cmds[0] + "\"");
                    }
                }
            }
        }

        public static bool Command_go(ref Dictionary<int, Event> EventDict, string[] args)
        {
            int id = -1;
            if (args.Length > 1 && int.TryParse(args[1], out id))
            {
                ; // 正常状态
            }
            else
            {
                Prompt("输入一个事件 ID:");
                string idstr = Console.ReadLine().Trim();
                if (int.TryParse(idstr, out id))
                {
                }
                else
                {
                    Warning("ID 必须是数字。");
                    id = -1;
                }
            }

            if (id == -1)
                return true;

            if (EventDict.ContainsKey(id))
            {
                EventDict.ShowEvent(id);
            }
            else
            {
                Warning("不存在该 ID 对应的事件");
            }
            return true;
        }

        public static bool Command_list(ref Dictionary<int, Event> EventDict, string[] args)
        {
            int cnt = EventDict.Count;
            if (args.Length > 1 && int.TryParse(args[1], out cnt))
            {
                if (cnt > EventDict.Count) cnt = EventDict.Count;
                Output("输出前" + cnt + "个事件");
            }
            else
            {
                Output("输出整个列表");
            }

            foreach (int k in EventDict.Keys)
            {
                if (cnt-- <= 0) break;
                Output(EventDict[k].ShortForm(32));
            }
            return true;
        }
        public static bool Command_find(ref Dictionary<int, Event> EventDict, string[] args)
        {
            Prompt("输入要查找的字符串（仅限 备注 和 UI文本 ）: ");
            string s = Console.ReadLine().Trim();

            foreach (int k in EventDict.Keys)
            {
                if (EventDict[k].Note.Contains(s) || EventDict[k].UIText.Contains(s))
                {
                    Output(EventDict[k].ShortForm(32));
                }
            }
            return true;
        }
        public static bool Command_load(ref Dictionary<int, Event> EventDict, string[] args)
        {
            Prompt("输入要载入的 Event_Date 文件路径: ");
            string path = Console.ReadLine().Trim();
            var state = EventSL.isEventFile(path);
            switch (state)
            {
                case EventSL.FILE_STATE.SUCCESS:
                    {
                        EventDict = EventSL.loadEventFromFile(path);
                        break;
                    }
                case EventSL.FILE_STATE.NOT_EXIST:
                    {
                        Warning("文件不存在。");
                        break;
                    }
                case EventSL.FILE_STATE.ILLEGAL:
                    {
                        Warning("该文件不是合法的 Event_Date 文件。");
                        break;
                    }
                default:
                    {
                        Error("Unexpected branch.");
                        break;
                    }
            }
            return true;
        }
        public static bool Command_append(ref Dictionary<int, Event> EventDict, string[] args)
        {
            Prompt("输入要增量加载的 Event_Date 文件路径: ");
            Dictionary<int, Event> temp = new Dictionary<int, Event>();
            string path = Console.ReadLine().Trim();
            var state = EventSL.isEventFile(path);
            switch (state)
            {
                case EventSL.FILE_STATE.SUCCESS:
                    {
                        temp = EventSL.loadEventFromFile(path);
                        break;
                    }
                case EventSL.FILE_STATE.NOT_EXIST:
                    {
                        Warning("文件不存在。");
                        break;
                    }
                case EventSL.FILE_STATE.ILLEGAL:
                    {
                        Warning("该文件不是合法的 Event_Date 文件。");
                        break;
                    }
                default:
                    {
                        Error("Unexpected branch.");
                        break;
                    }
            }

            int[] cnts = EventDict.Append(temp);
            Warning("覆盖了" + cnts[0] + "个事件，新增了" + cnts[1] + "个事件");
            return true;
        }

        public static bool Command_exit(ref Dictionary<int, Event> EventDict, string[] args)
        {
            return !CLIHelper.InputYESorNO();
        }
        public static bool Command_help(ref Dictionary<int, Event> EventDict, string[] args)
        {
            Output("事件编辑器 v0.1");
            foreach(string methodname in _CmdHelpDict.Keys)
            {
                List<string> legalforms = new List<string>();
                legalforms.Add("\"" + methodname + "\"");
                foreach(string k in _CmdDict.Keys)
                    if(_CmdDict[k] == methodname)
                        legalforms.Add("\"" + k + "\"");
                
                string s = "输入 " + string.Join(" 或者 ", legalforms.ToArray());
                s += "，" + _CmdHelpDict[methodname];
                Output(s);
            }
            return true;
        }
        static void Prompt(string pmp = "") => Console.Write("> " + pmp);
        static void Output(string info) => Console.WriteLine("[Output] " + info);
        static void Warning(string info) => Console.WriteLine("[Warning] " + info);
        static void Error(string info) => Console.WriteLine("[Error] " + info);
    }
}
