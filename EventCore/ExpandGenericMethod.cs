using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventCore
{
    public static class ExpandGenericMethod
    {
        public static string[] MapToString<T>(this T[] objs)
        {
            List<string> s = new List<string>();
            foreach (T obj in objs)
            {
                s.Add(obj.ToString());
            }
            return s.ToArray();
        }

        public static bool ArrayEquals<T>(this T[] objs, T[] objs2)
        {
            if (objs.Length != objs2.Length) return false;
            for (int i = 0; i < objs.Length; ++i)
            {
                if (!objs[i].Equals(objs2[i])) return false;
            }
            return true;
        }
        public static bool Compare<T>(T[] objs, T[] objs2)
        {
            if (objs == null || objs2 == null) return objs == objs2;
            return objs.ArrayEquals(objs2);
        }

        public static List<T1> ToList<T1, T2>(this Dictionary<T1, T2>.KeyCollection keys, int begin, int end)
        {
            return keys.Skip(begin).Take(end - begin).ToList();
        }
    }
}
