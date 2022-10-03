using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ARM_Baidankino
{
    [Table(Name = "ManualOil")]
    public class ManualAddRecord : INotifyPropertyChanged
    {
        private int Id { get; set; }
      
        private string userName { get; set; }
     
        private DateTime date { get; set; }
      
        private float oilCount { get; set; }


        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id {
            get { return Id; }
            set { Id = value;
                OnPropertyChanged("id");
            } 
        }
        [Column(Name = "UserName")]
        public string UserName {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
            }
        [Column(Name = "Date")]
        public DateTime Date {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        [Column(Name = "OilCount")]
        public float OilCount {
            get { return oilCount; }
            set { oilCount = value;
                OnPropertyChanged("OilCount");
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
