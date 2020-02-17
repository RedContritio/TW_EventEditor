using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WINGRAPHVIZLib;
using System.Runtime.InteropServices;
using Rodemeyer.Visualizing;
using System.Reflection;
using System.Windows;

namespace EventEditorGUI
{
    public static class GraphvizHelper
    {
        public static string Dot2Plain(string dotText)
        {
            WINGRAPHVIZLib.DOT twopi = new WINGRAPHVIZLib.DOT();
            string plain = "";
            if (twopi.Validate(dotText) == true)
            {
                plain = twopi.ToPlain(dotText);
            }
            return plain;
        }
    }
}
