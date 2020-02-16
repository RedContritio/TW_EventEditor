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
    public partial class ReloadForm : Form
    {
        string BaseFilePath = null;
        string[] AppendFilePaths = null;
        public ReloadForm()
        {
            InitializeComponent();
            BuildcheckedListBox1();
        }

        public void BuildcheckedListBox1()
        {
            BaseFilePath = EventSL.History.BaseFilePath;
            AppendFilePaths = EventSL.History.AppendFilePaths.ToArray();

            EventSL.History.BaseFilePath = null;
            EventSL.History.AppendFilePaths.Clear();

            label1.Text += BaseFilePath;
            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(AppendFilePaths);
            for(int i=0; i<AppendFilePaths.Length; ++i)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBox1.Enabled = checkBox1.Checked;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                Dictionary<int, Event> baseDict = EventSL.loadEventFromFile(BaseFilePath, out EventSL.FILE_STATE state, false);
                if (state == EventSL.FILE_STATE.SUCCESS)
                {
                    MainForm.EventDict = baseDict;
                    GUIHelper.OnBaseFileLoaded();

                    for (int i=0; i<AppendFilePaths.Length; ++i)
                    {
                        if(checkedListBox1.GetItemChecked(i))
                        {
                            Dictionary<int, Event> temp = EventSL.loadEventFromFile(AppendFilePaths[i], out state, true);
                            if (state == EventSL.FILE_STATE.SUCCESS)
                            {
                                int[] result = MainForm.EventDict.Append(temp);
                                if (result[0] + result[2] > 0)
                                {
                                    GUIHelper.OnExtraFileAppended(temp);
                                }
                                else
                                {
                                    MessageBox.Show("增量文件 " + AppendFilePaths[i] + " 无更改项，无效加载", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    EventSL.History.AppendFilePaths.RemoveAt(EventSL.History.AppendFilePaths.Count - 1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("增量文件 " + AppendFilePaths[i] + " 加载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("主文件加载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReloadFrom_Closing(object sender, EventArgs e)
        {
            GUIHelper.UpdateEventList(MainForm.EventDict.Keys);
        }
    }
}
