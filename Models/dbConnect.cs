using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

namespace MY_GYM.Models
{
    public class dbConnect
    {
        private static DateTime SQLMinDate = new DateTime(1900, 1, 1, 0, 0, 0);

        public DataSet ExecSelectProcedure(string ProcedureName, Hashtable Parameters, ref string strErrorMsg)
        {
            DataSet my_dataset = new DataSet();
            try
            {
                using (SqlConnection my_connection = OpenSqlConnection())
                {
                    SqlCommand my_command = my_connection.CreateCommand();
                    my_command.CommandText = ProcedureName;
                    my_command.CommandType = CommandType.StoredProcedure;
                    my_command.CommandTimeout = 0;
                    if (Parameters != null && Parameters.Count > 0)
                    {
                        AssignParameters(my_command, Parameters);
                    }

                    using (SqlDataAdapter my_adapter = new SqlDataAdapter(my_command))
                    {
                        my_adapter.Fill(my_dataset, ProcedureName);
                    }
                }
            }
            catch (Exception my_exception)
            {
                strErrorMsg = my_exception.Message.ToString();
            }
            return my_dataset;
        }

        public static SqlConnection OpenSqlConnection()
        {
            string connectionString = "Data Source=DESKTOP-U7874SD\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=GYM;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            SqlConnection my_connection = new SqlConnection(connectionString);
            my_connection.Open();
            return my_connection;
        }

        public static void AssignParameters(SqlCommand Command, Hashtable Parameters)
        {
            if (Parameters == null) return;
            foreach (object key in Parameters.Keys)
            {
                Command.Parameters.AddWithValue("@" + key.ToString().ToUpper(), Parameters[key]);
            }
        }
    }
}
