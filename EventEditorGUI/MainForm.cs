using EventCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EventEditorGUI
{
    public partial class MainForm : Form
    {
        public static Dictionary<int, Event> EventDict { get; set; } = new Dictionary<int, Event>();

        public static MainForm instance = null;
        public MainForm()
        {
            InitializeComponent();
            instance = this;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("暂无帮助\nv0.1", "帮助");
        }

        private void 载入主文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择名称为 Event_Date.txt 的文件";
            fileDialog.Filter = "事件数据文本|Event_Date.txt"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;//返回文件的完整路径      

                TopLine.Text = "载入文件 " + filePath;
                
                Dictionary<int, Event> dict = EventSL.loadEventFromFile(filePath, out EventSL.FILE_STATE state);
                if (state == EventSL.FILE_STATE.SUCCESS)
                {
                    EventDict = dict;
                    GUIHelper.Log("已载入 " + filePath);
                    GUIHelper.OnBaseFileLoaded();
                }
                else if (state == EventSL.FILE_STATE.NOT_EXIST)
                    Warning("文件不存在。");
                else if (state == EventSL.FILE_STATE.ILLEGAL)
                    Warning("该文件不是合法的 Event_Date 文件。");
                else
                    Error("Unexpected branch.");
            }
        }

        public static void Warning(string text)
        {
            MessageBox.Show(text, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void Error(string text)
        {
            MessageBox.Show(text, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void 清空所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

        private void 重新载入ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(EventSL.History.BaseFilePath != null)
            {
                Form reloadForm = new ReloadForm();
                reloadForm.ShowDialog();
            }
        }

        private void 载入增量文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择名称为 .txt 的文件";
            fileDialog.Filter = "事件增量数据文本|*.txt"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;//返回文件的完整路径      

                TopLine.Text = "载入文件 " + filePath;

                Dictionary<int, Event> temp = EventSL.loadEventFromFile(filePath, out EventSL.FILE_STATE state, true);
                if (state == EventSL.FILE_STATE.SUCCESS)
                {
                    int[] cnts = EventDict.Append(temp);
                    if (cnts[0] + cnts[2] == 0)
                    {
                        Warning("共" + cnts[1] + "个相同项，无更改项，增量加载失败");
                        EventSL.History.AppendFilePaths.RemoveAt(EventSL.History.AppendFilePaths.Count() - 1);
                    }
                    else
                    {
                        GUIHelper.Log("覆盖了" + cnts[0] + "个事件，新增了" + cnts[2] + "个事件，有" + cnts[1] + "个相同事件");
                    }
                    GUIHelper.OnExtraFileAppended(temp);
                }
                else if (state == EventSL.FILE_STATE.NOT_EXIST)
                    Warning("文件不存在。");
                else if (state == EventSL.FILE_STATE.ILLEGAL)
                    Warning("该文件不是合法的 Event_Date 增量文件。");
                else
                    Error("Unexpected branch.");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            EventSL.History.LoadPaths();
            if (EventSL.History.BaseFilePath != null)
            {
                Form reloadForm = new ReloadForm();
                reloadForm.ShowDialog();
            }
        }
        private void MainForm_Closing(object sender, EventArgs e)
        {
            EventSL.History.SavePaths();

        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0)
            {
                List<int> idxs = new List<int>();
                foreach(int k in EventDict.Keys)
                {
                    if(k.ToString().Contains(textBox1.Text))
                    {
                        idxs.Add(k);
                    }
                    else if(EventDict[k].Note.Contains(textBox1.Text) ||
                        EventDict[k].UIText.Contains(textBox1.Text))
                    {
                        idxs.Add(k);
                    }
                }
                GUIHelper.UpdateEventList(idxs);
            }
            else
            {
                GUIHelper.UpdateEventList(EventDict.Keys);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13) // Enter
            {
                SearchButton.PerformClick();
            }
        }
    }
}
