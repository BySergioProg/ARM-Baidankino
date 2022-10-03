using System;
using System.Collections.Generic;
using System.Data.Linq;
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
    /// Логика взаимодействия для ManualAdd.xaml
    /// </summary>
    public partial class ManualAdd : Window
    {
       static string SQL_s = Properties.Settings.Default.Sql;
       static string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
        DataContext db = new DataContext(connectionString);
        public string user;
        Logger logger = new Logger();
        public ManualAdd(string User)
        {
            InitializeComponent();
            user = User;
            AddRecords();

        }
        private void AddRecords()
        {
            try
            {
                Dat.ItemsSource = null;
                var manualAddsTable = db.GetTable<ManualAddRecord>().Where(u => u.Date >= DateTime.Now.Date);
                Dat.ItemsSource = manualAddsTable;
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OilCnt.Text != "")
                {
                    db.GetTable<ManualAddRecord>().InsertOnSubmit(new ManualAddRecord()
                    {
                        UserName = user,
                        OilCount = (float)Convert.ToDouble(OilCnt.Text),
                        Date = DateTime.Now
                    });

                }
                db.SubmitChanges();
                OilCnt.Text = null;
                AddRecords();
            }
            catch(Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }
        private void Dat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public int DS_Count(string s)
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
        private void OilCnt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) && (DS_Count(((TextBox)sender).Text) < 1))));
        }
    
    }
}
