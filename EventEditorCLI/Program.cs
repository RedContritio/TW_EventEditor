using System;
using System.Collections.Generic;
using System.Reflection;
using EventCore;

namespace EventEditorCLI
{
    class Program
    {
        public static int _Default_Show_Width = 40;
        public static int _Default_Show_Item_Count = 30;
        public static Dictionary<string, string> _CmdDict = new Dictionary<string, string> {
            {"跳转", "go" }, {"来源", "from"}, {"列表", "list"}, { "查找", "find"}, 
            {"加载", "load"}, {"载入", "load"}, { "重加载", "reload"},
            {"追加", "append"}, { "历史", "history"}, {"帮助", "help"},
            {"信息", "info" }, { "退出", "exit"}, 
        };
        public static string CmdMap(string s) => (_CmdDict.ContainsKey(s) ? _CmdDict[s] : s);
        
        static readonly Dictionary<string, string> _CmdHelpDict = new Dictionary<string, string> {
            { "load", "输入路径来加载 Event_Date.txt"},
            { "append", "增量加载 Event_Date.txt"},
            { "reload", "重新加载已经加载的所有文件，并逐个确认是否加载" },
            { "history", "列出当前加载的文件"},
            { "list", "列出所有事件，也可以\"list 数量\"列出前若干项，例如 \"list 10\" 列出前十项" },
            { "go", "输入事件ID跳转到某一事件，也可以\"go 事件ID\"直接跳转，例如 \"go 10001\""},
            { "from", "显示能到达该事件的事件，也可以\"from 事件ID\"直接显示"},
            { "find", "查找符合条件的事件"},
            { "help", "显示帮助"},
            { "info", "显示已经加载的事件集信息"},
            { "exit", "退出程序"},
        };

