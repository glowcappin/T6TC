using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace T6TC_NEW
{
    public class SolidColorAnimation : ColorAnimation
    {
        public SolidColorBrush ToBrush
        {
            get { return To == null ? null : new SolidColorBrush(To.Value); }
            set { To = value?.Color; }
        }
    }
    public static class PropertyChange
    {
        public static RoutedEvent PropertyChangeEvent = EventManager.RegisterRoutedEvent("PropertyChangeEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PropertyChange));

        public static void AddPropertyChangeEventHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(PropertyChange.PropertyChangeEvent, handler);
            }
        }

        public static void RemovePropertyChangeEventHandler(DependencyObject d, RoutedEventHandler handler)
        {
            var uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(PropertyChange.PropertyChangeEvent, handler);
            }
        }
    }

    partial class Style
    {
        public Style()
        {
            InitializeComponent();
        }

        private void StatusBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                ((Window)((FrameworkElement)sender).TemplatedParent).DragMove();
            }
            e.Handled = true;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox t = ((TextBox)((FrameworkElement)sender).TemplatedParent);
            t.Clear();
            t.ToolTip = null;

        }
    }
}
