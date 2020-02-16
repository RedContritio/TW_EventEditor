using EventCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EventEditorGUI
{
    public static class GUIHelper
    {
        public static void OnBaseFileLoaded()
        {
            MainForm.instance.重新载入ToolStripMenuItem.Enabled = true;
            MainForm.instance.载入增量文件ToolStripMenuItem.Enabled = true;
            MainForm.instance.清空所有ToolStripMenuItem.Enabled = true;
            ClearDictTextForm();
            UpdateDictTextForm(MainForm.EventDict);
            UpdateEventList(MainForm.EventDict.Keys);
            EventSL.History.SavePaths();
        }
        public static void OnExtraFileAppended(Dictionary<int, Event> ex)
        {
            UpdateDictTextForm(ex);
            EventSL.History.SavePaths();
        }

        public static void Log(string str)
        {
            MainForm.instance.TopLine.Text = str;
        }

        private static Dictionary<int, string> _EventTextForm = new Dictionary<int, string>();
        public static Dictionary<int, string> EventTextForm
        {
            get { return _EventTextForm;}
        }
        public static void ClearDictTextForm()
        {
            _EventTextForm.Clear();
        }
        public static void UpdateDictTextForm(Dictionary<int, Event> dict, int length = 20, int level = 0)
        {
            foreach(int k in dict.Keys)
            {
                if(_EventTextForm.ContainsKey(k))
                {
                    _EventTextForm[k] = dict[k].ShortForm(length, level);
                }
                else
                {
                    _EventTextForm.Add(k, dict[k].ShortForm(length, level));
                }
            }
        }
        public static void UpdateEventList(IEnumerable<int> list)
        {
            MainForm.instance.listBox1.Items.Clear();
            List<string> itemnames = new List<string>();
            foreach(int id in list)
            {
                if(_EventTextForm.ContainsKey(id))
                {
                    itemnames.Add(_EventTextForm[id]);
                }
                else
                {
                    itemnames.Add(id + " 未加载事件");
                }
            }
            MainForm.instance.listBox1.Items.AddRange(itemnames.ToArray());
        }
    }
}
