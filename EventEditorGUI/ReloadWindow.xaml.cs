using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EventCore;

namespace EventEditorGUI
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class ReloadWindow : Window
    {
        string BaseFilePath = null;
        string[] AppendFilePaths = null;
        public ReloadWindow()
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
            for (int i = 0; i < AppendFilePaths.Length; ++i)
            {
                listBox1.Items.Add(new CheckBox() { IsChecked = true, Content = AppendFilePaths[i]});
            }
        }

        private void buttonL_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                Dictionary<int, Event> baseDict = EventSL.loadEventFromFile(BaseFilePath, out EventSL.FILE_STATE state, false);
                if (state == EventSL.FILE_STATE.SUCCESS)
                {
                    MainWindow.EventDict = baseDict;
                    GUIHelper.OnBaseFileLoaded();

                    int[] appendCount = new int[2]{ 0, 0};
                    for (int i = 0; i < AppendFilePaths.Length; ++i)
                    {
                        if (((CheckBox)listBox1.Items.GetItemAt(i)).IsChecked == true)
                        {
                            Dictionary<int, Event> temp = EventSL.loadEventFromFile(AppendFilePaths[i], out state, true);
                            if (state == EventSL.FILE_STATE.SUCCESS)
                            {
                                int[] result = MainWindow.EventDict.Append(temp);
                                if (result[0] + result[2] > 0)
                                {
                                    ++appendCount[0];
                                    GUIHelper.OnExtraFileAppended(temp);
                                }
                                else
                                {
                                    ++appendCount[1];
                                    MainWindow.Warning("增量文件 " + AppendFilePaths[i] + " 无更改项，无效加载");
                                    EventSL.History.AppendFilePaths.RemoveAt(EventSL.History.AppendFilePaths.Count - 1);
                                }
                            }
                            else
                            {
                                MainWindow.Error("增量文件 " + AppendFilePaths[i] + " 加载失败");
                            }
                        }
                    }

                    MainWindow.Status("主文件加载成功" + (AppendFilePaths.Length > 0 ? string.Format("，加载了 {0} / {1} 个增量文件，有效加载 {2} 个", appendCount[0] + appendCount[1], AppendFilePaths.Length, appendCount[0]) : ""));
                }
                else
                {
                    MainWindow.Error("主文件加载失败");
                }
            }
            Close();
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            if(checkBox1.IsChecked != null)
            {
                listBox1.IsEnabled = (bool)checkBox1.IsChecked;
            }
        }

        private void buttonR_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.EventDict.UpdateDictTextForm();
        }
    }
}
