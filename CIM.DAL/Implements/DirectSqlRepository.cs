using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;

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

        public int ExecuteNonQuery(string sql)
        {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    return command.ExecuteNonQuery();
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
            }
            return output;
        }

        public int ExecuteSPNonQuery(string sql, Dictionary<string, object> parameters) {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    if (parameters != null)
                        foreach (var p in parameters)
                            if (p.Value != null) command.Parameters.AddWithValue(p.Key, p.Value);

                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
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

        public DataSet ExecuteSPWithQueryDSet(string sql, Dictionary<string, object> parameters)
        {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                        foreach (var p in parameters)
                            if (p.Value != null) command.Parameters.AddWithValue(p.Key, p.Value);

                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = new DataSet();
                    da.Fill(ds);

                    return ds;
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

        public void bulkCopy(string _destinationtable, DataTable _source)
        {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn))
                    {
                        bulkCopy.DestinationTableName = _destinationtable;

                        // Write from the source to the destination.
                        bulkCopy.WriteToServer(_source);
                    }
                }
                catch
                {
                }
            }
        }

        public T ExecuteFunction<T>(string sql, Dictionary<string, object> parameters) {
            var connectionString = _configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString)) {

                var pars = string.Join(",",parameters.Select(p=>$"'{p.Value}'"??"default"));
                sql = $"select {sql} ({pars})";

                using (SqlCommand command = new SqlCommand(sql, connection)) {

                    connection.Open();
                    return (T)command.ExecuteScalar();
                }
            }
        }
    }
}
