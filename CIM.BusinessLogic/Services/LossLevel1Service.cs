using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    //class LossLevel1Service
    public class LossLevel1Service : ILossLevel1Service
    {
        private IDirectSqlRepository2 _repository;

        public LossLevel1Service(IDirectSqlRepository2 repository)
        {
            _repository = repository;
        }

        public void InsertLossLevel1(LossLevel1Model lossLevel1)
        {

            String sql = "";
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

            _repository.NonQuery(sql);
        }

        public List<LossLevel1Model> ListAllLossLevel1()
        {
            DataTable dt;
            String sql = @"
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
            dt = _repository.Query(sql).Copy();

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
                //throw new NotImplementedException();
                throw ex;
            }
        }

    }
}
