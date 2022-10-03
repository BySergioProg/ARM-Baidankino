using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ARM_Baidankino
{
    public class Formdata : INotifyPropertyChanged
    {
        private float counter_in_sec { get; set; }//Расход газа в секунду
        public float Counter_in_sec//Расход газа в секунду
        {
            get { return counter_in_sec; }
            set
            {
                counter_in_sec = value;
                OnPropertyChanged("Counter_in_sec");
            }
        }
        private float count_hour { get; set; }//Расход газа в час
        public float Count_hour//Расход газа в час
        {
            get { return count_hour; }
            set
            {
                count_hour = value;
                OnPropertyChanged("Count_hour");
            }
        }
        private float count_day { get; set; }//Расход газа в сутки
        public float Count_day//Расход газа в сутки
        {
            get { return count_day; }
            set
            {
                count_day = value;
                OnPropertyChanged("Count_day");
            }
        }
        private int e1_level { get; set; }//Уровень в резервуаре Е1
        public int E1_level//Уровень в резервуаре Е1
        {
            get { return e1_level; }
            set
            {
                e1_level = value;
                OnPropertyChanged("E1_level");
            }
        }
        private int e2_level { get; set; }//Уровень в резервуаре Е2
        public int E2_level//Уровень в резервуаре Е2
        {
            get { return e2_level; }
            set
            {
                e2_level = value;
                OnPropertyChanged("E2_level");
            }
        }
        private int hsCounter { get; set; }//Счетчик жидкости
        public int HsCounter//Счетчик жидкости
        {
            get { return hsCounter; }
            set
            {
                hsCounter = value;
                OnPropertyChanged("HsCounter");
            }
        }
        private double voliumeE1 { get; set; }//Объем жидкости в резервуаре Е1
        public double VoliumeE1//Объем жидкости в резервуаре Е1
        {
            get { return voliumeE1; }
            set
            {
                voliumeE1 = value;
                OnPropertyChanged("VoliumeE1");
            }
        }
        private double voliumeE2 { get; set; }//Объем жидкости в резервуаре Е2
        public double VoliumeE2//Объем жидкости в резервуаре Е2
        {
            get { return voliumeE2; }
            set
            {
                voliumeE2 = value;
                OnPropertyChanged("VoliumeE2");
            }
        }
        private double massaE1 { get; set; }//Масса жидкости в резервуаре Е1
        public double MassaE1//Масса жидкости в резервуаре Е1
        {
            get { return massaE1; }
            set
            {
                massaE1 = value;
                OnPropertyChanged("MassaE1");
            }
        }
        private double massaE2 { get; set; }//Масса жидкости в резервуаре Е2
        public double MassaE2//Масса жидкости в резервуаре Е2
        {
            get { return massaE2; }
            set
            {
                massaE2 = value;
                OnPropertyChanged("MassaE2");
            }
        }
        private int fillingE1 { get; set; }//Шкала заполнения резервуаре Е1 в %
        public int FillingE1//Шкала заполнения резервуаре Е1 в %
        {
            get { return fillingE1; }
            set
            {
                fillingE1 = value;
                OnPropertyChanged("FillingE1");
            }
        }
        private int fillingE2 { get; set; }//Шкала заполнения резервуаре Е2 в %
        public int FillingE2//Шкала заполнения резервуаре Е2 в %
        {
            get { return fillingE2; }
            set
            {
                fillingE2 = value;
                OnPropertyChanged("FillingE2");
            }
        }
        private string userName { get; set; }//имя текущего пользователя
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private int userLevel { get; set; }//Уровень доступа пользователя
        public int UserLevel
        {
            get { return userLevel; }
            set
            {
                userLevel = value;
                OnPropertyChanged("UserLevel");
            }
        }
        private int alarmLevelE1 { get; set; }//Тревожный уровень в E1
        public int AlarmLevelE1
        {
            get { return alarmLevelE1; }
            set
            {
                alarmLevelE1 = value;
                OnPropertyChanged("AlarmLevelE1");
            }
        }
        private int alarmLevelE2 { get; set; }//Тревожный уровень в E2
        public int AlarmLevelE2
        {
            get { return alarmLevelE2; }
            set
            {
                alarmLevelE2 = value;
                OnPropertyChanged("AlarmLevelE2");
            }
        }
        private string colorE1 { get; set; }// Цвет обрамления уровня Е1
        public string ColorE1
        {
            get { return colorE1; }
            set
            {
                colorE1 = value;
                OnPropertyChanged("ColorE1");
            }
        }
        private string colorE2 { get; set; }// Цвет обрамления уровня Е2
        public string ColorE2
        {
            get { return colorE2; }
            set
            {
                colorE2 = value;
                OnPropertyChanged("ColorE2");
            }
        }
        private string valvePos { get; set; }// Положение задвижки
        public string ValvePos
        {
            get { return valvePos; }
            set
            {
                valvePos = value;
                OnPropertyChanged("ValvePos");
            }
        }
        private string showAlarmLevelE1 { get; set; }// Отображение сообщения тревожный уровень Е1
        public string ShowAlarmLevelE1
        {
            get { return showAlarmLevelE1; }
            set
            {
                showAlarmLevelE1 = value;
                OnPropertyChanged("ShowAlarmLevelE1");
            }
        }
        private string showAlarmLevelE2 { get; set; }// Отображение сообщения тревожный уровень Е2
        public string ShowAlarmLevelE2
        {
            get { return showAlarmLevelE2; }
            set
            {
                showAlarmLevelE2 = value;
                OnPropertyChanged("ShowAlarmLevelE2");
            }
        }
        private string errorLevelE1 { get; set; }// Оибка датчика уровня Е1
        public string ErrorLevelE1
        {
            get { return errorLevelE1; }
            set
            {
                errorLevelE1 = value;
                OnPropertyChanged("ErrorLevelE1");
            }
        }
        private string errorLevelE2 { get; set; }// Оибка датчика уровня Е1
        public string ErrorLevelE2
        {
            get { return errorLevelE2; }
            set
            {
                errorLevelE2 = value;
                OnPropertyChanged("ErrorLevelE2");
            }
        }
        private string plcAlarm { get; set; }//отображение сообщения потери связи с контроллером
        public string PlcAlarm 
        {
            get { return plcAlarm; }
            set
            {
                plcAlarm = value;
                OnPropertyChanged("PlcAlarm");
            }
        }
        private string gazVis { get; set; }//отображение сообщения ошибки счетчика газа
        public string GazVis
        {
            get { return gazVis; }
            set
            {
                gazVis = value;
                OnPropertyChanged("GazVis");
            }
        }
        private string showInd { get; set; }//отображение сирены
        public string ShowInd
        {
            get { return showInd; }
            set
            {
                showInd = value;
                OnPropertyChanged("ShowInd");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
