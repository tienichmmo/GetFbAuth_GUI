using FluentUI;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GetFbAuth_LdPlayerGUI.Models;
using System.IO;
using System.Diagnostics;

namespace GetFbAuth_LdPlayerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FabricWindow
    {
        #region Property
        public List<string> Devices = new List<string>();
        public Enums.FbAuthEnum.FBAppTypes FBAppType {  get; set; }
        private const string notfoundFb = "not found fb";
        private string SaveFile { get; set; }
        private DatagridModel NotFoundFb(string device, int row)
        {
            return new DatagridModel()
            {
                DeviceId = device,
                FbCookie = notfoundFb,
                Fbid = notfoundFb,
                FbToken = notfoundFb,
                NumberRow = row
            };
        }
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromRgb(32, 32, 32));
        }
        #region UI Execute
        private void btnreset_Click(object sender, RoutedEventArgs e)
        {
            Devices = AdbServices.GetListDevices();
            if(Devices.Count <= 0)
            {
                MessageBox.Show("List devices trống !", "Devices List is null");
                return;
            }
            SaveFile = AppDomain.CurrentDomain.BaseDirectory + $"result\\result_{Utilities.RandomStr(4)}.txt";
            dataGrid.Items.Clear();
            lbDevicesCount.Text = "Devices " + Devices.Count.ToString();
            int row = 0;
            foreach(string device in Devices)
            {
                DatagridModel datagridModel = new DatagridModel()
                {
                    NumberRow = row+=1,
                    DeviceId = device
                };
                string _SessionFile = $"sess\\{Utilities.RandomStr(7) + ".txt"}";
                string _CmdCommandGetAuth = AdbServices.CmdQuery(device, FBAppType) + $" {_SessionFile}";
                AdbServices.ExecuteCMD(_CmdCommandGetAuth);
                if (File.Exists(_SessionFile))
                {
                    string _Result = File.ReadAllText(_SessionFile);
                    CookieModel cookieModel = new CookieModel(_Result, FBAppType);
                    if (!string.IsNullOrEmpty(cookieModel.FullCookie) || !string.IsNullOrEmpty(cookieModel.Token))
                    {
                        datagridModel.FbCookie = cookieModel.FullCookie;
                        datagridModel.Fbid = cookieModel.uid;
                        datagridModel.FbToken = cookieModel.Token;
                        File.AppendAllText(SaveFile, $"{cookieModel.uid}|{cookieModel.Token}|{cookieModel.FullCookie}\r\n");
                    }
                    else
                    {
                        datagridModel = NotFoundFb(device, row);
                    }
                    DeleteFile(_SessionFile);
                }
                else
                {
                    datagridModel = NotFoundFb(device, row);
                }
                dataGrid.Items.Add(datagridModel);
            }   
                
        }
        #endregion
        #region Methods
        private void DeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch { }
        }
        #endregion

        private void cbbFbAppType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FBAppType = cbbFbAppType.SelectedIndex == 0 ? Enums.FbAuthEnum.FBAppTypes.Fb
                : Enums.FbAuthEnum.FBAppTypes.FbLite;
        }
        private void cbbadbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdbServices.Adb = cbbadbType.SelectedIndex == 0 ? Enums.FbAuthEnum.AdbTypes.Normal : Enums.FbAuthEnum.AdbTypes.Nox;
        }
        private void FabricWindow_Activated(object sender, EventArgs e)
        {
            cbbFbAppType.SelectedIndex = 0;
            Uri fileUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/images/AppIcon.ico");
            imgLogo.Source = new BitmapImage(fileUri);
            this.Icon = new BitmapImage(fileUri);
            cbbadbType.SelectedIndex = 0;
        }

        private void btnGit_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/msc0d3",
                UseShellExecute = true
            });
        }

        private void btnviewResult_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(SaveFile))
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(SaveFile)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            
        }
    }
}
