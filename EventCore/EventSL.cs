using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EventCore
{
    public static class EventSL
    {
        public enum FILE_STATE {
            SUCCESS,
            NOT_EXIST,
            ILLEGAL
        };


        static readonly string eventFileHead = "#,0,1,2,3,4,5,6,7,8,9,10,11";
        public static FILE_STATE isEventFile(string fileName)
        {
            if(!File.Exists(fileName)) return FILE_STATE.NOT_EXIST;
            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string head = sr.ReadLine();

            sr.Close();
            fs.Close();

            if (head != null && head.Equals(eventFileHead))
            {
                return FILE_STATE.SUCCESS;
            }
            else return FILE_STATE.ILLEGAL;
        }
        private static void Debug(string info, int level = 0)
        {
            Console.WriteLine(info);
        }

        public static Dictionary<int, Event> loadEventFromFile(string filePath, out FILE_STATE state, bool isAppend = false)
        {
            state = isEventFile(filePath);
            if (state != FILE_STATE.SUCCESS)
                return new Dictionary<int, Event>();

            StreamReader istream = new StreamReader(filePath);
            istream.ReadLine(); // skip head line

            Dictionary<int, Event> events = new Dictionary<int, Event>();

            string eventline;
            string[] eventdata;
            while(!istream.EndOfStream)
            {
                eventline = istream.ReadLine().Trim();
                if(eventline != null && eventline.Length > 0)
                {
                    eventdata = eventline.Split(',');
                    for(int i=0; i<eventdata.Length; ++i)
                        eventdata[i].Trim();

                    if(eventdata.Length >= 13)
                    {
                        if(int.TryParse(eventdata[0], out int id))
                        {
                            events.Add(id, eventdata.ToEvent());
                        }
                        else
                        {
                            Debug("不合法数据：\"" + eventline + "\"，失败原因：不合法 ID");
                        }
                    }
                    else
                    {
                        Debug("不合法数据：\"" + eventline + "\"，失败原因：项数过少（需要 13，只有");
                    }
                }
            }

            istream.Close();
            
            if(isAppend)
            {
                History.AppendFilePaths.Add(filePath);
            }
            else
            {
                History.BaseFilePath = filePath;
                History.AppendFilePaths.Clear();
            }
            History.SavePaths();

            return events;
        }

        public static class History
        {
            public static string BaseFilePath = null;
            public static List<string> AppendFilePaths = new List<string>();
            static readonly string SaveFilePath = "History.xml";
            public static void SavePaths()
            {
                List<string> Data = new List<string>(AppendFilePaths.ToArray());
                Data.Insert(0, BaseFilePath);

                XmlSerializer writer = new XmlSerializer(typeof(List<string>));
                FileStream file = File.Create(SaveFilePath);
                writer.Serialize(file, Data);

                file.Close();
            }

            public static void LoadPaths()
            {
                XmlSerializer reader = new XmlSerializer(typeof(List<string>));
                if(!File.Exists(SaveFilePath))
                    return;
                StreamReader file = new StreamReader(SaveFilePath);
                List<string> Data = (List<string>)reader.Deserialize(file);
                file.Close();

                if (Data.Count > 0) BaseFilePath = Data[0];
                if (Data.Count > 1)
                {
                    AppendFilePaths = Data;
                    AppendFilePaths.RemoveAt(0);
                }
            }
        };

    }
}
