using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using ImageMagick;
using System.Threading;

namespace T6TC
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    using System.Windows.Forms;
    using System.Windows.Input;
    using static T6TC_NEW.Structures;
    using ColorWheel.Controls;
    using ColorWheel.Core;
    using System.Globalization;
    using MessageBox = System.Windows.Forms.MessageBox;
    using Microsoft.Toolkit.Uwp.Notifications;
    using System.Runtime.InteropServices;
    using System.Net;

    public partial class MainWindow : Window
    {

        #region Vars
        public string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filepath, filepath1, uniqueID;

        public int fileCount = 0;

        public long totalSize = 0, total = 0, lastVal = 0, sum = 0;

        public bool logged = false;

        public Process[] processes = Process.GetProcessesByName("t6mpv43");

        public static Palette Palette
        {
            get
            {
                return Palette.Create(new RGBColorWheel(), Colors.White, PaletteSchemaType.Analogous, 1);
            }
        }
        #endregion

        #region Init
        public MainWindow()
        {
            InitializeComponent();
            uniqueID = GetUniqueID.getUniqueID(""); //получение HWID
            DataContext = this;
            if (processes.Length == 0)
            {
                ReloadBO2.Content = "START BO2";
            }
            if (File.Exists(roaming + "/T6TC/cache3"))
            {
                string CID = new StreamReader(System.Net.WebRequest.Create("https://pastebin.com/raw/QSBNSuJG").GetResponse().GetResponseStream()).ReadToEnd();
                string[] CIDS = CID.Replace(Environment.NewLine, " ").Split(' ');
                for (int i = 0; i < CIDS.Length; i++)
                {
                    string cache = File.ReadAllText(roaming + "/T6TC/cache3");
                    if (CIDS[i] == cache || CIDS[i] == cache + "_" + uniqueID) //если совпал введённый полный ID или совпал введенный ник, к которому автоматически добавляется HWID
                    {
                        Skybox.IsEnabled = true;
                        HQTex.IsEnabled = true;
                        Tritium.IsEnabled = true;
                        Misc.IsEnabled = true;
                        HomeGrid.Visibility = Visibility.Visible;
                        LoginGrid.Visibility = Visibility.Hidden;
                        this.Height = this.MinHeight = 307.333;
                        this.MaxHeight = 1080;
                        logged = true;
                        try
                        {
                            filepath1 = File.ReadAllText(roaming + "/T6TC/cache2");
                            filepath = filepath1 + "\\data\\images";
                        }
                        catch
                        {
                            if (logged = true)
                            {
                                MessageBox.Show("This seems like it's your first time running the tool, or something went wrong with your cache. Please press OK to choose your directory.");
                                ChooseDirec();
                            }
                        }
                        //some code here...
                    }
                }
                if (logged == false)    //если ни одного совпадения по логинам
                {
                    System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("You can not use the tool :(", "До связи");
                    if (result == System.Windows.Forms.DialogResult.OK)
                        Close();
                }
            }
            CheckForUpdates();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (logged == false)
            {
                Skybox.IsEnabled = false;
                HQTex.IsEnabled = false;
                Tritium.IsEnabled = false;
                Misc.IsEnabled = false;
                HomeGrid.Visibility = Visibility.Hidden;
                LoginGrid.Visibility = Visibility.Visible;
                this.Height = this.MinHeight = 162;
                this.MaxHeight = 1080;
            }
        }
        #endregion

        #region Main Menu
        private void Direc_Click(object sender, RoutedEventArgs e)
        {
            ChooseDirec();
        }

        async private void ReloadTool_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation anim = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            AllPages.BeginAnimation(OpacityProperty, anim);
            Indicator.BeginAnimation(OpacityProperty, anim);
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(500);
            });
            DoubleAnimation anim2 = new DoubleAnimation();
            anim.From = 1;
            anim.To = 0.5;
            anim.Duration = TimeSpan.FromMilliseconds(20);
            AllPages.BeginAnimation(OpacityProperty, anim2);
            Indicator.BeginAnimation(OpacityProperty, anim2);
        }
        private void ReloadBO2_Click(object sender, RoutedEventArgs e)
        {
            var pathdefault = System.IO.Path.Combine(Directory.GetCurrentDirectory());
            System.Environment.CurrentDirectory = filepath1;
            if (Process.GetProcessesByName("t6mpv43").Length != 1)
            {
                Process.Start(filepath1 + "/t6mpv43.exe");
                ReloadBO2.Content = "RELOAD BO2";
            }
            else
            {
                foreach (Process proc in Process.GetProcessesByName("t6mpv43"))
                {
                    proc.Kill();
                }
                Process.Start(filepath1 + "/t6mpv43.exe");
            }
            System.Environment.CurrentDirectory = pathdefault;
        }

        async private void RestoreAll_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (Directory.Exists(filepath))
                {
                    Directory.Delete(filepath, true);
                    MessageBox.Show("Textures reset to default.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Textures already default.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
        #endregion

        #region Tab Control
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.TabControl) //if this event fired from TabControl then enter
            {
                var selected = AllPages.SelectedItem as TabItem;
                TabItem_OnChanged(selected);
                DoubleAnimation anim = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(200)
                };

                if (selected.Header.ToString() == "HOME")
                {
                    this.Width = this.MinWidth = 460.84;
                    if(logged == true)
                    {
                        this.Height = this.MinHeight = 307.333;
                    }
                    this.MaxWidth = 1920;
                    this.MaxHeight = 1080;
                    HomeGrid.BeginAnimation(OpacityProperty, anim);
                    //rpc.UpdateRPC("Logged in as " + titlecontent, "Editing their CFG.");
                }
                else if (selected.Header.ToString() == "SKYBOX")
                {
                    this.Width = this.MinWidth = 406.5;
                    this.Height = this.MinHeight = 143.833;
                    this.MaxWidth = 1920;
                    this.MaxHeight = 1080;
                    SkyGrid.BeginAnimation(OpacityProperty, anim);
                    //rpc.UpdateRPC("Logged in as " + titlecontent, "It's foggy!");
                }
                else if (selected.Header.ToString() == "HQ TEX")
                {
                    this.Width = this.MinWidth = 406.5;
                    this.Height = this.MinHeight = 181.833;
                    this.MaxWidth = 1920;
                    this.MaxHeight = 1080;
                }
                else if (selected.Header.ToString() == "TRITIUM")
                {
                    this.Width = this.MinWidth = 406.5;
                    this.Height = this.MinHeight = 503.667;
                    this.MaxWidth = 1920;
                    this.MaxHeight = 1080;
                }
                else if (selected.Header.ToString() == "MISC")
                {
                    this.Width = this.MinWidth = 406.5;
                    this.Height = this.MinHeight = 150.167;
                    this.MaxWidth = 1920;
                    this.MaxHeight = 1080;
                }
            }
        }

        private static void TabItem_OnChanged(object sender) //method to update the rectangle that is used for highlighting selected tab on tabControl
        {
            if (System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Window win = System.Windows.Application.Current.Windows[0];
                Point relativePoint = (sender as TabItem).TransformToAncestor(win).Transform(new Point(0, 0));
                double oldX = (win as MainWindow).Indicator.TransformToAncestor(win).Transform(new Point(0, 0)).X;
                double newX = relativePoint.X; // + ((TabItem)((FrameworkElement)sender).TemplatedParent).ActualWidth / 2;
                ThicknessAnimation anim = new ThicknessAnimation
                {
                    From = new Thickness(oldX, relativePoint.Y - -7, 0, 0),
                    To = new Thickness(newX, relativePoint.Y - -7, 0, 0),
                    Duration = TimeSpan.FromMilliseconds(200)
                };

                DoubleAnimation anim2 = new DoubleAnimation
                {
                    From = (sender as TabItem).ActualWidth,
                    To = (sender as TabItem).ActualWidth,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                (win as MainWindow).Indicator.BeginAnimation(TabItem.MarginProperty, anim);
                (win as MainWindow).Indicator.BeginAnimation(TabItem.WidthProperty, anim2);
            }
        }

        #endregion

        #region Skybox
        private void ImportSky_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BO2 Skybox|*.IWI";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                if (Directory.Exists(filepath))
                {

                }
                else
                {
                    Directory.CreateDirectory(filepath);
                }
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory());
                File.Copy(ofd.FileName, @"Dependencies\sky\sky.iwi", true);
                Thread.Sleep(500);
                System.Environment.CurrentDirectory = @"Dependencies\sky";
                Process.Start("runme.bat");
                foreach (var file in Directory.GetFiles(path, "*.iwi", SearchOption.AllDirectories))
                    File.Copy(file, Path.Combine(filepath, System.IO.Path.GetFileName(file)), true);
                if (File.Exists(Path.Combine(@"dependencies\sky", "sky.iwi"))) 
                {
                    File.Delete(Path.Combine("sky.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "sky.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "sky.iwi"));
                }
                System.Windows.Forms.MessageBox.Show("Finished.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Environment.CurrentDirectory = path;
            }
        }

        private void RestoreSky_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Path.Combine(filepath, "skybox_mp_bridge_ft.iwi")))
            {
                File.Delete(Path.Combine(filepath, "skybox_mp_bridge_ft.iwi"));
                if (File.Exists(Path.Combine(filepath, "skybox_mp_carrier_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_carrier_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_castaway_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_castaway_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_concert_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_concert_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_dig_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_dig_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_downhill_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_downhill_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_drone_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_drone_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_express_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_express_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_frostbite_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_frostbite_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_hijacked_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_hijacked_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_hydro_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_hydro_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_magma_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_magma_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_meltdown_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_meltdown_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_mirage_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_mirage_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_nightclub_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_nightclub_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_nuketown_2020_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_nuketown_2020_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_overflow_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_overflow_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_paintball_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_paintball_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_raid_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_raid_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_skate_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_skate_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_skate.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_skate.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "mp_skate_ft.iwii"))) 
                {
                    File.Delete(Path.Combine(filepath, "mp_skate_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_slums_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_slums_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_socotra_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_socotra_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_studio_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_studio_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_takeoff_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_takeoff_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_turbine_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_turbine_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_uplink_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_uplink_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_vertigo_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_vertigo_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "mp_castaway_04_vue_skybox_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "mp_castaway_04_vue_skybox_ft.iwi"));
                }
                if (File.Exists(Path.Combine(filepath, "skybox_mp_pod_ft.iwi"))) 
                {
                    File.Delete(Path.Combine(filepath, "skybox_mp_pod_ft.iwi"));
                    MessageBox.Show("Finished.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Skybox doesn't exist.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Preset_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Textures

        private void RestoreTex_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(filepath))
            {
                Directory.Delete(filepath, true);
                MessageBox.Show("Textures reset to default.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Textures already default.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable4K_Click_1(object sender, RoutedEventArgs e)
        {
            this.Height = 228;
            this.Dispatcher.Invoke(() =>
            {
                StatusLabel.Content = "Extracting...";
                string fourk1 = @"dependencies\fourk1.zip";
                string fourk2 = @"dependencies\fourk2.zip";
                string fourk5 = @"dependencies\fourk5.zip";
                string fourk4 = @"dependencies\fourk4.zip";
                StatusLabel.Content = "Extracting... (fourk1.zip)";
                ReadAndExtract(fourk1, filepath);
                StatusLabel.Content = "Extracting... (fourk2.zip)";
                ReadAndExtract(fourk2, filepath);
                StatusLabel.Content = "Extracting... (fourk4.zip)";
                ReadAndExtract(fourk4, filepath);
                StatusLabel.Content = "Extracting... (fourk5.zip)";
                ReadAndExtract(fourk5, filepath);
                    new ToastContentBuilder()
                        .AddText("T6TC", hintMaxLines: 1)
                        .AddText("4K Textures have finished extracting!")
                .Show();
                ExtractProgress.Value = 0;
                StatusLabel.Content = "";
                this.Height = 181.833;
            });
        }
        private void EnableWinter_Click_1(object sender, RoutedEventArgs e)
        {
            this.Height = 228;
            this.Dispatcher.Invoke(() =>
            {
                string winter1 = @"dependencies\winter1.zip";
                string winter2 = @"dependencies\winter2.zip";
                StatusLabel.Content = "Extracting... (winter1.zip)";
                ReadAndExtract(winter1, filepath);
                StatusLabel.Content = "Extracting... (winter2.zip)";
                ReadAndExtract(winter2, filepath);
                new ToastContentBuilder()
                .AddText("T6TC", hintMaxLines: 1)
                .AddText("Winter Textures have finished extracting!")
                .Show();
                ExtractProgress.Value = 0;
                StatusLabel.Content = "";
                this.Height = 181.833;
            });
        }
        #endregion

        #region Tritium
        private void styleColorWheel_ColorsUpdated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                styleColorWheel.Palette.Colors[0].Brightness255 = 255;
                ColorWheelControl wheel = (ColorWheelControl)sender;
                DoubleColor color = wheel.Palette.Colors[0].DoubleColor;
                flColor ColorVector = new flColor()
                {
                    r = (float)(color.R),
                    g = (float)(color.G),
                    b = (float)(color.B)

                };
                int redvalue = (int)ColorVector.r;
                int greenvalue = (int)ColorVector.g;
                int bluevalue = (int)ColorVector.b;
                string red = redvalue.ToString();
                string green = greenvalue.ToString();
                string blue = bluevalue.ToString();
                string hexValue = "#" + redvalue.ToString("X2") + greenvalue.ToString("X2") + bluevalue.ToString("X2");
                SolidColorBrush MyBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(hexValue);
                this.Resources["DefaultColor"] = MyBrush;
                SolidColorBrush border = (SolidColorBrush)new BrushConverter().ConvertFromString(hexValue);
                this.Resources["BorderColor"] = border;
                hexcode.Text = hexValue;
                Tritium.Foreground = MyBrush;
                rgbcode.Text = red + ", " + green + ", " + blue;
            });

        }

        private void hexcode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string hexString = hexcode.Text;
                int r, g, b = 0;
                if (hexcode.Text.Contains("#") == true)
                {
                    string result = hexString.Substring(hexString.LastIndexOf('#') + 1);
                    r = int.Parse(result.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                    g = int.Parse(result.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                    b = int.Parse(result.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                    string ra = r.ToString();
                    string ga = g.ToString();
                    string ba = b.ToString();
                    rgbcode.Text = ra + ", " + ga + ", " + ba;
                    SolidColorBrush MyBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(hexString);
                    this.Resources["DefaultColor"] = MyBrush;
                    SolidColorBrush border = (SolidColorBrush)new BrushConverter().ConvertFromString(hexString);
                    this.Resources["BorderColor"] = border;
                }
                else
                {
                    SolidColorBrush MyBrush = (SolidColorBrush)new BrushConverter().ConvertFromString($"#{hexcode.Text}");
                    this.Resources["DefaultColor"] = MyBrush;
                    SolidColorBrush border = (SolidColorBrush)new BrushConverter().ConvertFromString($"#{hexcode.Text}");
                    this.Resources["BorderColor"] = border;
                    hexcode.Text = $"#{hexcode.Text}";
                    string result = hexString.Substring(hexString.LastIndexOf('#') + 1);
                    r = int.Parse(result.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                    g = int.Parse(result.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                    b = int.Parse(result.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                    string ra = r.ToString();
                    string ga = g.ToString();
                    string ba = b.ToString();
                    Tritium.Foreground = MyBrush;
                    rgbcode.Text = ra + ", " + ga + ", " + ba;
                }
            }
        }

        private void rgbcode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                string[] splitArray = rgbcode.Text.Split(',');
                string hexValue = "#" + (Int32.Parse(splitArray[0])).ToString("X2") + (Int32.Parse(splitArray[1])).ToString("X2") + (Int32.Parse(splitArray[2])).ToString("X2");
                hexcode.Text = hexValue;
                SolidColorBrush MyBrush = (SolidColorBrush)new BrushConverter().ConvertFromString(hexValue);
                this.Resources["DefaultColor"] = MyBrush;
                SolidColorBrush border = (SolidColorBrush)new BrushConverter().ConvertFromString(hexValue);
                this.Resources["BorderColor"] = border;
                Tritium.Foreground = MyBrush;
            }
        }

        private void AddTrit_Click(object sender, RoutedEventArgs e)
        {
            string sourceDir = Directory.GetCurrentDirectory() + "/dependencies/tritium/pngs";
            string orig = sourceDir + "/original/~~-gmtl_t6_attach_tritium_red~74df00e5.png";
            string orig2 = sourceDir + "/original/~~-gmtl_t6_attach_tritium_wht~74df00e5.png";
            string orig3 = sourceDir + "/original/~-gmtl_t6_attach_tritium_grn_col.png";
            string orig4 = sourceDir + "/original/~-gmtl_t6_attach_tritium_red_col.png";
            string orig5 = sourceDir + "/original/~-gmtl_t6_attach_tritium_wht_col.png";
            string orig6 = sourceDir + "/original/mtl_t6_attach_tritium_blu_glo.png";
            string orig7 = sourceDir + "/original/mtl_t6_attach_tritium_grn_glo.png";
            string orig8 = sourceDir + "/original/mtl_t6_attach_tritium_red_glo.png";
            string orig9 = sourceDir + "/original/mtl_t6_attach_tritium_red_text.png";
            string new1 = sourceDir + "/~~-gmtl_t6_attach_tritium_red~74df00e5.png";
            string new2 = sourceDir + "/~~-gmtl_t6_attach_tritium_wht~74df00e5.png";
            string new3 = sourceDir + "/~-gmtl_t6_attach_tritium_grn_col.png";
            string new4 = sourceDir + "/~-gmtl_t6_attach_tritium_red_col.png";
            string new5 = sourceDir + "/~-gmtl_t6_attach_tritium_wht_col.png";
            string new6 = sourceDir + "/mtl_t6_attach_tritium_blu_glo.png";
            string new7 = sourceDir + "/mtl_t6_attach_tritium_grn_glo.png";
            string new8 = sourceDir + "/mtl_t6_attach_tritium_red_glo.png";
            string new9 = sourceDir + "/mtl_t6_attach_tritium_red_text.png";
            var color1 = hexcode.Text;
            using (MagickImage images = new MagickImage())
            {
                images.Read(orig);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new1);
                }
                images.Read(orig2);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new2);
                }
                images.Read(orig3);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new3);
                }
                images.Read(orig4);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new4);
                }
                images.Read(orig5);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new5);
                }
                images.Read(orig6);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new6);
                }
                images.Read(orig7);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new7);
                }
                images.Read(orig8);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new8);
                }
                images.Read(orig9);
                {
                    images.Colorize(new MagickColor(color1), new Percentage(50));
                    images.Write(new9);
                }
                if (Directory.Exists(filepath))
                {
                    foreach (var file in Directory.GetFiles(sourceDir, "*.png", SearchOption.TopDirectoryOnly))
                        File.Copy(file, Path.Combine(filepath, System.IO.Path.GetFileName(file)), true);
                    MessageBox.Show("Tritium Added", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Directory.CreateDirectory(filepath);
                    foreach (var file in Directory.GetFiles(sourceDir, "*.png", SearchOption.TopDirectoryOnly))
                        File.Copy(file, Path.Combine(filepath, System.IO.Path.GetFileName(file)), true);
                    MessageBox.Show("Tritium Added", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void RemoveTrit_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(filepath + "\\~~-gmtl_t6_attach_tritium_red~74df00e5.png");
            File.Delete(filepath + "\\~~-gmtl_t6_attach_tritium_wht~74df00e5.png");
            File.Delete(filepath + "\\~-gmtl_t6_attach_tritium_grn_col.png");
            File.Delete(filepath + "\\~-gmtl_t6_attach_tritium_red_col.png");
            File.Delete(filepath + "\\~-gmtl_t6_attach_tritium_wht_col.png");
            File.Delete(filepath + "\\mtl_t6_attach_tritium_blu_glo.png");
            File.Delete(filepath + "\\mtl_t6_attach_tritium_grn_glo.png");
            File.Delete(filepath + "\\mtl_t6_attach_tritium_red_glo.png");
            File.Delete(filepath + "\\mtl_t6_attach_tritium_red_text.png");
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF00A7E6"));
            this.Resources["DefaultColor"] = mySolidColorBrush;
            this.Resources["BorderColor"] = mySolidColorBrush;
            MessageBox.Show("Tritium Removed", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Misc

        void ChooseDirec()
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!Directory.Exists(roaming + "/T6TC")) Directory.CreateDirectory(roaming + "/T6TC");
                StreamWriter writer = new StreamWriter(roaming + "/T6TC/cache2");
                writer.Write(fbd.SelectedPath);
                writer.Close();
                filepath1 = File.ReadAllText(roaming + "/T6TC/cache2");
                filepath = filepath1 + "\\data\\images";
            }
        }



        public void ReadAndExtract(string openPath, string savePath)
        {
            try
            {
                fileCount = 0;
                Ionic.Zip.ZipFile myZip = new Ionic.Zip.ZipFile();
                myZip = Ionic.Zip.ZipFile.Read(openPath);
                myZip.Password = "EinLOL";
                foreach (var entry in myZip)
                {
                    fileCount++;
                    totalSize += entry.UncompressedSize;
                }
                ExtractProgress.Maximum = (Int32)totalSize;
                myZip.ExtractProgress += new EventHandler<Ionic.Zip.ExtractProgressEventArgs>(myZip_ExtractProgress);
                myZip.ExtractAll(savePath, ExtractExistingFileAction.OverwriteSilently);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        void myZip_ExtractProgress(object sender, Ionic.Zip.ExtractProgressEventArgs e)
        {

            System.Windows.Forms.Application.DoEvents();
            if (total != e.TotalBytesToTransfer)
            {
                sum += total - lastVal + e.BytesTransferred;
                total = e.TotalBytesToTransfer;
            }
            else
                sum += e.BytesTransferred - lastVal;

            lastVal = e.BytesTransferred;

            ExtractProgress.Value = (Int32)sum;
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        #endregion

        #region Login
        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Login")
            {
                LoginBox.Text = "";
                LoginBox.Foreground = Brushes.White;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string CID = new StreamReader(System.Net.WebRequest.Create("https://pastebin.com/raw/QSBNSuJG").GetResponse().GetResponseStream()).ReadToEnd();
            string[] CIDS = CID.Replace(Environment.NewLine, " ").Split(' ');
            string ID = GetUniqueID.getUniqueID(""); //получение HWID
            for (int i = 0; i < CIDS.Length; i++)

            {
                if (CIDS[i] == LoginBox.Text || CIDS[i] == LoginBox.Text + "_" + ID) //если совпал введённый полный ID или совпал введенный ник, к которому автоматически добавляется HWID
                {
                    string AccountName = EnterName.Text;
                    LoginButton.Content = "Loading...";
                    //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
                    // string ok = System.Convert.ToBase64String(plainTextBytes);
                    string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string specificFolder = roaming + "/T6TC/cache3";
                    if (!Directory.Exists(roaming + "/T6TC")) Directory.CreateDirectory(roaming + "/T6TC");
                    using (StreamWriter writer = new StreamWriter(specificFolder))
                        writer.Write(LoginBox.Text);
                    Skybox.IsEnabled = true;
                    HQTex.IsEnabled = true;
                    Tritium.IsEnabled = true;
                    Misc.IsEnabled = true;
                    HomeGrid.Visibility = Visibility.Visible;
                    LoginGrid.Visibility = Visibility.Hidden;
                    this.Height = this.MinHeight = 307.333;
                    this.MaxHeight = 1080;
                    logged = true;
                    MessageBox.Show("This seems like it's your first time running the tool, or something went wrong with your cache. Please press OK to choose your directory.");
                    ChooseDirec();
                    //some code here...
                }
            }
            if (logged == false)    //если ни одного совпадения по логинам
            {
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("You can not use the tool :(", "До связи");
                if (result == System.Windows.Forms.DialogResult.OK)
                    Close();
            }
        }

        private void GetID_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RemoveWM_Click(object sender, RoutedEventArgs e)
        {
            string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filepath = File.ReadAllText(roaming + "/T6TC/cache2");
            File.Copy(@"dependencies\ExtendedConsole.Red32n", filepath + "\\Plugins\\ExtendedConsole.Red32n", true);
            MessageBox.Show("Watermark removed.", "T6TC", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GetID_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Height == 226.5)
            {
                this.Height = 162;
            }
            else
            {
                this.Height = 226.5;
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
                //Copy.Visibility = Visibility.Hidden;

            }
            else if (EnterName.Text == " ")
            {
                ID1.Text = "";
                //Copy.Visibility = Visibility.Hidden;
            }
            else
            {
                string AccountName = EnterName.Text;
                ID1.Text = (AccountName + "_" + uniqueID);
            }
        }
        public string download;
        void CheckForUpdates()
        {
            string CID = new StreamReader(System.Net.WebRequest.Create("https://pastebin.com/raw/4SNJCZpR").GetResponse().GetResponseStream()).ReadToEnd();
            string[] CIDS = CID.Replace(Environment.NewLine, " ").Split(' ');
            var Upd = CID.Substring(CID.LastIndexOf('-') + 1);
            for (int i = 0; i < CIDS.Length; i++)
            {
                int index = CIDS[0].IndexOf('-');
                if (index > 0)
                {
                    string thing = CIDS[0].Substring(0, index);
                    if (thing == "v0.9.8")
                    {
                        return;
                    }
                    else
                    {
                        vers.IsEnabled = true;
                        vers.Foreground = System.Windows.Media.Brushes.Red;
                        vers.Cursor = System.Windows.Input.Cursors.Hand;
                        download = Upd.ToString();
                    }
                }
            }
        }
        private void vers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WebClient webClient = new WebClient();
            Process.Start(download);
        }
        #endregion
    }
}
