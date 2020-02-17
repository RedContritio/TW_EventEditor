using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Rodemeyer.Visualizing
{
    public class SelectNodeEventArgs : RoutedEventArgs
    {
        public SelectNodeEventArgs(RoutedEvent routedEvent, object source, object tag)
            : base(routedEvent, source)
        {
            Tag = tag;
        }

        public readonly object Tag;
    }

    public delegate void SelectNodeEventHandler(object sender, SelectNodeEventArgs e);
    public delegate object SelectNodeProviderDelegate(object tag);
}