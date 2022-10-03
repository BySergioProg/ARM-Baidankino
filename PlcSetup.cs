

namespace ARM_Baidankino
{
    public class PlcSetup
    {
        public short Min_E1 { get; set; }//Минимальный измеряемый уровень Е1
        public short Max_E1 { get; set; }//Максимальный измеряемый уровень Е1
        public short Min_E2 { get; set; }//Минимальный измеряемый уровень Е2
        public short Max_E2 { get; set; }//Максимальный измеряемый уровень Е2
        public short Counter_max { get; set; }//Максимальный расход газового счетчика
    }
}
