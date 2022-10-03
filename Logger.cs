using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARM_Baidankino
{
    public class Logger
    {
        private void CreateCat()
        {
            try
            {
                string subpath = DateTime.Now.ToString("ddMMyyyy");
                string path = $@".\Logs\{subpath}";

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
            }
            catch
            { }
        }
        public void AddEvent(bool Alarm, string EventText)
        {
            try
            {
                string File;
                CreateCat();
                string subpath = DateTime.Now.ToString("ddMMyyyy");
                if (Alarm) { File = "Alarm"; } else { File = "Event"; }
                string path = $@".\Logs\{subpath}\{File}.txt";

                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"{DateTime.Now} : {EventText}");
                }
            }
            catch
            { }

        }
    }
}
