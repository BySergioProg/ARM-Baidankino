using S7.Net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ARM_Baidankino
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

        [DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
        private static extern int GetMenuItemCount(IntPtr hmenu);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

        [DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
        private static extern int DrawMenuBar(IntPtr hwnd);

        private const int MF_BYPOSITION = 0x0400;
        private const int MF_DISABLED = 0x0002;


        public List<GradTable> E1GradTable=new List<GradTable>();
        public List<GradTable> E2GradTable = new List<GradTable>();
        public PlcData plcData = new PlcData();
        private DispatcherTimer ordersWorker = new DispatcherTimer();
        private DispatcherTimer ordersWorker2 = new DispatcherTimer();
        public bool ReadBusy;
        public Formdata formdata = new Formdata();
        public int PrevTime;
        public bool FirstRecordToDb;
        public int StatusOil;
        public int LinkQuality;
        public DateTime TimeToSaveGas;
        Logger logger = new Logger();
        public MainWindow()
        {
            FirstRecordToDb = true;
            InitializeComponent();
            this.SourceInitialized += new EventHandler(Window1_SourceInitialized);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);
            logger.AddEvent(false, "Программа Запущена");
            LinkQuality = 2;
            TimeToSaveGas = DateTime.Now;
            PrevTime = DateTime.Now.Hour;
            E1GradTable= InitGradTable(25, E1GradTable);
            E2GradTable= InitGradTable(50, E2GradTable);
            ordersWorker.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ordersWorker.Tick += ReadPlc;
         
            ordersWorker.Start();
            ordersWorker2.Interval = new TimeSpan(0, 0, 0, 60, 0);
            ordersWorker2.Tick += SaveGazData;
            ordersWorker2.Start();
            this.DataContext = formdata;
         
        }
        void Window1_SourceInitialized(object sender, EventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            IntPtr windowHandle = helper.Handle; //Get the handle of this window
            IntPtr hmenu = GetSystemMenu(windowHandle, 0);
            int cnt = GetMenuItemCount(hmenu);
            //remove the button
            RemoveMenu(hmenu, cnt - 1, MF_DISABLED | MF_BYPOSITION);
            //remove the extra menu line
            RemoveMenu(hmenu, cnt - 2, MF_DISABLED | MF_BYPOSITION);
            DrawMenuBar(windowHandle); //Redraw the menu bar
        }
        private void SaveGazData(object sender, EventArgs e)//Сохранение суточных показаний счетчика газа
        {
            try
            {
                var Hour = 23;
                var Minute = 59;
                if ((Hour == DateTime.Now.Hour) &&
                  (Minute == DateTime.Now.Minute))
                {
                    string SQL_s = Properties.Settings.Default.Sql;
                    string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        DateTime date = DateTime.Now;
                        string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                        string sqlExpression = $"INSERT INTO GazDay (Date, Value) VALUES ('{date_str}', {formdata.Count_day})";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
                
            }


        }
        private List<GradTable> InitGradTable(int Vol, List<GradTable> gradTables)
        {
           
            string Patch = $@"GradTable/Tank{Vol}.csv";
            string[] Values = null;
            string[] ReadData = File.ReadAllLines(Patch);
            for (int i = 0; i < ReadData.Length; i++)
            {
                if (!String.IsNullOrEmpty(ReadData[i]))
                {
                    Values = ReadData[i].Split(';');
                    Values[1] = Values[1].Replace(",", ".");
                    gradTables.Add(new GradTable {
                        HeightOil = Convert.ToInt32(Values[0]),
                        VoliumeOil = Convert.ToDouble(Values[1])
                    });
                }
            }
            return gradTables;
        }
        #region ReadDataFromPlc
        private void ReadPlc(object sender, EventArgs e)//Запуск чтения данных с контроллера асинхронно
        {
            Task.Run(() =>
             {
                 if (!ReadBusy)
                 {
                     Thread myThread = new Thread(new ThreadStart(ReadS7));
                     myThread.IsBackground = true;
                     myThread.Start(); 
                     Thread.Sleep(10000);
                     if (myThread.IsAlive)
                     {
                         logger.AddEvent(true, "Зависание потока чтения данных");
                    
                         myThread.Abort();
                     }
                 }
             }
            );

 
            
           //if (!ReadBusy)
           //{

          //  Task.Run(() =>  ReadS7());
              
           // }
        }
        private bool CheckAlarms()//Проверка есть ли непринятые тревоги
        {
            try
            {
                bool result = false;
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = $"SELECT * FROM Alarms WHERE Curr=1 and AlarmType='Тревога' and AdoptUser  IS NULL  ";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        result = true;
                    }


                    connection.Close();
                }

                return result;
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message);
                return false;
            }

        }
        private void ReadS7()//Чтение данных с контроллера
        {
            try
            {
                if (LinkQuality > 0) { new SaveErrors(false, "PLC", "Внимание", "Отсутствие связи");
                    formdata.PlcAlarm = "Hidden";
                }
                else { new SaveErrors(true, "PLC", "Внимание", "Отсутствие связи");
                    formdata.PlcAlarm = "Visible";
                }
                if (CheckAlarms())
                {
                    formdata.ShowInd = "Visible";
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = "Resources/Alarm.wav";
                    sp.Load();
                    sp.Play();
                }
                else
                {
                    formdata.ShowInd = "Hidden";
                }
                ReadBusy = true;
                string Ip = Properties.Settings.Default.Ip;
                using (var plc = new Plc(CpuType.S71200, Ip, 0, 1))
                {
                    plc.ReadTimeout = 2000;
                    plc.Open();
                    if (plc.IsConnected)
                    {
                        plc.ReadClass(plcData, 4);
                    }
                    plc.Close();
                }
                if (LinkQuality < 10) LinkQuality++;
                TransferData();
                if ((PrevTime != DateTime.Now.Hour)||(FirstRecordToDb))
                { SaveOilCount();
                    SaveLevels();
                    FirstRecordToDb = false;
                }
                PrevTime = DateTime.Now.Hour;
            }
            catch (ThreadAbortException Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }
            catch (Exception Ex)
            {
                if (LinkQuality > 0) { LinkQuality--; }
                else
                {
                    logger.AddEvent(true, Ex.Message);
                }

            }
          
            finally
            { ReadBusy = false; }
        }
        private void SaveLevels()//Сохранение уровней в резервуарах в БД
        {
            try
            {
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DateTime date = DateTime.Now;
                    string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                    string VoliumeE1 = Convert.ToString(formdata.VoliumeE1);
                    VoliumeE1 = VoliumeE1.Replace(",", ".");
                    string VoliumeE2 = Convert.ToString(formdata.VoliumeE2);
                    VoliumeE2 = VoliumeE2.Replace(",", ".");
                    string sqlExpression = $"INSERT INTO LevelHistory (Date, E1Level, E2Level) VALUES ('{date_str}', {VoliumeE1}, {VoliumeE2} )";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }
        
        private void SaveOilCount()//Сохранение показаний счетчика нефти в базу данных
        {
            try
            {
                if ((StatusOil == 1) && ((plcData.Status & 1) == 0) && (plcData.HsCounter != 0))
                //Переделать программу управления контроллера чтобы показания в счетчик садились только после слива нефти
                {
                    string SQL_s = Properties.Settings.Default.Sql;
                    string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        DateTime date = DateTime.Now;
                        string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                        string OilCount = Convert.ToString(formdata.HsCounter);
                        OilCount = OilCount.Replace(",", ".");
                        string sqlExpression = $"INSERT INTO OIlCount (Date, OilCount) VALUES ('{date_str}', {OilCount})";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        int number = command.ExecuteNonQuery();

                        connection.Close();
                    }

                }
                StatusOil = plcData.Status & 1;
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }
        #endregion

        private void TransferData()//Заполнение класса формы
        {
            try
            {
                formdata.E1_level = plcData.E1_level;
                formdata.E2_level = plcData.E2_level;
                formdata.FillingE1 = plcData.E1_level / (E1GradTable[E1GradTable.Count - 1].HeightOil * 10 / 100);
                formdata.FillingE2 = plcData.E2_level / (E2GradTable[E2GradTable.Count - 1].HeightOil * 10 / 100);

                //formdata.FillingE1 = plcData.E1_level/(plcData.Max_E1 / 100) ;
                //formdata.FillingE2 = plcData.E2_level / (plcData.Max_E2 / 100);

                formdata.HsCounter = plcData.HsCounter;
                ReadGradTable readGradTable = new ReadGradTable();
                formdata.VoliumeE1 = readGradTable.ReadTable(formdata.E1_level, E1GradTable);
                formdata.MassaE1 = Math.Round(formdata.VoliumeE1 * 0.9, 2);
                formdata.VoliumeE2 = readGradTable.ReadTable(formdata.E2_level, E2GradTable);
                formdata.MassaE2 = Math.Round(formdata.VoliumeE2 * 0.9, 2);
                formdata.Counter_in_sec = plcData.Counter_in_sec;
                formdata.Count_day = plcData.Count_day;
                formdata.Count_hour = plcData.Count_hour;
                formdata.AlarmLevelE1 = Properties.Settings.Default.AlarmLevelE1;
                formdata.AlarmLevelE2 = Properties.Settings.Default.AlarmLevelE2;
                if ((plcData.DrainStatus & 2) == 2) { formdata.ValvePos = "Задвижка открыта"; }
                else if ((plcData.DrainStatus & 4) == 4) { formdata.ValvePos = "Задвижка закрыта"; }
                else { formdata.ValvePos = "Задвижка частично открыта"; }
                if (formdata.E1_level < formdata.AlarmLevelE1)
                {
                    formdata.ColorE1 = "#FFE6E6E6";
                    formdata.ShowAlarmLevelE1 = "Collapsed";
                    new SaveErrors(false, "E1", "Тревога", "Критический уровень");
                }
                else
                {
                    formdata.ColorE1 = "#FFEC1B1B";
                    formdata.ShowAlarmLevelE1 = "Visible";
                    new SaveErrors(true, "E1", "Тревога", "Критический уровень");
                }
                if (formdata.E2_level < formdata.AlarmLevelE2)
                {
                    formdata.ColorE2 = "#FFE6E6E6";
                    formdata.ShowAlarmLevelE2 = "Collapsed";
                    new SaveErrors(false, "E2", "Тревога", "Критический уровень");
                }
                else
                {
                    formdata.ColorE2 = "#FFEC1B1B";
                    formdata.ShowAlarmLevelE2 = "Visible";
                    new SaveErrors(true, "E2", "Тревога", "Критический уровень");
                }
                if ((plcData.Status & 2) == 2)
                {
                    formdata.ErrorLevelE1 = "Visible";
                    new SaveErrors(true, "E1", "Внимание", "Неисправность уровнемера");
                }
                else
                {
                    formdata.ErrorLevelE1 = "Collapsed";
                    new SaveErrors(false, "E1", "Внимание", "Неисправность уровнемера");
                }
                if ((plcData.Status & 4) == 4)
                {
                    formdata.ErrorLevelE2 = "Visible";
                    new SaveErrors(true, "E2", "Внимание", "Неисправность уровнемера");
                }
                else
                {
                    formdata.ErrorLevelE2 = "Collapsed";
                    new SaveErrors(false, "E2", "Внимание", "Неисправность уровнемера");
                }
                if ((plcData.Status & 1) == 1)
                {
                    formdata.GazVis = "Visible";
                    new SaveErrors(true, "Счетчик газа", "Внимание", "Неисправность");
                }
                else
                {
                    formdata.GazVis = "Collapsed";
                    new SaveErrors(false, "Счетчик газа", "Внимание", "Неисправность");
                }
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }

        }

        private void AuthorisationClick(object sender, RoutedEventArgs e)
        {
            Authorisation authorisation = new Authorisation(this);
            authorisation.Owner = this;
            authorisation.Show();
        }

        private void ManAdd(object sender, RoutedEventArgs e)
        {
            if (formdata.UserName == null) { 
                MessageBox.Show("Пожалуйста авторизуйтесь");
                return;
            }
            ManualAdd manualAdd = new ManualAdd(formdata.UserName);
            manualAdd.Owner = this;
            manualAdd.Show();
        }



        private void Balance(object sender, RoutedEventArgs e)
        {
            BalanceOil balanceOil = new BalanceOil();
            balanceOil.Owner = this;
            balanceOil.Show();
        }

        private void SettingsShow(object sender, RoutedEventArgs e)
        {
            if (formdata.UserName == null)
            {
                MessageBox.Show("Пожалуйста авторизуйтесь");
                return;
            }
            SettingsForm settingsForm = new SettingsForm(this);
            settingsForm.Owner = this;
            settingsForm.Show();
        }

        private void ShowAlarmList(object sender, RoutedEventArgs e)
        {
            ShowAlarms showAlarms = new ShowAlarms();
            showAlarms.Owner = this;
            showAlarms.Show();
        }

        private void AcceptAlarms(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (formdata.UserName == null)
                {
                    MessageBox.Show("Пожалуйста авторизуйтесь");
                    return;
                }
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DateTime date = DateTime.Now;
                    string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                    string sqlExpression = $"UPDATE Alarms SET AdoptUser='{formdata.UserName}', DateAdopt='{date_str}' WHERE Curr=1 ";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }

        private void BalanceGaz(object sender, RoutedEventArgs e)
        {
            GazDebet gazDebet = new GazDebet();
            gazDebet.Owner = this;
            gazDebet.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            logger.AddEvent(false, "Программа Завершена");
        }

        private void TrendsShow(object sender, RoutedEventArgs e)
        {
            Trends trends = new Trends();
            trends.Owner = this;
            trends.Show();
        }
    }
}
