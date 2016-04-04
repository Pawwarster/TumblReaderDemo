using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TumblReader.Helper
{
    public class ScrollViewerMonitor
    {
        public static DependencyProperty AtEndCommandProperty = DependencyProperty.RegisterAttached("AtEndCommand", typeof(ICommand), typeof(ScrollViewerMonitor), new PropertyMetadata(new PropertyChangedCallback(ScrollViewerMonitor.OnAtEndCommandChanged)));

        static ScrollViewerMonitor()
        {
        }

        public static ICommand GetAtEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ScrollViewerMonitor.AtEndCommandProperty);
        }

        public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ScrollViewerMonitor.AtEndCommandProperty, (object)value);
        }

        public static void OnAtEndCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)d;
            if (frameworkElement == null)
                return;
            frameworkElement.Loaded -= new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
            frameworkElement.Loaded += new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
        }

        private static void ElementLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= new RoutedEventHandler(ScrollViewerMonitor.ElementLoaded);
            ScrollViewer scrollViewer = (ScrollViewer)element;

            DependencyPropertyListener propertyListener = new DependencyPropertyListener();
            propertyListener.Changed += (EventHandler<BindingChangedEventArgs>)delegate
            {
                if (scrollViewer.ScrollableHeight == 0) return;
                if ((scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset) > 0.15)
                    return;

                ICommand atEndCommand = ScrollViewerMonitor.GetAtEndCommand((DependencyObject)element);
                if (atEndCommand == null)
                    return;
                atEndCommand.Execute((object)null);
            };
            Binding binding = new Binding("VerticalOffset")
            {
                Source = (object)scrollViewer
            };
            propertyListener.Attach((FrameworkElement)scrollViewer, binding);
        }
    }
}
