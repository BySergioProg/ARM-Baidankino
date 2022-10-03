using ARM_Baidankino.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для BalanceOil.xaml
    /// </summary>
    public class TableData
        {
        public DateTime DateEvent { get; set; }
        public double CounterData { get; set; }
        public double ManualData { get; set; }
        public double TotalData { get; set; }
        public double TotalBallance { get; set; }
    }
    
    public partial class BalanceOil : Window
    {

        Logger logger = new Logger();
        public BalanceOil()
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

                var TempDateStart = Convert.ToDateTime(DateStart.SelectedDate).Date;
                var TempDateEnd = Convert.ToDateTime(DateEnd.SelectedDate).AddHours(24).Date;
                while (TempDateStart != TempDateEnd)
                {
                    tableDatas.Add(new TableData { DateEvent = TempDateStart });
                    TempDateStart = TempDateStart.AddHours(24);

                }
                using (DbConnect db = new DbConnect())
                {
                    var manualOil = db.ManualOils.ToList().OrderBy(p => p.Date).Where(p => p.Date >= Convert.ToDateTime(DateStart.SelectedDate) && p.Date <= Convert.ToDateTime(DateEnd.SelectedDate).AddHours(24));
                    var counterOil = db.OilCounts.ToList().OrderBy(p => p.Date).Where(p => p.Date >= Convert.ToDateTime(DateStart.SelectedDate) && p.Date <= Convert.ToDateTime(DateEnd.SelectedDate).AddHours(24));
                    var levelHistories = db.LevelHistories.ToList().OrderBy(p => p.Date).Where(p => p.Date >= Convert.ToDateTime(DateStart.SelectedDate) && p.Date <= Convert.ToDateTime(DateEnd.SelectedDate).AddHours(24));
                    foreach (var Data in tableDatas)
                    {
                        foreach (var ManOil in manualOil)
                        {
                            if (Data.DateEvent.Date == ManOil.Date.Date)
                            {
                                Data.ManualData += ManOil.OilCount;
                            }
                            Data.ManualData = Math.Round(Data.ManualData, 2);
                        }
                        foreach (var CountOil in counterOil)
                        {
                            if (Data.DateEvent.Date == CountOil.Date.Date)
                            { Data.CounterData += CountOil.OilCount1; }
                        }
                        Data.CounterData = Math.Round(Data.CounterData, 2);
                        Data.TotalData = Data.ManualData + Data.CounterData;
                        float StartE1 = 0, StartE2 = 0, EndE1 = 0, EndE2 = 0;
                      
                        foreach (var OilData in levelHistories)
                        {
                            if ((Data.DateEvent.Date <= OilData.Date) && (Data.DateEvent.Date.AddHours(24) >= OilData.Date))
                            //    if (Data.DateEvent.Date == OilData.Date.Date)
                                {
                                
                                if (StartE1 == 0)
                                {
                                   
                                    StartE1 = (float)Math.Round( OilData.E1Level, 3);
                                    StartE2 =(float)Math.Round( OilData.E2Level, 3);
                                }
                                EndE1 =(float)Math.Round( OilData.E1Level, 3);
                                EndE2 =(float) Math.Round( OilData.E2Level, 3);
                            }
                        }
                        Data.TotalBallance = Math.Round((EndE1 + EndE2) - (StartE1 + StartE2) + Data.TotalData, 3);

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
