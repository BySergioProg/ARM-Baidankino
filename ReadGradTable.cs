using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARM_Baidankino
{
    class ReadGradTable
    {
        
        public double ReadTable(double Level, List<GradTable> gradTables)
        {
            
            double Data = 0;
         
            foreach (GradTable gradTable in gradTables) 
            {
                if (gradTable.HeightOil == (int)Math.Round(Level / 10))
                {
                    Data = gradTable.VoliumeOil;
                }

            }
            return Data;
        }

    }
}
