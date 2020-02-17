using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Globalization;

namespace Rodemeyer.Visualizing
{
    class DotParser
    {
        private StreamReader r;
        private string s;
        private int i;
        private bool matched;
        private StringBuilder sb = new StringBuilder();

        readonly int CURRENT_CODE_PAGE = Encoding.Default.CodePage;
        readonly int TARGET_CODE_PAGE = Encoding.UTF8.CodePage;
        public DotParser(string path, bool isPath)
        {
            if(isPath)
            {
                r = new StreamReader(path);
            }
            else
            {
                r = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(path)));
            }
            matched = true;
        }

        public bool ReadLine(string tag)
        {
            if (matched)
            {
                s = r.ReadLine();
                matched = false;
            }
            if (s != null && s.StartsWith(tag))
            {
                i = tag.Length;
                matched = true;
            }
            return matched;
        }

        public string ReadString()
        {
            while (s[i] == ' ') ++i;  
            sb.Length = 0;
            if (s[i] == '"') // Quoted
            {
                ++i;
                while (i < s.Length && s[i] != '"')
                {
                    if(s[i] == '\\' && i+1 < s.Length)
                    {
                        switch(s[i+1])
                        {
                            case 'n':
                                sb.Append('\n');
                                ++i; ++i;
                                continue;
                            default:
                                break;
                        }
                    }
                    sb.Append(s[i]);
                    ++i;
                }
                ++i;
            }
            else
            {
                while (i < s.Length && s[i] != ' ')
                {
                    sb.Append(s[i]);
                    ++i;
                }
            }
            
            return sb.ToString();
        }

        public void Read(int n)
        {
            for (; n > 0; --n)
            {
                while (s[i] == ' ') ++i;
                if (s[i] == '"') // Quoted
                {
                    ++i;
                    while (i < s.Length && s[i] != '"') ++i;
                    ++i;
                }
                else
                {
                    while (i < s.Length && s[i] != ' ') ++i;
                }
            }
        }

        public double ReadDouble()
        {
            return double.Parse(ReadString(), CultureInfo.InvariantCulture);
        }

        public int ReadInt()
        {
            return int.Parse(ReadString(), CultureInfo.InvariantCulture);
        }

        public double yaxis; // the dot coordinate origin is in the lower left, wpf is in the upper left

        public Point ReadPoint()
        {
            return new Point(ReadDouble(), yaxis - ReadDouble());
        }

        internal void Close()
        {
            r.Close();
        }
    }
}
