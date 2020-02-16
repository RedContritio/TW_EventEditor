using System;
using System.Collections.Generic;
using System.Text;

namespace EventEditorCLI
{
    public static class CLIHelper
    {
        public static string InputInLowerCaseField(string prompt, ICollection<string> field)
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
            string s = InputInLowerCaseField(prompt, new List<string>(){ "y", "n"});
            return s.Equals("y");
        }
        public static bool InputYESorNO(string prompt = "请输入 yes 或者 no: ")
        {
            string s = InputInLowerCaseField(prompt, new List<string>() { "yes", "no" });
            return s.Equals("yes");
        }

        public static bool GetIDFromArgsOrInput(string[] args, out int id)
        {
            if (args != null && args.Length > 1 && int.TryParse(args[1], out id))
            {
                return true; // 正常状态
            }
            else
            {
                Program.Prompt("输入一个事件 ID:");
                string idstr = Console.ReadLine().Trim();
                if (int.TryParse(idstr, out id))
                {
                    return true;
                }
                else
                {
                    Program.Warning("ID 必须是数字。");
                    return false;
                }
            }
        }

        public static void PrettyShowList(List<int> ids, int count, int width)
        {
            int showCount = ids.Count;
            if (showCount > count)
            {
                Program.Output("由于过多，仅显示前" + count + "项");
                showCount = count;
            }
            for (int i = 0; i < showCount; ++i)
            {
                Program.Output(Program.EventDict[ids[i]].ShortForm(width));
            }
        }
    }
}
