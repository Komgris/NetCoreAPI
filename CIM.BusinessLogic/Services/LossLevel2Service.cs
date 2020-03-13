//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CIM.BusinessLogic.Services
//{
//    class LossLevel2Service
//    {
//    }
//}
using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class LossLevel2Service : ILossLevel2Service
    {
        private IDirectSqlRepository2 _repository;

        public LossLevel2Service(IDirectSqlRepository2 repository)
        {
            _repository = repository;
        }
        public void ExecuteNonQuery(string sql, object[] parameters = null)
        {
            _repository.ExecuteNonQuery(sql, parameters);
        }

        public string ExecuteReader(string sql, object[] parameters = null)
        {
            return _repository.ExecuteReader(sql, parameters);
        }

        public void InsertLossLevel2(LossLevel2Model lossLevel2)
        {

            String sql = "";
            sql = @"

                    INSERT INTO [dbo].[Loss_Level_2]
                               ([Description]
                               ,[IsActive]
                               ,[IsDelete]
                               ,[CreatedAt]
                               ,[CreatedBy]
                               ,[UpdatedAt]
                               ,[UpdatedBy]
                               ,[Loss_Level_1_Id])
                         VALUES
                               (
			                       '" + lossLevel2.Description + @"'--<Description, nvarchar(250),>
			                       ," + Convert.ToInt32(lossLevel2.IsActive) + @"--<IsActive, bit,>
			                       ," + Convert.ToInt32(lossLevel2.IsDelete) + @"--<IsDelete, bit,>
			                       ,CURRENT_TIMESTAMP--<CreatedAt, datetime,>
			                       ,'" + lossLevel2.CreatedBy + @"'--<CreatedBy, nvarchar(128),>
			                       ,CURRENT_TIMESTAMP--<UpdatedAt, datetime,>
			                       ,'" + lossLevel2.CreatedBy + @"'--<UpdatedBy, nvarchar(128),>
			                       ," + Convert.ToInt32(lossLevel2.LossLevel1Id) + @"--<Loss_Level_1_Id, int,>
		                       );
                    ";

            _repository.NonQuery(sql);
        }

        public List<LossLevel2Model> ListAllLossLevel2()
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
                                  ,[Loss_Level_1_Id]
                              FROM [Loss_Level_2]
                              ORDER BY [Id] ASC
                                ";
            dt = _repository.Query(sql).Copy();

            try
            {
                List<LossLevel2Model> lists = new List<LossLevel2Model>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LossLevel2Model list = new LossLevel2Model();

                    list.Id = Convert.ToInt32(dt.Rows[i]["Id"]);//public int Id { get; set; }
                    list.Description = dt.Rows[i]["Description"].ToString();//public string Description { get; set; }
                    list.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]);//public bool IsActive { get; set; }
                    list.IsDelete = Convert.ToBoolean(dt.Rows[i]["IsDelete"]);//public bool IsDelete { get; set; }
                    list.CreatedAt = Convert.ToDateTime(dt.Rows[i]["CreatedAt"]);//public DateTime CreatedAt { get; set; }
                    list.CreatedBy = dt.Rows[i]["CreatedBy"].ToString();//public string CreatedBy { get; set; }
                    list.UpdatedAt = Convert.ToDateTime(dt.Rows[i]["UpdatedAt"]);//public DateTime? UpdatedAt { get; set; }
                    list.UpdatedBy = dt.Rows[i]["UpdatedBy"].ToString(); //public string UpdatedBy { get; set; }
                    list.LossLevel1Id = Convert.ToInt32(dt.Rows[i]["Loss_Level_1_Id"]);//public int? LossLevel1Id { get; set; }
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

