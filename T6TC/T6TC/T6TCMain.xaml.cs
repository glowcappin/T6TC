using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace T6TC
{
    /// <summary>
    /// Interaction logic for T6TCMain.xaml
    /// </summary>
    public partial class T6TCMain : Window
    {
        public T6TCMain()
        {
            InitializeComponent();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush = (SolidColorBrush)(new
            BrushConverter().ConvertFrom("#FF00A7E6"));
            if (skybox.IsSelected)
            {
                hqtex.Foreground = Brushes.DarkGray;
                tritium.Foreground = Brushes.DarkGray;
                misc.Foreground = Brushes.DarkGray;
                skybox.Foreground = mySolidColorBrush;
                this.Height = 141;
                this.Width = 339;   
            }
            else if (hqtex.IsSelected)
            {
                tritium.Foreground = Brushes.DarkGray;
                misc.Foreground = Brushes.DarkGray;
                skybox.Foreground = Brushes.DarkGray;
                hqtex.Foreground = mySolidColorBrush;
                this.Width = 339;
                this.Height = 180;
            }
            else if (tritium.IsSelected)
            {
                misc.Foreground = Brushes.DarkGray;
                hqtex.Foreground = Brushes.DarkGray;
                skybox.Foreground = Brushes.DarkGray;
                tritium.Foreground = mySolidColorBrush;
                this.Width = 329;
                this.Height = 460;
            }
            else if (misc.IsSelected)
            {
                tritium.Foreground = Brushes.DarkGray;
                hqtex.Foreground = Brushes.DarkGray;
                skybox.Foreground = Brushes.DarkGray;
                misc.Foreground = mySolidColorBrush;
            }
            else if (home.IsSelected)
            {
                this.Close();
            }
        }
    }
}
