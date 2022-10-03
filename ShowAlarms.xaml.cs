using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace ARM_Baidankino
{
    /// <summary>
    /// Логика взаимодействия для ShowAlarms.xaml
    /// </summary>
    public partial class ShowAlarms : Window
    {
        public class AlarmData
        {
            public string ID { get; set; }
            public string DTime { get; set; }
            public string Object { get; set; }
            public string User { get; set; }
            public string A_Type { get; set; }
            public string A_Text { get; set; }
            public string ASK_DTime { get; set; }
            public string RES_DTime { get; set; }
        }
        Logger logger = new Logger();
        //  public AlarmData alarmData { get; set; }
        List<AlarmData> dataList = new List<AlarmData>();
        public ShowAlarms()
        {
            InitializeComponent();
            Date_start.SelectedDate = DateTime.Now.Date;
            Date_end.SelectedDate = DateTime.Now.Date.AddHours(24);
            ShowData();
        }
        private void ShowData()
        {
            try
            {
                dataList.Clear();
                string date_start = Convert.ToDateTime(Date_start.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                string date_end = Convert.ToDateTime(Date_end.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "";
                    if (Ch_alarm.IsChecked == true) { sqlExpression = $"SELECT * FROM  Alarms WHERE DateStart>='{date_start}' and DateStart<='{date_end}' and AlarmType='Тревога' order by DateStart DESC"; }
                    else { sqlExpression = $"SELECT * FROM  Alarms WHERE DateStart>='{date_start}' and DateStart<='{date_end}' order by DateStart DESC"; }

                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataList.Add(new AlarmData()
                            {
                                ID = Convert.ToString(reader.GetValue(0)),
                                DTime = Convert.ToString(reader.GetValue(1)), 
                                Object = Convert.ToString(reader.GetValue(7)),
                                User = Convert.ToString(reader.GetValue(4)),
                                A_Type = Convert.ToString(reader.GetValue(5)),
                                A_Text = Convert.ToString(reader.GetValue(6)),
                                ASK_DTime = Convert.ToString(reader.GetValue(3)),
                                RES_DTime = Convert.ToString(reader.GetValue(2))
                            });
                        }

                    }
                    reader.Close();
                    connection.Close();
                    Alarm_table.ItemsSource = null;
                    Alarm_table.ItemsSource = dataList;
                }
            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);
            }

        }
        private void Date_start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData();
        }

        private void Date_end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowData();
        }

        private void Ch_alarm_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }
        private SolidColorBrush Ye = new SolidColorBrush(Colors.Yellow);
        private SolidColorBrush Re = new SolidColorBrush(Colors.Red);
        private SolidColorBrush Gr = new SolidColorBrush(Colors.LightGreen);
        private void Al_list_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var path = (AlarmData)e.Row.DataContext;
            string Al = path.A_Type;
            //  string Us = path.ASK_User;
            if (Al == "Внимание") e.Row.Background = Ye;
            if (Al == "Тревога")
            {

                e.Row.Background = Re;

            }
        }
    }
}
