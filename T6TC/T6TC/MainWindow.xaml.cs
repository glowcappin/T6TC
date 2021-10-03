using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace T6TC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (this.Height == 196.333)
            {
                this.Height = 125.97;
            }
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Login")
            {
                LoginBox.Text = "";
                LoginBox.Foreground = Brushes.White;
            }
        }

        private void GetID_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Height == 196.333)
            {
                this.Height = 125.97;
            }
        }

        private void EnterName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (EnterName.Text == "Enter a name")
            {
                EnterName.Text = "";
                EnterName.Foreground = Brushes.White;
            }
        }

        private void EnterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EnterName.Text == "Enter a name")
            {

            }
            else if (EnterName.Text == "")
            {
                ID1.Text = "";
                Copy.Visibility = Visibility.Hidden;

            }
            else if (EnterName.Text == " ")
            {
                ID1.Text = "";
                Copy.Visibility = Visibility.Hidden;
            }
            else
            {
                var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
                ManagementObjectCollection mbsList = mbs.Get();
                string id = "";
                string AccountName = EnterName.Text;
                foreach (ManagementObject mo in mbsList)
                {
                    id = mo["ProcessorId"].ToString();
                    string myUniqueID = id;
                    string HWID = myUniqueID;
                    string value = HWID;
                    ID1.Text = (AccountName + "_" + myUniqueID);
                    Copy.Visibility = Visibility.Visible;
                }
            }
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Copy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ID1.Text == "")
            {
                System.Windows.MessageBox.Show("No ID to copy!");
            }
            else
            {
                System.Windows.Clipboard.Clear();
                System.Windows.Clipboard.SetText(ID1.Text);
            }

        }

        private void GetID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Height == 196.333)
            {
                this.Height = 125.97;
            }
            else
            {
                this.Height = 196.333;
            }
        }

        public void Login()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                string username = LoginBox.Text;

                string myUniqueID = id;

                string HWID = myUniqueID;

                string value = username + "_" + HWID;

                string value2 = username;

                string value3 = username + "_" + HWID + "*";
                WebClient webClient = new WebClient();

                if (LoginBox.Text == "Login")
                {
                    LoginButton.Content = "Invalid ID";
                }
                else if (LoginBox.Text == "")
                {
                    LoginButton.Content = "Invalid ID";
                }
                else
                {
                    bool success = webClient.DownloadString("https://git.io/JgeKe").Contains(value);
                    if (success)
                    {
                        LoginButton.Content = "Loading...";
                        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
                        string ok = System.Convert.ToBase64String(plainTextBytes);
                        string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        string specificFolder = roaming + "/T6TC/cache";
                        if (!Directory.Exists(roaming + "/T6TC")) Directory.CreateDirectory(roaming + "/T6TC");
                        using (StreamWriter writer = new StreamWriter(specificFolder))
                            writer.Write(ok);
                        this.WindowState = WindowState.Minimized;
                        T6TCMenu win2 = new T6TCMenu();
                        win2.Show();
                        win2.Top = this.Top;
                        win2.WindowState = WindowState.Normal;
                        win2.Left = this.Left;
                        this.Close();
                    }
                    else
                    {
                        bool fruad = webClient.DownloadString("https://git.io/JgeKe").Contains(value2);
                        bool banned = webClient.DownloadString("https://git.io/JgeKe").Contains(value3);
                        if (fruad)
                        {
                            LoginButton.Content = "Wrong HWID ¯\\_(ツ)_//¯";
                        }
                        else if (banned)
                        {
                            LoginButton.Content = "Banned LOL";
                        }
                        else
                        {
                            LoginButton.Content = "Sorry, doesn't look like you have permission to use T6TC.";
                        }
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }
    }
}


