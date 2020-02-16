using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rodemeyer.Visualizing
{
    public class NodeTipEventArgs : RoutedEventArgs
    {
        public NodeTipEventArgs(RoutedEvent routedEvent, object source, object tag)
            : base(routedEvent, source)
        {
            Tag = tag;
        }

        public readonly object Tag;
        public object Content;
    }

    public delegate void NodeTipEventHandler(object sender, NodeTipEventArgs e);
}
