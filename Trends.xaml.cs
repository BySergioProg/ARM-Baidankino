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
    /// Логика взаимодействия для Trends.xaml
    /// </summary>
    public partial class Trends : Window
    {
        private class Data
        {
            public string DateTime { get; set; }
            public double E1Vol { get; set; }//Уровень в емкости Е1
            public double E2Vol { get; set; }//Уровень в емкости Е2
            public double E1Massa { get; set; }//Масса в емкости Е1
            public double E2Massa { get; set; }//Масса в емкости Е2
        }
        public Trends()
        {
            InitializeComponent();
            Date_start.SelectedDate = DateTime.Now.Date;
            DateTime Date1 = new DateTime();
            Date1 = Convert.ToDateTime(Date_start.SelectedDate).AddHours(24);
        }
        private void Show_Data()
        {
            try
            {
                string Dir = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={Dir};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string date_start = Convert.ToDateTime(Date_start.SelectedDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    DateTime Date1 = new DateTime();
                    Date1 = Convert.ToDateTime(Date_start.SelectedDate).AddHours(24);
                    string date_end = Convert.ToDateTime(Date1).ToString("yyyy-MM-ddTHH:mm:ss");
                    List<Data> data = new List<Data>();
                    connection.Open();
                    string sqlExpression = $"SELECT * FROM LevelHistory WHERE Date>='{date_start}' AND Date<='{date_end}' ORDER BY Date";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            data.Add(new Data()
                            {
                                DateTime = Convert.ToDateTime(reader.GetValue(1)).ToString("yyyy-MM-dd HH:mm:ss"),
                                E1Vol = Math.Round(Convert.ToDouble(reader.GetValue(2)), 3) ,
                                E2Vol = Math.Round(Convert.ToDouble(reader.GetValue(3)), 3),
                                E1Massa = Math.Round(Convert.ToDouble(reader.GetValue(2)), 3)*0.9,
                                E2Massa = Math.Round(Convert.ToDouble(reader.GetValue(3)), 3) * 0.9,

                            });

                        }
                    }

                    DataExit.ItemsSource = data;

                    connection.Close();
                }
            }
            catch (Exception Ex)
            {
                //Logger log = LogManager.GetCurrentClassLogger();
                //log.Error($"Ошибка доступа к БД {Ex.Message}");
                //MessageBox.Show(Ex.Message);

            }

        }
        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Show_Data();
        }
    }
}
