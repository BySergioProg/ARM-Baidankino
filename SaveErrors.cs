using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARM_Baidankino
{
   public class SaveErrors
    {
        Logger logger = new Logger();
        public SaveErrors(bool Set, string Device, string ErrorType, string ErrorText)
        {
            try
            {
                string SQL_s = Properties.Settings.Default.Sql;
                string connectionString = $@"Data Source={SQL_s};Initial Catalog=DNS_Baydankino;User Id = sa; Password = Qq123456";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    if (Set)
                    {
                        string sqlExpression = $"SELECT * FROM Alarms WHERE Curr=1 and AlarmType='{ErrorType}' and AlarmText='{ErrorText}' and Device='{Device}'  ";
                        SqlCommand command = new SqlCommand(sqlExpression, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                connection2.Open();
                                DateTime date = DateTime.Now;
                                string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                                sqlExpression = $"INSERT INTO Alarms (DateStart, AlarmType, AlarmText, Device, Curr) VALUES ('{date_str}', '{ErrorType}', '{ErrorText}', '{Device}', 1)";
                                SqlCommand command2 = new SqlCommand(sqlExpression, connection2);
                                int number = command2.ExecuteNonQuery();
                                connection2.Close();
                            }
                        }
                    }
                    else
                    {
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            connection2.Open();
                            DateTime date = DateTime.Now;
                            string date_str = date.ToString("yyyy-MM-ddTHH:mm:ss");
                            string sqlExpression = $"UPDATE Alarms SET Curr=0, DateEnd='{date_str}' WHERE Curr=1  and AlarmType='{ErrorType}' and AlarmText='{ErrorText}' and Device='{Device}' ";
                            SqlCommand command2 = new SqlCommand(sqlExpression, connection2);
                            int number = command2.ExecuteNonQuery();
                            connection2.Close();
                        }
                    }


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
