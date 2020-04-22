using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using StoredProcedureEFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
namespace CIM.DAL.Implements
{
    public class LossLevel3Repository : Repository<LossLevel3>, ILossLevel3Repository
    {
        //protected cim_dbContext _entities;
        //protected readonly DbSet<T> _dbset;
        private readonly string _connectionString;
        public LossLevel3Repository(cim_dbContext context) : base(context)
        {
            //_entities = context;
            //_dbset = context.Set<T>();
            _connectionString = _entities.Database.GetDbConnection().ConnectionString;
        }

        public async Task<IList<LossLevelComponentMappingModel>> ListComponentMappingAsync()
        {
            IList<LossLevelComponentMappingModel> data = null;
            //If you get a Object reference not set to instance of object error then make sure your sp result column is not null
            var proc = _entities.LoadStoredProc("sp_ListComponentMappingAsync");
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevelComponentMappingModel>()));
            return data;

        }

        public async Task<IList<LossLevelMachineMappingModel>> ListMachineMappingAsync()
        {

            IList<LossLevelMachineMappingModel> data = null;
            //If you get a Object reference not set to instance of object error then make sure your sp result column is not null
            var proc = _entities.LoadStoredProc("sp_ListMachineMappingAsync");
            await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevelMachineMappingModel>()));
            return data;
        }

        //public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        //{
        //    List<LossLevel3ViewModel> data = null;
        //    var proc = _entities.LoadStoredProc("[sp_ListLossLevel3]");
        //    proc.AddParam("total_count", out IOutParam<int> totalCount);
        //    proc.AddParam("@keyword", keyword);
        //    proc.AddParam("@is_active", isActive);
        //    proc.AddParam("@howmany", howmany);
        //    proc.AddParam("@page", page);
        //    await proc.ExecAsync(x => Task.Run(() => data = x.ToList<LossLevel3ViewModel>()));
        //    return ToPagingModel(data, totalCount.Value, page, howmany);
        //}

        public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        {
            List<LossLevel3ViewModel> output = new List<LossLevel3ViewModel>();

            int total = 0;


            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    string sql = @"EXEC	[dbo].[sp_ListLossLevel3] @is_active = 1";
            //    var output = connection.QueryAsync<LossLevel3ViewModel>(sql).Result.ToList();
            //    Console.WriteLine(output.Count());
            //}

            using (var connection = new SqlConnection(_connectionString))
            {
                DynamicParameters parameter = new DynamicParameters();

                parameter.Add("@keyword", keyword, DbType.String, ParameterDirection.Input);
                parameter.Add("@is_active", isActive, DbType.Boolean, ParameterDirection.Input);
                parameter.Add("@page", page, DbType.Int16, ParameterDirection.Input);
                parameter.Add("@howmany", howmany, DbType.Int16, ParameterDirection.Input);

                string sql = @"sp_ListLossLevel3_cte";

                //storeProcedureName, parameters, null, null, CommandType.StoredProcedure
                var result = connection.QueryAsync(sql, parameter, null, null, CommandType.StoredProcedure).Result.ToList();
                Console.WriteLine(output.Count());


                foreach (var item in result)
                {
                    //if (total == 0)
                    //{
                    //    total = Convert.ToInt16(item.TotalCount);
                    //}
                    Console.WriteLine(item.Id);
                    //output.Add(new LossLevel3ViewModel
                    //{
                    //    Id = item.Id,//public int Id { get; set; }
                    //    Name = item.Name,              //public string Name { get; set; }
                    //    Description = item.Description,             //public string Description { get; set; }
                    //    IsActive = item.IsActive,                    //public bool IsActive { get; set; }
                    //    LossLevel2Id = item.LossLevel2Id,                                //public int? LossLevel2Id { get; set; }
                    //    LossLevel1Id = item.LossLevel1Id,                                            //public int LossLevel1Id { get; set; }
                    //    LossLevel2Name = item.LossLevel2Name,                                                       //public string LossLevel1Name { get; set; }

                    output.Add(new LossLevel3ViewModel
                    {
                        Id = Convert.ToInt16(item.Id),
                        Name = Convert.ToString(item.Name),
                        Description = Convert.ToString(item.Description),
                        IsActive = Convert.ToBoolean(item.IsActive),
                        LossLevel2Id = Convert.ToInt16(item.LossLevel2Id),
                        LossLevel1Id = Convert.ToInt16(item.LossLevel1Id),
                        LossLevel1Name = Convert.ToString(item.LossLevel1Name),
                        LossLevel2Name = Convert.ToString(item.LossLevel2Name),

                    });
                }
            }

            //var output = new List<LossLevel3ViewModel>();



            //foreach (var item in output)
            //    item.LossLevel2Name = "test";
            return new PagingModel<LossLevel3ViewModel>
            {
                HowMany = total,
                Data = output
            };

            //Task<PagingModel<LossLevel3ViewModel>> data = null;
            //return await data;
        }
        /*


        using (var connection = new SqlConnection(FiddleHelper.GetConnectionStringSqlServerW3Schools()))
        {
            connection.Open();

            using (var multi = connection.QueryMultipleAsync(sql).Result)
            {
                var orders = multi.Read<Order>().ToList();
            var orderDetails = multi.Read<OrderDetail>().ToList();

            FiddleHelper.WriteTable(orders);
                FiddleHelper.WriteTable(orderDetails);
            }
        }
        */


        //      public async Task<PagingModel<LossLevel3ViewModel>> ListAsPaging(int page, int howmany, string keyword, bool isActive)
        //      {
        //          //var output = new List<LossLevel3ViewModel>();
        //          //using (var connection = new SqlConnection(_connectionString))

        //          //{

        //          //    var output = await connection.QueryAsync<T>(sql, parameters);

        //          //    return output.ToList();

        //          //}
        //          //Dictionary<string, object> parameters = new Dictionary<string, object> ();
        //          //// IDictionary<int, string> dict = new Dictionary<int, string>();
        //          //parameters.Add("keyword","");
        //          //parameters.Add("is_active",true);
        //          //parameters.Add("page",1);
        //          //parameters.Add("howmany",10);

        //          //int total = 10;
        //          //string sql = "sp_ListLossLevel3";

        //          //var result =  await execStoreProcedure<LossLevel3ViewModel>(sql, parameters);


        //          //using (var connection = new SqlConnection(_connectionString))
        //          //{
        //          //    output = connection.QueryAsync<LossLevel3ViewModel>(sql).Result.ToList();

        //          //    var output2 = connection.QueryAsync(sql).Result.ToList();

        //          //    Console.WriteLine(output.Count());

        //          //    Console.WriteLine(output.Count());
        //          //    //FiddleHelper.WriteTable(orderDetails);

        //          //}
        //          //await execStoreProcedure<SiteModel>(sql);

        //          //foreach (var item in output)

        //          //var output = new List<LossLevel3ViewModel>();
        //          //foreach (var item in dbModel)
        //          //    output.Add(MapperHelper.AsModel(item, new LossLevel3ViewModel()));

        //          //foreach (var item in output)
        //          //    item.LossLevel2Name = "test";
        //          //return new PagingModel<LossLevel3ViewModel>
        //          //{
        //          //    HowMany = total,
        //          //    Data = output
        //          //};
        //          // Dictionary<string, object> parameters = new Dictionary<string, object>();
        //          // parameters.Add("keyword", "");
        //          // parameters.Add("is_active", true);
        //          // parameters.Add("page", 1);
        //          // parameters.Add("howmany", 10);
        //          // string procedureName = "EXEC sp_ListLossLevel3";

        //          //IList<LossLevel3ViewModel> _lossLevel3s;
        //          // _lossLevel3s =  await execStoreProcedure<LossLevel3ViewModel>(procedureName, parameters);
        //          // int a = 0;
        //          int total = 99;

        //          var sql = "sp_ListLossLevel3";

        //          using (var connection = new SqlConnection(_connectionString))
        //          {
        //              connection.Open();

        //              //DynamicParameters parameter = new DynamicParameters();

        //              //parameter.Add("@keyword", keyword, DbType.String, ParameterDirection.Input);
        //              //parameter.Add("@is_active", keyword, DbType.Binary, ParameterDirection.Input);
        //              //parameter.Add("@page", keyword, DbType.Int32, ParameterDirection.Input);
        //              //parameter.Add("@howmany", howmany, DbType.Int32, ParameterDirection.Input);



        //              //var result = connection.Execute(sql,
        //              //    parameter,
        //              //    commandType: CommandType.StoredProcedure);
        //              //int b = 99;
        //              //int rowCount = parameter.Get<int>("@RowCount");

        //              //return new PagingModel<LossLevel3ViewModel>
        //              //{
        //              //    HowMany = total,
        //              //    Data = result.tolist();
        //              //};




        //              sql = @"EXEC	@return_value = [dbo].[sp_ListLossLevel3]

        //      @keyword = N'1',
        //@is_active = 1,
        //@page = 2,
        //@howmany = 10";


        //                 var  output = connection.QueryAsync(sql).Result.ToList();
        //              Console.WriteLine(output.Count());
        //          }



        //          Task<PagingModel<LossLevel3ViewModel>> data = null;
        //          return await data;
        //      }

        //public async Task<IList<LossLevel3ViewModel>> ListAsPaging2()

        //{

        //    Dictionary<string, object> parameters = new Dictionary<string, object>();
        //    // IDictionary<int, string> dict = new Dictionary<int, string>();
        //    parameters.Add("keyword", "");
        //    parameters.Add("is_active", true);
        //    parameters.Add("page", 1);
        //    parameters.Add("howmany", 10);

        //    int total = 10;
        //    string procedureName = "sp_ListLossLevel3";

        //    return await execStoreProcedure<LossLevel3ViewModel>(procedureName, parameters);

        //}

    }
}
