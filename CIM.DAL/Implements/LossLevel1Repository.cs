using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using CIM.Model;

namespace CIM.DAL.Implements
{
    //class LossLevel1Repository
    public class LossLevel1Repository : ILossLevel1Repository
    {
        private IConfiguration configuration;

        public LossLevel1Repository(
            IConfiguration config
            )
        {
            configuration = config;
        }


        public List<LossLevel1Model> All()
        {
            DataTable dt = new DataTable("Loss_Level_1");

            //throw new NotImplementedException();
            var connectionString = configuration.GetConnectionString("CIMDatabase");
            var output = string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //SqlDataReader
                connection.Open();

                //var _sql = " DECLARE @result NVARCHAR(max); ";
                //_sql += $" SET @result = ({sql} for json auto); ";
                //_sql += " SELECT @result as Result;";

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
                                ";

                SqlCommand command = new SqlCommand(_sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    /*
                    String result = "";
                    while (dataReader.Read())
                    {
                        output = Convert.ToString(dataReader["Description"]);
                        //result += dataReader.GetValues().ToString().Trim();
                    }
                    */


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
                    //lists.StudentId = Convert.ToInt32(dt.Rows[i]["StudentId"]);
                    //lists.StudentName = dt.Rows[i]["StudentName"].ToString();
                    //lists.Address = dt.Rows[i]["Address"].ToString();
                    //lists.MobileNo = dt.Rows[i]["MobileNo"].ToString();

                    list.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    list.Description = dt.Rows[i]["Description"].ToString();



                    //public int Id { get; set; }
                    //public string Description { get; set; }
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
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
        }


    }
}
