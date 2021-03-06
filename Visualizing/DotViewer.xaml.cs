using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Threading;
using System.Printing;

namespace Rodemeyer.Visualizing
{
    /// <summary>
    /// Interaction logic for Visualizing.xaml
    /// </summary>
    public partial class DotViewer : System.Windows.Controls.UserControl
    {
        public static readonly RoutedEvent ShowNodeTipEvent;

        public string SelectNodeTag => DotGraph.SelectNodeTag;

        

        static DotViewer()
        {
            ShowNodeTipEvent = EventManager.RegisterRoutedEvent("ShowNodeTip", RoutingStrategy.Bubble, typeof(NodeTipEventHandler), typeof(DotViewer));
        }

        public DotViewer()
        {
            InitializeComponent();

            PreviewMouseLeftButtonDown += PreviewMouseRightButtonDownHandler;  // obsolete if zoom rectangles are supported
            MouseLeftButtonUp += MouseRightButtonUpHandler;                    // obsolete if zoom rectangles are supported
            PreviewMouseRightButtonDown += PreviewMouseRightButtonDownHandler;
            MouseRightButtonUp += MouseRightButtonUpHandler;
            MouseMove += MouseMoveHandler;

            DotGraph.ToolTipContentProvider = NodeTipContentProvider;
        }

        /// <summary>
        ///  Loads a rendered dot graph in -plain format
        /// </summary>
        public void LoadPlain(string path, bool isPath = true)
        {
            GraphLoader g = new GraphLoader(path, "����", isPath);
            DotGraph.Graph = g.Graph;
            DotGraph.ZoomTo(RenderSize);
            UpdateLayout();
            ScrollViewer.ScrollToHorizontalOffset((DotGraph.RenderSize.Width - ScrollViewer.ViewportWidth) / 2);
            ScrollViewer.ScrollToVerticalOffset((DotGraph.RenderSize.Height - ScrollViewer.ViewportHeight) / 2);
            Statistics.Text = g.NodeCount + " nodes, " + g.EdgeCount + " edges, " + g.PointCount + " points";

            UpdateTextBlockPosition();
        }


        /// <summary>
        /// Prints a graph over several pages, if necessary
        /// </summary>
        /// <param name="pd"></param>
        public void Print(PrintDialog pd)
        {
            pd.PrintTicket.PageOrientation = (DotGraph.ActualWidth > DotGraph.ActualHeight) ? PageOrientation.Landscape : PageOrientation.Portrait;
            Size s = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
            GraphPaginator paginator = new GraphPaginator(DotGraph.Graph, s);
            try
            {
                pd.PrintDocument(paginator, "Dot2Wpf");
            }
            catch (PrintSystemException x)
            {
                MessageBox.Show(x.Message, "Dot2Wpf");
            }
        }

        /// <summary>
        /// Fired, when a tooltip over a node is displayed. The handling code has to supply the tooltip content
        /// </summary>
        public event NodeTipEventHandler ShowNodeTip
        {
            add { AddHandler(ShowNodeTipEvent, value); }
            remove { RemoveHandler(ShowNodeTipEvent, value); }
        }

        #region Dragging

        void PreviewMouseRightButtonDownHandler(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(ScrollViewer);
            if (pt.X < ScrollViewer.ViewportWidth && pt.Y < ScrollViewer.ViewportHeight)
            {
                waitForDrag = true;
                dragStart = pt;
                scrollOrigin = new Point(ScrollViewer.HorizontalOffset, ScrollViewer.VerticalOffset);
            }
        }

        void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (waitForDrag && (e.GetPosition(this) - dragStart).Length > 1)
            {
                waitForDrag = false;
                dragging = CaptureMouse();
            }
            if (dragging)
            {
                e.Handled = true;
                Point offset = scrollOrigin - (e.GetPosition(this) - dragStart);
                ScrollViewer.ScrollToHorizontalOffset(offset.X);
                ScrollViewer.ScrollToVerticalOffset(offset.Y);
            }
        }

        void MouseRightButtonUpHandler(object sender, MouseButtonEventArgs e)
        {
            waitForDrag = false;
            if (dragging)
            {
                e.Handled = true;
                dragging = false;
                ReleaseMouseCapture();
            }
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            dragging = false;
            base.OnLostMouseCapture(e);
        }

