using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WINGRAPHVIZLib;

using EventCore;
using Rodemeyer.Visualizing;

namespace EventEditorGUI
{
    public static class GUIHelper
    {
        public static void OnBaseFileLoaded()
        {
            MainWindow.instance.menuAppend.IsEnabled = true;
            MainWindow.instance.menuReloadAll.IsEnabled = true;
            MainWindow.instance.infoText.Text = string.Format("已加载记录 {0} 项", MainWindow.EventDict.Count);
            EventManagerHelper.ClearDictTextForm();
            MainWindow.EventDict.UpdateDictTextForm();
            UpdateEventList(MainWindow.EventDict.Keys);
            EventSL.History.SavePaths();
        }
        public static void OnExtraFileAppended(Dictionary<int, Event> ex)
        {
            MainWindow.instance.infoText.Text = string.Format("已加载记录 {0} 项", MainWindow.EventDict.Count);
            ex.UpdateDictTextForm();
            EventSL.History.SavePaths();
        }

        public static void Log(string str)
        {
            MainWindow.Status(str);
        }

        public static void UpdateEventList(IEnumerable<int> list)
        {
            MainWindow.instance.eventList.Items.Clear();
            foreach (int id in list)
            {
                string itemtext = EventManagerHelper.EventTextForm.ContainsKey(id) ? EventManagerHelper.EventTextForm[id] : (id + " 未加载事件");
                MainWindow.instance.eventList.Items.Add(itemtext);
            }
        }

        public static void UpdateEventToGrid(Grid grid, Event e)
        {
            string[] names = Event.PropertyNames;
            string[] values = e.ToStringArray();
            for (int i = 0; i < names.Length; ++i)
            {
                ScrollViewer v = grid.Children[i + names.Length] as ScrollViewer;
                TextBlock vtxt = v.Content as TextBlock;
                vtxt.Text = values[i];
            }
        }

        public static void UpdateEventToGraph(DotViewer dotViewer, Dictionary<int, Event> EventDict, Event e)
        {
            string plain = EventDict.GetRelationGraph(e);
            MainWindow.instance.dotViewer.LoadPlain(plain, false);
        }
    }

    public static class EventManagerHelper
    {
        private static Dictionary<int, string> _EventTextForm = new Dictionary<int, string>();
        public static Dictionary<int, string> EventTextForm
        {
            get { return _EventTextForm; }
        }
        public static void ClearDictTextForm()
        {
            _EventTextForm.Clear();
        }

        public static void UpdateDictTextForm(this Dictionary<int, Event> dict, int length = 20, int level = 0)
        {
            foreach (int k in dict.Keys)
            {
                if (_EventTextForm.ContainsKey(k))
                {
                    _EventTextForm[k] = dict[k].ShortForm(length, level);
                }
                else
                {
                    _EventTextForm.Add(k, dict[k].ShortForm(length, level));
                }
            }
        }



        public static string GetRelationGraph(this Dictionary<int, Event> dict, Event e)
        {
            string src = "strict digraph MyGraph { ranksep = 1; node[fontname = \"Verdana\"]";

            List<int> idInGraph = new List<int>();
            List<List<int>> Hierarchy = new List<List<int>>();
            Hierarchy.Add(new List<int>(){ e.ID });

            List<int> Temp = new List<int>();
            bool meaningful = true;
            while(meaningful)
            {
                meaningful = false;
                Temp.Clear();
                foreach(int id in Hierarchy[0])
                {
                    List<int> froms = dict.GetFromEventID(id);
                    if(froms.Count > 0)
                    {
                        foreach(int from in froms)
                        {
                            src += string.Format("\"{0}\"->\"{1}\";", from, id);
                        }
                        Temp.AddRange(froms);
                        meaningful = true;
                    }
                }
                if(Temp.Count > 0)
                    Hierarchy.Insert(0, Temp.ToArray().ToList());
                Temp.Clear();
                foreach (int id in Hierarchy[Hierarchy.Count-1])
                {
                    List<int> tos = dict.GetToEventID(id);
                    if (tos.Count > 0)
                    {
                        foreach (int to in tos)
                        {
                            src += string.Format("\"{0}\"->\"{1}\";", id, to);
                        }
                        Temp.AddRange(tos);
                        meaningful = true;
                    }
                }
                if (Temp.Count > 0)
                    Hierarchy.Add(Temp.ToArray().ToList());
                Temp.Clear();
            }

            for(int i = 0; i < Hierarchy.Count; ++i)
            {
                src += string.Format("subgraph cluster_{0}", i) + "{";
                src += string.Format("label=\"Layer {0}\";", i);
                foreach (int id in Hierarchy[i])
                {
                    string alignIDstr = id.ToString();
                    string info = dict[id].UIText.Length > 0 ? dict[id].UIText : dict[id].Note;
                    if(info.Length > 10) info = info.Substring(0, 10);
                    if(alignIDstr.Length < info.Length * 3)
                        alignIDstr = alignIDstr.PadLeft((info.Length * 2- alignIDstr.Length)/ 2);
                    src += string.Format("\"{0}\"[label=\"{1}\\n{2}\", fillcolor=Yellow];", id, alignIDstr, info);
                    
                }
                src += "}";
            }

            src += "}";
            
            string plain = GraphvizHelper.Dot2Plain(src);
            
            return plain;
        }
    }
}
