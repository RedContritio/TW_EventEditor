using System;
using System.Collections.Generic;
using System.Text;

namespace EventEditor
{
    public static class CLIHelper
    {
        public static string InputInLowerCaseField(string prompt, ISet<string> field)
        {
            string s = null;
            while(!field.Contains(s))
            {
                Console.Write(prompt);
                s = Console.ReadLine().Trim().ToLower();
            }
            return s;
        }
        public static bool InputYorN(string prompt = "请输入 Y 或者 N: ")
        {
            string s = InputInLowerCaseField(prompt, new SortedSet<string>(){ "y", "n"});
            return s.Equals("y");
        }
        public static bool InputYESorNO(string prompt = "请输入 yes 或者 no: ")
        {
            string s = InputInLowerCaseField(prompt, new SortedSet<string>() { "yes", "no" });
            return s.Equals("yes");
        }
    }
}
