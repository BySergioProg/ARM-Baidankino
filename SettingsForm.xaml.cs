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
using S7.Net;

namespace ARM_Baidankino
{
    /// <summary>
    /// Логика взаимодействия для SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public class AlarmLevels
        {
            public int LevelE1 { get; set; }
            public int LevelE2 { get; set; }
        }
        public AlarmLevels alarmLevels = new AlarmLevels();
        public PlcSetup plcSetup = new PlcSetup();
        Logger logger = new Logger();
        public MainWindow mainWindow;
        public SettingsForm(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            if (mainWindow.formdata.UserLevel==0)
            {
                UpSector.IsEnabled = true;
            }
            else
            {
                UpSector.IsEnabled = false;
            }
            LoadData();
         
        }
        private void LoadData()
        {
            try
            {
                string Ip = Properties.Settings.Default.Ip;
                using (var plc = new Plc(CpuType.S71200, Ip, 0, 1))
                {
                    plc.ReadTimeout = 2000;
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        plc.ReadClass(plcSetup, 4);
                    }
                    plc.Close();
                }
                alarmLevels.LevelE1 = Properties.Settings.Default.AlarmLevelE1;
                alarmLevels.LevelE2 = Properties.Settings.Default.AlarmLevelE2;
                UpSector.DataContext = plcSetup;
                DownSector.DataContext = alarmLevels;
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }//обработчик событий на ошибку
        }

        private void SaveData(object sender, RoutedEventArgs e)
        {
            try
            {
                string Ip = Properties.Settings.Default.Ip;
                using (var plc = new Plc(CpuType.S71200, Ip, 0, 1))
                {
                    plc.ReadTimeout = 2000;
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        plc.WriteClassAsync(plcSetup, 4);
                    }
                    plc.Close();
                }
                Properties.Settings.Default.AlarmLevelE1 = alarmLevels.LevelE1;
                Properties.Settings.Default.AlarmLevelE2 = alarmLevels.LevelE2;
                Properties.Settings.Default.Save();
                LoadData();
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }//обработчик событий на ошибку
        }
    }
}
