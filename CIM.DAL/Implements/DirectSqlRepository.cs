using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class DirectSqlRepository : IDirectSqlRepository
    {
        private IConfiguration configuration;

        public DirectSqlRepository(
            IConfiguration config
            )
        {
            configuration = config;
        }
        public void ExecuteNonQuery(string sql, object[] parameters)
        {
            var connectionString = configuration.GetConnectionString("CIMDatabase");
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
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            var output = string.Empty;
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

        public DataTable ExecuteWithQuery(string sql, SqlParameter[] parameters) {
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    if (parameters != null) 
                        foreach (var p in parameters)
                            command.Parameters.AddWithValue(p.ParameterName, p.Value);

                    connection.Open();
                    command.CommandType = CommandType.Text;
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    return dt;
                }
            }
        }
    }
}
