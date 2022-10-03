

namespace ARM_Baidankino
{
   public class PlcData
    {
        public short Min_E1 { get; set; }//Минимальный измеряемый уровень Е1
        public short Max_E1 { get; set; }//Максимальный измеряемый уровень Е1
        public short Min_E2 { get; set; }//Минимальный измеряемый уровень Е2
        public short Max_E2 { get; set; }//Максимальный измеряемый уровень Е2
        public short Counter_max { get; set; }//Максимальный расход газового счетчика
        public float Counter_in_sec { get; set; } //Расход в секунду
        public float Count_hour { get; set; }//Расход в час
        public float Count_day { get; set; }//Расход в сутки
        public short E1_level { get; set; }//Уровень в резервуаре Е1
        public short E2_level { get; set; }//Уровень в резервуаре Е2
        public float Oil_count { get; set; }//Показания счетчика жидкости
        public ushort Status { get; set; }//Статус системы (см Tags SystemStatus)
        public ushort DrainStatus { get; set; }//Статус узла слива (см Tags Status)
        public int HsCounter { get; set; }//Количество импульсов скоростного счетчика
    }
}
