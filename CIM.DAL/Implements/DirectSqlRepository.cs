using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CIM.DAL.Implements
{
    public class DirectSqlRepository : IDirectSqlRepository
    {

        public void ExecuteNonQuery(string sql, object[] parameters)
        {
            string connectionString = "Server=tcp:dole-cim.database.windows.net,1433;Initial Catalog=cim_db;Persist Security Info=False;User ID=cim;Password=4dev@psec;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";// Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public string ExecuteReader(string sql, object[] parameters)
        {
            var output = string.Empty;
            string connectionString = "Server=tcp:dole-cim.database.windows.net,1433;Initial Catalog=cim_db;Persist Security Info=False;User ID=cim;Password=4dev@psec;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";// Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                var _sql = " DECLARE @result NVARCHAR(max); ";
                _sql += $" SET @result = ({sql} for json auto); ";
                _sql += " SELECT @result as Result;";
                SqlCommand command = new SqlCommand(_sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        output = Convert.ToString(dataReader["result"]);
                    }
                }
                connection.Close();
            }
            return output;
        }
    }
}
