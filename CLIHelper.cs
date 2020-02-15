using System;
using System.Collections.Generic;
using System.Text;

namespace EventEditor
{
    public static class CLIHelper
    {
        public static bool InputYorN()
        {
            char c = 'a';
            while(c != 'y' && c != 'n')
            {
                Console.Write("请输入 Y 或者 N: ");
                string s = Console.ReadLine().Trim().ToLower();
                if(s.Length == 1)
                    c = s[0];
            }
            return (c == 'y');
        }
        public static bool InputYESorNO()
        {
            string s = "a";
            while (!s.Equals("yes") && !s.Equals("no"))
            {
                Console.Write("请输入 yes 或者 no: ");
                s = Console.ReadLine().Trim().ToLower();
            }
            return (s.Equals("yes"));
        }
    }
}
