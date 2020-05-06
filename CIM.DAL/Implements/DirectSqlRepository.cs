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
        private IConfiguration _configuration;

        public DirectSqlRepository(
            IConfiguration configuration
        )
        {
            _configuration = configuration;
        }
        public void ExecuteNonQuery(string sql, object[] parameters)
        {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
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
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
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

        public DataTable ExecuteSPWithQuery(string sql, Dictionary<string,object> parameters) {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    if (parameters != null) 
                        foreach (var p in parameters)
                            if(p.Value!=null) command.Parameters.AddWithValue(p.Key, p.Value);

                    connection.Open();

                    command.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    return dt;
                }
            }
        }

        public DataTable ExecuteWithQuery(string sql) {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand command = new SqlCommand(sql, connection)) {

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
