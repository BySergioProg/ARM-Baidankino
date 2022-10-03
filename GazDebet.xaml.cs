using ARM_Baidankino.Models;
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

namespace ARM_Baidankino
{
    /// <summary>
    /// Логика взаимодействия для GazDebet.xaml
    /// </summary>
    public partial class GazDebet : Window
    {
        Logger logger = new Logger();
        public class TableData
        {
            public DateTime DateEvent { get; set; }
            public double Value { get; set; }

        }
        public GazDebet()
        {
            InitializeComponent();
            DateStart.SelectedDate = DateTime.Now;
            DateEnd.SelectedDate = DateTime.Now;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                List<TableData> tableDatas = new List<TableData>();
                using (DbConnect db = new DbConnect())
                {
                    var counterData = db.GazDays.ToList().OrderBy(p => p.Date).Where(p => p.Date >= Convert.ToDateTime(DateStart.SelectedDate) && p.Date <= Convert.ToDateTime(DateEnd.SelectedDate).AddHours(24));
                    foreach (var CountData in counterData)
                    {
                        tableDatas.Add(new TableData { DateEvent = CountData.Date, Value = CountData.Value });
                    }
                }
                ExitTable.ItemsSource = tableDatas;
            }
            catch (Exception Ex)
            { logger.AddEvent(true, Ex.Message); }
        }

        private void CalendarClosed(object sender, RoutedEventArgs e)
        {
            var StartDate = Convert.ToDateTime(DateStart.SelectedDate);
            var EndDate = Convert.ToDateTime(DateEnd.SelectedDate);
            if (StartDate <= EndDate) { LoadData(); }
            else { MessageBox.Show("Дата окончания периода меньше даты начала"); }
        }
    }
}
