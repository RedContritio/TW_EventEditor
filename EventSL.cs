using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EventEditor
{
    public static class EventSL
    {
        public enum FILE_STATE {
            SUCCESS,
            NOT_EXIST,
            ILLEGAL
        };
        static string eventFileHead = "#,0,1,2,3,4,5,6,7,8,9,10,11";
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

        public static Dictionary<int, Event> loadEventFromFile(string fileName)
        {
            if(isEventFile(fileName) != FILE_STATE.SUCCESS) return new Dictionary<int, Event>();
            StreamReader istream = new StreamReader(fileName);
            istream.ReadLine(); // jump head line

            Dictionary<int, Event> events = new Dictionary<int, Event>();

            string eventline;
            string[] eventdata;
            while(!istream.EndOfStream)
            {
                eventline = istream.ReadLine().Trim();
                if(eventline != null && eventline.Length > 0)
                {
                    eventdata = eventline.Split(',');
                    

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
            return events;
        }
    }
}
