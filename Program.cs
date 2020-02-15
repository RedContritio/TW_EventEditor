using System;
using System.Collections.Generic;

namespace EventEditor
{
    class Program
    {
        static void Prompt(string pmp = "") => Console.Write("> " + pmp);
        static void Output(string info) => Console.WriteLine("[Output] " + info);
        static void Warning(string info) => Console.WriteLine("[Warning] " + info);
        static void Error(string info) => Console.WriteLine("[Error] " + info);
        static void Main(string[] args)
        {
            Dictionary<int, Event> EventDict = new Dictionary<int, Event>();
            while(true)
            {
                Prompt();
                string cmd = Console.ReadLine().Trim().ToLower();
                string[] cmds = cmd.Split(" ");
                if(cmd.Length > 0)
                {
                    if(cmds[0].Equals("go") || cmds[0].Equals("跳转"))
                    {
                        int id = -1;
                        if(cmds.Length > 1 && int.TryParse(cmds[1], out id))
                        {
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
                        
                        if(id == -1)
                            continue;

                        if(EventDict.ContainsKey(id))
                        {
                            EventDict.ShowEvent(id);
                        }
                        else
                        {
                            Warning("不存在该 ID 对应的事件");
                        }
                    }
                    else if (cmds[0].Equals("list") || cmds[0].Equals("列表"))
                    {
                        int cnt = EventDict.Count;
                        if (cmds.Length > 1 && int.TryParse(cmds[1], out cnt))
                        {
                            if(cnt > EventDict.Count) cnt = EventDict.Count;
                            Output("输出前" + cnt + "个事件");
                        }
                        else
                        {
                            Output("输出整个列表");
                        }

                        foreach(int k in EventDict.Keys)
                        {
                            if (cnt-- <= 0) break;
                            Output(EventDict[k].ShortForm(32));
                        }
                    }
                    else if (cmds[0].Equals("find") || cmds[0].Equals("查找"))
                    {
                        Prompt("输入要查找的字符串（仅限 备注 和 UI文本 ）: ");
                        string s = Console.ReadLine().Trim();

                        foreach (int k in EventDict.Keys)
                        {
                            if(EventDict[k].Note.Contains(s) || EventDict[k].UIText.Contains(s))
                            {
                                Output(EventDict[k].ShortForm(32));
                            }
                        }
                    }
                    else if(cmd.Equals("load") || cmd.Equals("加载") || cmd.Equals("载入"))
                    {
                        Prompt("输入要载入的 Event_Date 文件路径: ");
                        string path = Console.ReadLine().Trim();
                        var state = EventSL.isEventFile(path);
                        switch(state)
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
                    }
                    else if (cmd.Equals("append") || cmd.Equals("patch") || cmd.Equals("追加"))
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
                    }
                    else if (cmds[0].Equals("exit") || cmds[0].Equals("退出"))
                    {
                        break;
                    }
                    else if (cmds[0].Equals("help") || cmds[0].Equals("帮助"))
                    {
                        Output("事件编辑器 v0.1");
                        Output("输入 \"load\" 或者 \"加载\" 来加载 Event_Date.txt");
                        Output("输入 \"append\" 或者 \"追加\" 来增量加载 Event_Date.txt");
                        Output("输入 \"go\" 或者 \"跳转\" 来跳转到某一事件，也可以直接 \"go 事件ID\"，例如 \"go 10001\"");
                        Output("输入 \"list\" 或者 \"列表\" 来列出所有事件，也可以 \"list 项数\"，例如 \"list 10\" 列出前十项");
                        Output("输入 \"find\" 或者 \"查找\" 来查找符合条件的事件");
                        Output("输入 \"exit\" 或者 \"退出\" 退出程序");
                    }
                    else
                    {
                        Error("Illegal Command:\"" + cmd + "\"");
                    }
                }
            }
        }
    }
}
