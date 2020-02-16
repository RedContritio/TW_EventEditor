using System;
using System.Collections.Generic;
using System.Text;

namespace EventCore
{
    public class Event
    {
        public int ID;
        
        public string Note;// 备注

        public string PictureID; // 图片 ID

        public string AvatarID; // 头像 ID

        public string UIText; // UI 文本

        public int[] Parameters; // 参数列表

        public int[] Options; // 选项列表

        public string[] Requirements; // 需求

        public string JumpTo; // 跳转

        public string[] Effects; // 事件结果

        public string UnknownData1;

        public string UnknownData2;

        public int TextBox; // 启用文本框
        public void ShowBasicInfo()
        {
            Console.Write(IDInfo);
            if (NoteInfo != null) Console.Write("\t" + NoteInfo);
            if (PictureIDInfo != null) Console.Write("\t" + PictureIDInfo);
            if (AvatarIDInfo() != null) Console.Write("\t" + AvatarIDInfo());
            if (ParametersInfo() != null) Console.Write("\t" + ParametersInfo());
            Console.WriteLine();
            bool needNewLine = false;
            if(TextBox != 0)
            {
                Console.Write("含有文本框" + "\t");
                needNewLine = true;
            }
            if (EffectsInfo() != null)
            {
                Console.Write(EffectsInfo() + "\t");
                needNewLine = true;
            }
            if(needNewLine) Console.WriteLine();
            if(UIText.Length > 0)
            {
                Console.WriteLine(UITextInfo);
            }
        }
        public override string ToString()
        {
            return ID.ToString() + "," + Note + "," + PictureID + "," + AvatarID + "," + UIText + "," +
                (Parameters == null ? "" : string.Join("|", Parameters.MapToString<int>())) + "," +
                (Options == null ? "" : string.Join("|", Options.MapToString<int>())) + "," +
                string.Join("|", Requirements) + "," +
                JumpTo + "," + string.Join("|", Effects) + "," + UnknownData1 + "," + UnknownData2 + "," + TextBox.ToString();
        }

        public override bool Equals(object obj)
        {
            Event e = (Event)obj;
            if (!(ID == e.ID)) return false;
            if (!(Note == e.Note)) return false;
            if (!(PictureID == e.PictureID)) return false;
            if (!(AvatarID == e.AvatarID)) return false;
            if (!(UIText == e.UIText)) return false;
            if (!(ExpandGenericMethod.Compare(Parameters, e.Parameters))) return false;
            if (!(ExpandGenericMethod.Compare(Options, e.Options))) return false;
            if (!(ExpandGenericMethod.Compare(Requirements, e.Requirements))) return false;
            if (!(JumpTo == e.JumpTo)) return false;
            if (!(ExpandGenericMethod.Compare(Effects, e.Effects))) return false;
            if (!(UnknownData1.Equals(e.UnknownData1))) return false;
            if (!(UnknownData2.Equals(e.UnknownData2))) return false;
            if (!(TextBox == e.TextBox)) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public string MinForm => ID + "\t" +
            ((RequirementsInfo() != null) ? (RequirementsInfo() + "\t") : "") +
            (UIText.Length > 0 ? UIText : Note);

        public string ShortForm(int length)
        {
            string s = MinForm;
            if(length < s.Length)
                return s.Substring(0, length);
            else
                return s;
        }

        public string IDInfo => "事件ID: " + ID.ToString();
        public string NoteInfo => (Note.Length > 0) ? "备注：" + Note : null;
        public string PictureIDInfo => (PictureID.Length > 0) ? "照片ID：" + PictureID : null;
        public string UITextInfo => "UI 文本：\t" + UIText;

        public string JumpInfo => (int.TryParse(JumpTo, out int target) ? (((target > 0) ? "跳转至" : "特殊跳转") + target) : null);

        public int JumpToId => int.Parse(JumpTo);

        public string AvatarIDInfo()
        {
            if(int.TryParse(AvatarID, out int id))
            {
                if(id == 0)
                    return "头像：动态";
                else if(id == -1)
                    return "头像：太吾";
                else if(id == -99)
                    return "头像：无";
                else
                    return "头像ID：" + AvatarID;
            }
            return null;
        }

        public string ParameterInfo(int param)
        {
            string[] results = {
                "", "角色ID", "道具ID", "功法ID", "资源ID",
                "道具ID", "baseSkillID", "数值", "skillID", "好感等级",
                "未知", "partWorldMap", "角色ID", "gangValueId"};

            if(param >= 0 && param < results.Length)
            {
                return "<" + param + ">" + " " + results[param];
            }
            return "<" + param + ">" + " " + "未知参数";
        }
        public string ParametersInfo()
        {
            if(Parameters != null && Parameters.Length > 0)
            {
                string result = "参数列表：" + ParameterInfo(Parameters[0]);
                for(int i = 1; i < Parameters.Length; ++i)
                {
                    result += "，" + ParameterInfo(Parameters[i]);
                }
                return result;
            }
            else
                return null;
        }

        public string RequirementsInfo()
        {
            if(Requirements != null && Requirements.Length > 0 && Requirements[0].Length > 0)
            {
                return "需求：" + string.Join(" | ", Requirements);
            }
            else
                return null;
        }
        public string EffectsInfo()
        {
            if (Effects != null && Effects.Length > 0 && Effects[0].Length > 0)
            {
                return "触发结果：" + string.Join(" | ", Effects);
            }
            else
                return null;
        }
    }

    public static class EventExpandMethod
    {
        public static Event ToEvent(this string[] eventdata)
        {
            Event e = new Event();
            int index = 0;
            e.ID = int.Parse(eventdata[index++]); // 0
            e.Note = eventdata[index++]; // 1
            e.PictureID = eventdata[index++]; // 2
            e.AvatarID = eventdata[index++]; // 3
            e.UIText = eventdata[index++]; // 4

            string[] parameterStrs = eventdata[index++].Split("|".ToCharArray()); // 5
            if (parameterStrs.Length == 0 || parameterStrs[0].Length == 0)
                e.Parameters = null;
            else
            {
                e.Parameters = new int[parameterStrs.Length];
                for(int i = 0; i < parameterStrs.Length; ++i)
                {
                    e.Parameters[i] = int.Parse(parameterStrs[i]);
                }
            }

            string[] optionStrs = eventdata[index++].Split("|".ToCharArray()); // 6
            if(optionStrs.Length == 0 || optionStrs[0].Length == 0)
                e.Options = null;
            else
            {
                e.Options = new int[optionStrs.Length];
                for (int i = 0; i < optionStrs.Length; ++i)
                {
                    e.Options[i] = int.Parse(optionStrs[i]);
                }
            }

            e.Requirements = eventdata[index++].Split("|".ToCharArray()); // 7

            e.JumpTo = eventdata[index++]; // 8
            
            e.Effects = eventdata[index++].Split("|".ToCharArray()); // 9

            e.UnknownData1 = eventdata[index++]; // 10

            e.UnknownData2 = eventdata[index++]; // 11

            e.TextBox = int.Parse(eventdata[index++]); // 12

            return e;
        }
    }
}
