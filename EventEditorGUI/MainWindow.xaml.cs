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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using EventCore;

namespace EventEditorGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance = null;

        public static Dictionary<int, Event> EventDict = new Dictionary<int, Event>();
        public MainWindow()
        {
            InitializeComponent();
            if(instance == null) instance = this;
        }

        private void MenuLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择名称为 Event_Date.txt 的文件";
            fileDialog.Filter = "事件数据文本|Event_Date.txt"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == true)
            {
                string filePath = fileDialog.FileName;//返回文件的完整路径      

                //TopLine.Text = "载入文件 " + filePath;

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

        private void MenuAppend_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择名称为 .txt 的文件";
            fileDialog.Filter = "事件增量数据文本|*.txt"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == true)
            {
                string filePath = fileDialog.FileName;//返回文件的完整路径      

                bottomStatus.Text = "载入增量文件 " + filePath;

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

        private void MenuReloadAll_Click(object sender, RoutedEventArgs e)
        {
            if (EventSL.History.BaseFilePath != null)
            {
                Window window = new ReloadWindow();
                window.ShowDialog();
            }
        }


        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by 炽翼幻灵", "关于", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public static MessageBoxResult Warning(string info) => MessageBox.Show(info, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
        public static MessageBoxResult Error(string info) => MessageBox.Show(info, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        public static void Status(string info) => instance.bottomStatus.Text = info;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EventSL.History.LoadPaths();
            if (EventSL.History.BaseFilePath != null)
            {
                Window window = new ReloadWindow();
                window.ShowDialog();
            }
            string[] names = Event.PropertyNames;
            int[] height = new int[13]{ 1, 1, 1, 1, 5, 1, 2, 2, 1, 2, 1, 1, 1};
            for (int i = 0; i < names.Length; ++i)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new System.Windows.GridLength(height[i], System.Windows.GridUnitType.Star);
                propertyGrid.RowDefinitions.Add(rowdef);
            }

            propertyGrid.ShowGridLines = true;
            propertyGrid.UpdateLayout();

            for (int i = 0; i < names.Length; ++i)
            {
                TextBlock txt = new TextBlock();
                txt.Text = Event.PropertyNameTranslate(names[i]) + "/" + names[i];
                propertyGrid.Children.Add(txt);
                Grid.SetRow(txt, i);
                Grid.SetColumn(txt, 0);
            }
            for (int i = 0; i < names.Length; ++i)
            {
                ScrollViewer scrollViewer = new ScrollViewer();
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                TextBlock btxt = new TextBlock();
                btxt.TextWrapping = TextWrapping.Wrap;
                scrollViewer.Content = btxt;
                propertyGrid.Children.Add(scrollViewer);
                Grid.SetRow(scrollViewer, i);
                Grid.SetColumn(scrollViewer, 1);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                List<int> idxs = new List<int>();
                foreach (int k in EventDict.Keys)
                {
                    if (k.ToString().Contains(textBox1.Text))
                    {
                        idxs.Add(k);
                    }
                    else if (EventDict[k].Note.Contains(textBox1.Text) ||
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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStates == Keyboard.GetKeyStates(Key.Enter)) // Enter
            {
                searchButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); ;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void EventList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string idstr = e.AddedItems[0].ToString().Split(new char[] { ' ' })[0];
            if(int.TryParse(idstr, out int id))
            {
                GUIHelper.UpdateEventToGrid(propertyGrid, EventDict[id]);
                GUIHelper.UpdateEventToGraph(dotViewer, EventDict, EventDict[id]);
            }
            else
            {
                Error(e.AddedItems[0].ToString() + "无法解析得到ID");
            }
        }

        public void UpdateRelationPage(Event e)
        {
        }

        private void dotViewer_ShowNodeTip(object sender, Rodemeyer.Visualizing.NodeTipEventArgs e)
        {
            e.Handled = true;
            string key = e.Tag as string;
            if(int.TryParse(key, out int id))
            {
                e.Content = EventDict[id].ShortForm(50, 1);
            }
        }

        private void dotViewer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(dotViewer.SelectNodeTag != null)
            {
                if(int.TryParse(dotViewer.SelectNodeTag, out int id))
                {
                    int i = EventDict.GetEventIndex(id);
                    if(i >= 0)
                    {
                        eventList.SelectedIndex = i;
                    }
                    eventList.ScrollIntoView(eventList.Items[i]);
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dotViewer.ZoomTo(1);
        }
    }
}
