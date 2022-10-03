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
    /// Логика взаимодействия для Authorisation.xaml
    /// </summary>
   
    public partial class Authorisation : Window
    {
        Logger logger = new Logger();
        public MainWindow mainWindow;
        public Authorisation(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void AuthClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string user = User.Text;
                string password = Password.Password;
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlExpression = $"SELECT * FROM UserData WHERE UserName='{user}' AND Password='{password}'";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int level = Convert.ToInt32(reader.GetValue(3));
                            mainWindow.formdata.UserName = user;
                            mainWindow.formdata.UserLevel = level;
                            logger.AddEvent(false, $"Авторизация пользователя: {user}");
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неправильное имя пользователя или пароль");
                    }
                    reader.Close();
                    connection.Close();
                }

            }
            catch (Exception Ex)
            {
                logger.AddEvent(true, Ex.Message);

         
            }
        }
    }
}