        private bool waitForDrag;
        private bool dragging;
        Point dragStart;
        Point scrollOrigin;

        #endregion

        #region Zooming

        public void ZoomTo(double zoom)
        {
            if (zoom < 0.07) zoom = 0.07;
            else if (zoom > 7) zoom = 7;
            DotGraph.Zoom = zoom;

            double centerX = ScrollViewer.ViewportWidth / 2;
            double centerY = ScrollViewer.ViewportHeight / 2;

            double offsetX = (ScrollViewer.HorizontalOffset + centerX) / DotGraph.Zoom;
            double offsetY = (ScrollViewer.VerticalOffset + centerY) / DotGraph.Zoom;

            UpdateLayout();

            ScrollViewer.ScrollToHorizontalOffset(offsetX * zoom - centerX);
            ScrollViewer.ScrollToVerticalOffset(offsetY * zoom - centerY);

            UpdateTextBlockPosition();
        }

        [Obsolete]
        public void ZoomTo(string tag)
        {
            DotGraph.SelectNodeTag = tag;

            DrawingVisual node = null;
            foreach (DrawingVisual v in DotGraph.Graph.Children)
            {
                if ((string)v.ReadLocalValue(FrameworkElement.TagProperty) == tag)
                {
                    node = v;
                    break;
                }
            }

            double centerX = ScrollViewer.ViewportWidth / 2;
            double centerY = ScrollViewer.ViewportHeight / 2;

            if(node != null)
            {
                Rect area = node.ContentBounds;
                centerX = area.X + area.Width / 2;
                centerY = area.Y + area.Width / 2;
            }


            double offsetX = (ScrollViewer.HorizontalOffset + centerX) / DotGraph.Zoom;
            double offsetY = (ScrollViewer.VerticalOffset + centerY) / DotGraph.Zoom;

            UpdateLayout();

            ScrollViewer.ScrollToHorizontalOffset(offsetX - centerX);
            ScrollViewer.ScrollToVerticalOffset(offsetY - centerY);

            UpdateTextBlockPosition();
        }
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;

            // Scales and scrolls so, that the visual center keeps in the center
            double zoom = DotGraph.Zoom;
            double centerX = ScrollViewer.ViewportWidth / 2;
            double centerY = ScrollViewer.ViewportHeight / 2;
            double offsetX = (ScrollViewer.HorizontalOffset + centerX) / DotGraph.Zoom;
            double offsetY = (ScrollViewer.VerticalOffset + centerY) / DotGraph.Zoom;

            // zoom the content of the graph element
            zoom = zoom * (1 + e.Delta / 1200.0);
            if (zoom < 0.07) zoom = 0.07;
            else if (zoom > 7) zoom = 7;
            DotGraph.Zoom = zoom;

            // Wait until the ScrollViewer has updated its layout because of the Graph's size changings
            UpdateLayout();

            ScrollViewer.ScrollToHorizontalOffset(offsetX * zoom - centerX);
            ScrollViewer.ScrollToVerticalOffset(offsetY * zoom - centerY);

            UpdateTextBlockPosition();
        }

        #endregion

        #region Faked Layout

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new VoidDelegate(UpdateTextBlockPosition));
        }

        delegate void VoidDelegate();

        /// <summary>
        /// Updates the Textblocks relative to the lower and right scrollbars
        /// </summary>
        void UpdateTextBlockPosition()
        {
            double deltaX = ScrollViewer.ActualWidth - ScrollViewer.ViewportWidth;
            double deltaY = ScrollViewer.ActualHeight - ScrollViewer.ViewportHeight;

            Statistics.Margin = new Thickness(6, 0, 0, 6 + deltaY);
            Copyright.Margin = new Thickness(0, 0, 6 + deltaX, 6 + deltaY);
        }

        #endregion

        object NodeTipContentProvider(object tag)
        {
            NodeTipEventArgs e = new NodeTipEventArgs(ShowNodeTipEvent, this, tag);
            RaiseEvent(e);
            return e.Content;
        }

    }
}