        public static Dictionary<int, Event> EventDict = new Dictionary<int, Event>();
        static void Main(string[] args)
        {
            
            LoadHistory();

            while(true)
            {
                Prompt();
                string[] cmds = Console.ReadLine().Trim().ToLower().Split(" ".ToCharArray());
                if (cmds.Length > 0 && cmds[0].Length > 0)
                {
                    Type t = typeof(Program);
                    MethodInfo mt = t.GetMethod("Command_" + CmdMap(cmds[0]));
                    object[] InvokeArgs = new object[] {cmds};
                    if(mt != null)
                    {
                        if((bool) mt.Invoke(null, InvokeArgs))
                        {
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
            EventSL.History.SavePaths();
        }

        public static bool Command_go(string[] args)
        {
            if(!CLIHelper.GetIDFromArgsOrInput(args, out int id))
            {
                return true;
            }

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
        public static bool Command_from(string[] args)
        {
            if (!CLIHelper.GetIDFromArgsOrInput(args, out int id))
            {
                return true;
            }

            if (EventDict.ContainsKey(id))
            {
                List<int> srcs = EventDict.GetFromEventID(id);
                if(srcs.Count > 0)
                {
                    Output("共有 " + srcs.Count + "项来源");
                    CLIHelper.PrettyShowList(srcs, _Default_Show_Item_Count, _Default_Show_Width);
                }
                else
                {
                    Output("该事件无来源");
                }
            }
            else
            {
                Warning("不存在该 ID 对应的事件");
            }
            return true;
        }

        public static bool Command_list(string[] args)
        {
            if (args.Length > 1 && int.TryParse(args[1], out int cnt))
            {
                if (cnt > EventDict.Count) cnt = EventDict.Count;
                Output("输出前" + cnt + "个事件");
                CLIHelper.PrettyShowList(EventDict.Keys.ToList(0, cnt), cnt, _Default_Show_Width);
            }
            else
            {
                Output("输出整个列表");
                CLIHelper.PrettyShowList(EventDict.Keys.ToList(0, EventDict.Count), EventDict.Count, _Default_Show_Width);
            }
            return true;
        }
        public static bool Command_find(string[] args)
        {
            Prompt("输入要查找的字符串（仅限 备注 和 UI文本 ）: ");
            string s = Console.ReadLine().Trim();

            foreach (int k in EventDict.Keys)
            {
                if (EventDict[k].Note.Contains(s) || EventDict[k].UIText.Contains(s))
                {
                    Output(EventDict[k].ShortForm(_Default_Show_Width));
                }
            }
            return true;
        }
        public static bool Command_load(string[] args)
        {
            if(EventSL.History.BaseFilePath != null)
            {
                Warning("当前加载文件不为空，直接加载将会覆盖已加载的数据，确认加载吗？");
                if(!CLIHelper.InputYorN()) return true;
            }

            Prompt("输入要载入的 Event_Date 文件路径: ");
            string path = Console.ReadLine().Trim();
            Dictionary<int, Event> dict = EventSL.loadEventFromFile(path, out EventSL.FILE_STATE state);
            if(state == EventSL.FILE_STATE.SUCCESS)
                EventDict = dict;
            else if(state == EventSL.FILE_STATE.NOT_EXIST)
                Warning("文件不存在。");
            else if (state == EventSL.FILE_STATE.ILLEGAL)
                Warning("该文件不是合法的 Event_Date 文件。");
            else
                Error("Unexpected branch.");

            return true;
        }
        public static bool Command_append(string[] args)
        {
            if (EventSL.History.BaseFilePath == null)
            {
                Error("请先使用 load 加载 EventDate 本体。");
            }
            else
            {
                Prompt("输入要增量加载的 Event_Date 文件路径: ");
                string path = Console.ReadLine().Trim();

                Dictionary<int, Event> temp = EventSL.loadEventFromFile(path, out EventSL.FILE_STATE state, true);
                if (state == EventSL.FILE_STATE.SUCCESS)
                {
                    int[] cnts = EventDict.Append(temp);
                    if (cnts[0] + cnts[2] == 0)
                    {
                        Warning("共" + cnts[1] + "个相同项，无更改项，增量加载失败");
                        EventSL.History.AppendFilePaths.RemoveAt(EventSL.History.AppendFilePaths.Count - 1);
                    }
                    else
                    {
                        Warning("覆盖了" + cnts[0] + "个事件，新增了" + cnts[2] + "个事件，有" + cnts[1] + "个相同事件");
                    }
                }
                else if (state == EventSL.FILE_STATE.NOT_EXIST)
                    Warning("文件不存在。");
                else if (state == EventSL.FILE_STATE.ILLEGAL)
                    Warning("该文件不是合法的 Event_Date 增量文件。");
                else
                    Error("Unexpected branch.");
            }
            return true;
        }

        public static bool Command_reload(string[] args)
        {
            string BaseFilePath = EventSL.History.BaseFilePath;
            EventSL.History.BaseFilePath = null;
            string[] AppendFilePaths = EventSL.History.AppendFilePaths.ToArray();
            EventSL.History.AppendFilePaths.Clear();

            if (BaseFilePath != null)
            {
                Output("存在待加载的 EventDate 主文件记录，是否加载？");
                Output("文件路径：" + BaseFilePath);
                if (CLIHelper.InputYorN("是否加载（输入 y 或者 n）: "))
                {
                    EventDict = EventSL.loadEventFromFile(BaseFilePath, out EventSL.FILE_STATE state, false);
                    if (state == EventSL.FILE_STATE.SUCCESS)
                    {
                        Output("加载主文件成功");
                        if (AppendFilePaths.Length > 0)
                        {
                            Output("存在待加载的 EventDate 增量文件记录" + AppendFilePaths.Length + "个");
                            foreach (string s in AppendFilePaths)
                            {
                                Output("文件路径：" + s);
                                if (CLIHelper.InputYorN("是否加载（输入 y 或者 n）: "))
                                {
                                    Dictionary<int, Event> temp = EventSL.loadEventFromFile(s, out state, true);
                                    if (state == EventSL.FILE_STATE.SUCCESS)
                                    {
                                        int[] result = EventDict.Append(temp);
                                        if(result[0] + result[2] > 0)
                                        {
                                            Output("加载成功");
                                        }
                                        else
                                        {
                                            EventSL.History.AppendFilePaths.RemoveAt(EventSL.History.AppendFilePaths.Count-1);
                                            Output("无更改项，无效加载");
                                        }
                                    }
                                    else
                                    {
                                        Output("加载失败");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Output("加载主文件失败");
                    }
                }
            }
            return true;
        }
        public static bool Command_history(string[] args)
        {
            if(EventSL.History.BaseFilePath != null)
            {
                Output("当前加载的主文件：" + EventSL.History.BaseFilePath);
                if(EventSL.History.AppendFilePaths != null && EventSL.History.AppendFilePaths.Count > 0)
                {
                    Output("当前加载了 " + EventSL.History.AppendFilePaths.Count + "个增量文件");
                    foreach(string s in EventSL.History.AppendFilePaths)
                    {
                        Output("+ " + s);
                    }
                }
            }
            else
            {
                Warning("当前未加载任何文件。");
            }
            return true;
        }
        public static bool Command_exit(string[] args)
        {
            return !CLIHelper.InputYESorNO();
        }
        public static bool Command_help(string[] args)
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
        public static bool Command_info(string[] args)
        {
            Output("当前加载的事件共有 " + EventDict.Count + " 项");
            List<int> SrcItems = new List<int>();
            List<int> DstItems = new List<int>();
            foreach (int id in EventDict.Keys)
            {
                List<int> froms = EventDict.GetFromEventID(id);
                List<int> tos = EventDict.GetToEventID(id);
                if(froms.Count == 0) SrcItems.Add(id);
                if(tos.Count == 0) DstItems.Add(id);
            }

            Output("有 " + SrcItems.Count + " 项无源事件");
            Output("有 " + DstItems.Count + " 项终止事件");
            return true;
        }
        public static void Prompt(string pmp = "") => Console.Write("> " + pmp);
        public static void Output(string info) => Console.WriteLine("[Output] " + info);
        public static void Warning(string info) => Console.WriteLine("[Warning] " + info);
        public static void Error(string info) => Console.WriteLine("[Error] " + info);

        public static void LoadHistory()
        {
            EventSL.History.LoadPaths();
            if(EventSL.History.BaseFilePath != null)
            {
                Command_reload(new string[]{ "reload"});
            }
        }
    }
}
