//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CIM.DAL.Implements
//{
//    class DirectSqlRepository2
//    {
//    }
//}
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Configuration;
using CIM.Model;

namespace CIM.DAL.Implements
{
    public class DirectSqlRepository2 : IDirectSqlRepository2
    {
        private IConfiguration configuration;

        public DirectSqlRepository2(
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
                SqlCommand command = new SqlCommand(sql, connection);
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

        public List<LossLevel1Model> LossLevel1ListAll()
        {
            DataTable dt = new DataTable("Loss_Level_1");
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            var output = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                String _sql = @"
                                SELECT [Id]
                                      ,[Description]
                                      ,[IsActive]
                                      ,[IsDelete]
                                      ,[CreatedAt]
                                      ,[CreatedBy]
                                      ,[UpdatedAt]
                                      ,[UpdatedBy]
                                  FROM [Loss_Level_1]
                                  ORDER BY [Id] ASC
                                ";

                SqlCommand command = new SqlCommand(_sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    //Load DataReader into the DataTable.
                    dt.Load(dataReader);

                }
                connection.Close();
            }

            try
            {
                List<LossLevel1Model> lists = new List<LossLevel1Model>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LossLevel1Model list = new LossLevel1Model();

                    list.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    list.Description = dt.Rows[i]["Description"].ToString();
                    list.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);      //public bool IsActive { get; set; }
                    list.IsDelete = Convert.ToBoolean(dt.Rows[i]["IsDelete"]);      //public bool IsDelete { get; set; }
                    list.CreatedAt = Convert.ToDateTime(dt.Rows[i]["CreatedAt"]);   //public DateTime CreatedAt { get; set; }
                    list.CreatedBy = dt.Rows[i]["CreatedBy"].ToString();            //public string CreatedBy { get; set; }
                    list.UpdatedAt = Convert.ToDateTime(dt.Rows[i]["UpdatedAt"]);   //public DateTime? UpdatedAt { get; set; }
                    list.UpdatedBy = dt.Rows[i]["UpdatedBy"].ToString();            //public string UpdatedBy { get; set; }

                    lists.Add(list);
                }
                return lists;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public void LossLevel1Insert(LossLevel1Model lossLevel1)
        {


            string sql;
            sql = @"
                    INSERT INTO [dbo].[Loss_Level_1]
                   ([Description]
                   ,[IsActive]
                   ,[IsDelete]
                   ,[CreatedAt]
                   ,[CreatedBy]
                   ,[UpdatedAt]
                   ,[UpdatedBy])
             VALUES
                   (
		           '" + lossLevel1.Description + @"'--<Description, nvarchar(250),>
                   ," + Convert.ToInt32(lossLevel1.IsActive) + @"--< IsActive, bit,>
                   ," + Convert.ToInt32(lossLevel1.IsDelete) + @"--<IsDelete, bit,>
                   ,CURRENT_TIMESTAMP--<CreatedAt, datetime,>
                   ,'" + lossLevel1.CreatedBy + @"'--<CreatedBy, nvarchar(128),>
                   ,CURRENT_TIMESTAMP--<UpdatedAt, datetime,>
                   ,CURRENT_TIMESTAMP--<UpdatedBy, nvarchar(128),>
		           )
                    ";
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

        public void LossLevel2Insert(LossLevel1Model lossLevel2)
        {


            string sql;
            sql = @"
                    INSERT INTO [dbo].[Loss_Level_1]
                   ([Description]
                   ,[IsActive]
                   ,[IsDelete]
                   ,[CreatedAt]
                   ,[CreatedBy]
                   ,[UpdatedAt]
                   ,[UpdatedBy])
             VALUES
                   (
		           '" + lossLevel2.Description + @"'--<Description, nvarchar(250),>
                   ," + Convert.ToInt32(lossLevel2.IsActive) + @"--< IsActive, bit,>
                   ," + Convert.ToInt32(lossLevel2.IsDelete) + @"--<IsDelete, bit,>
                   ,CURRENT_TIMESTAMP--<CreatedAt, datetime,>
                   ,'" + lossLevel2.CreatedBy + @"'--<CreatedBy, nvarchar(128),>
                   ,CURRENT_TIMESTAMP--<UpdatedAt, datetime,>
                   ,CURRENT_TIMESTAMP--<UpdatedBy, nvarchar(128),>
		           )
                    ";
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


        public DataTable Query(String sql)
        {
            DataTable dt = new DataTable("result");
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            var output = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                //String _sql = @"
                //                SELECT [Id]
                //                      ,[Description]
                //                      ,[IsActive]
                //                      ,[IsDelete]
                //                      ,[CreatedAt]
                //                      ,[CreatedBy]
                //                      ,[UpdatedAt]
                //                      ,[UpdatedBy]
                //                  FROM [Loss_Level_1]
                //                  ORDER BY [Id] ASC
                //                ";

                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    //Load DataReader into the DataTable.
                    dt.Load(dataReader);

                }
                connection.Close();
                return dt;
            }

            //try
            //{
            //    List<LossLevel1Model> lists = new List<LossLevel1Model>();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        LossLevel1Model list = new LossLevel1Model();

            //        list.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
            //        list.Description = dt.Rows[i]["Description"].ToString();
            //        list.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);      //public bool IsActive { get; set; }
            //        list.IsDelete = Convert.ToBoolean(dt.Rows[i]["IsDelete"]);      //public bool IsDelete { get; set; }
            //        list.CreatedAt = Convert.ToDateTime(dt.Rows[i]["CreatedAt"]);   //public DateTime CreatedAt { get; set; }
            //        list.CreatedBy = dt.Rows[i]["CreatedBy"].ToString();            //public string CreatedBy { get; set; }
            //        list.UpdatedAt = Convert.ToDateTime(dt.Rows[i]["UpdatedAt"]);   //public DateTime? UpdatedAt { get; set; }
            //        list.UpdatedBy = dt.Rows[i]["UpdatedBy"].ToString();            //public string UpdatedBy { get; set; }

            //        lists.Add(list);
            //    }
            //    return lists;
            //}
            //catch (Exception ex)
            //{
            //    throw new NotImplementedException();
            //}
        }

        public bool NonQuery(string sql)
        {
            bool result = false;

            //   string sql;
            //   sql = @"
            //       INSERT INTO [dbo].[Loss_Level_1]
            //      ([Description]
            //      ,[IsActive]
            //      ,[IsDelete]
            //      ,[CreatedAt]
            //      ,[CreatedBy]
            //      ,[UpdatedAt]
            //      ,[UpdatedBy])
            //VALUES
            //      (
            //'" + lossLevel1.Description + @"'--<Description, nvarchar(250),>
            //      ," + Convert.ToInt32(lossLevel1.IsActive) + @"--< IsActive, bit,>
            //      ," + Convert.ToInt32(lossLevel1.IsDelete) + @"--<IsDelete, bit,>
            //      ,CURRENT_TIMESTAMP--<CreatedAt, datetime,>
            //      ,'" + lossLevel1.CreatedBy + @"'--<CreatedBy, nvarchar(128),>
            //      ,CURRENT_TIMESTAMP--<UpdatedAt, datetime,>
            //      ,CURRENT_TIMESTAMP--<UpdatedBy, nvarchar(128),>
            //)
            //       ";
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    result = true;
                }
            }
            return result;
            //throw new NotImplementedException();
        }
    }
}
