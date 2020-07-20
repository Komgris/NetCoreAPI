using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoredProcedureEFCore;
using System.Data.SqlClient;
using System.Data;
using CIM.DAL.Utility;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class ProductionPlanRepository : Repository<ProductionPlan, object>, IProductionPlanRepository
    {

        private IDirectSqlRepository _directSqlRepository;
        public ProductionPlanRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration)
            : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<ProductionPlanModel>> Paging(int page, int howmany)
        {
            var query = _entities.ProductionPlan;
            int skipRec = (page - 1) * howmany;
            int takeRec = howmany;
            int row = query.Count();

            var paging = query.OrderBy(s => s.PlanId).Skip(skipRec).Take(takeRec);

            var data = await paging

                .Select(x => new ProductionPlanModel
                {
                    PlanId = x.PlanId,
                    ProductId = x.ProductId,
                    Target = x.Target,
                    Unit = x.UnitId,
                    StatusId = x.StatusId,
                    IsActive = x.IsActive,
                    UpdatedAt = x.UpdatedAt

                }).ToListAsync();
            return new PagingModel<ProductionPlanModel>
            {
                HowMany = row,
                Data = data
            };
        }

        public async Task<PagingModel<ProductionPlanListModel>> ListAsPaging(int page, int howmany, string keyword, int? productId, int? routeId, bool isActive, string statusIds)
        {
            return await Task.Run(() =>
                                    {
                                        Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@route_id", routeId},
                                            {"@product_id", productId},
                                            {"@keyword", keyword},
                                            {"@is_active", isActive},
                                            {"@status_id", statusIds},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                                        var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProductionPlan", parameterList);
                                        var totalCount = 0;
                                        if (dt.Rows.Count > 0)
                                            totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                                        return ToPagingModel(dt.ToModel<ProductionPlanListModel>(), totalCount, page, howmany);
                                    });
        }

        public List<ProductionPlanModel> Get()
        {
            var query = _entities.ProductionPlan;
            var data = query
                .Select(x => new ProductionPlanModel
                {
                    PlanId = x.PlanId,
                    ProductId = x.ProductId,
                    Target = x.Target,
                    Unit = x.UnitId,
                    StatusId = x.StatusId,
                }).ToList();
            return data;
        }

        public void InsertProduction(List<ProductionPlanModel> import)
        {
            foreach (var plan in import)
            {

                var insert = new ProductionPlan();
                insert.PlanId = plan.PlanId;
                insert.ProductId = plan.ProductId;
                insert.Target = plan.Target;
                insert.UnitId = plan.Unit;
                insert.StatusId = (int)Constans.PRODUCTION_PLAN_STATUS.New;
                insert.UpdatedAt = DateTime.Now;
                _entities.ProductionPlan.Add(insert);
                _entities.SaveChanges();
            }
        }

        public void DeleteProduction(string id)
        {

            var delete = _entities.ProductionPlan.Where(x => x.PlanId == id).FirstOrDefault();
            if (delete != null)
            {
                _entities.ProductionPlan.Remove(delete);
            }
            _entities.SaveChanges();

        }

        public void UpdateProduction(List<ProductionPlanModel> list)
        {
            foreach (var plan in list)
            {
                var update = _entities.ProductionPlan.Where(x => x.PlanId == plan.PlanId).FirstOrDefault();
                if (update != null)
                {
                    update.PlanId = plan.PlanId;
                    update.ProductId = plan.ProductId;
                    update.Target = plan.Target;
                    update.UnitId = plan.Unit;
                    update.UpdatedAt = DateTime.Now;
                }
                _entities.SaveChanges();
            }
        }

        public FilterLoadProductionPlanListModel FilterLoadProductionPlan(int? productId, int? routeId, int? statusId,string? planId)
        {
            var output = new FilterLoadProductionPlanListModel();
            Dictionary<string, object> parameterList = new Dictionary<string, object>()
                {
                    {"@routeid", routeId},
                    {"@productid", productId},
                    {"@statusid", statusId},
                    {"@planid", planId }
                };
            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListFilterLoadProductionPlan", parameterList);
            if (dt.Rows.Count > 0)
            {
                output.Products = dt.AsEnumerable().Select(x => new { id = x.Field<int>("productid"), name = x.Field<string>("productcode") }).Distinct().ToDictionary(x => x.id, y => y.name);
                output.Routes = dt.AsEnumerable().Select(x => new { id = x.Field<int>("routeid"), name = x.Field<string>("routename") }).Distinct().ToDictionary(x => x.id, y => y.name);
                output.Status = dt.AsEnumerable().Select(x => new { id = x.Field<int>("statusid"), name = x.Field<string>("statusname") }).Distinct().ToDictionary(x => x.id, y => y.name);

                var routeList = dt.AsEnumerable().Where(x=>x.Field<string>("PlanId") == planId || string.IsNullOrEmpty(planId)).Select(x => new { id = x.Field<int>("routeid"), name = x.Field<string>("routename"), inProcess = Convert.ToBoolean(x.Field<Int32>("inprocess")) }).Distinct().ToList();
                output.Route = new List<RouteModel>();
                foreach (var item in routeList)
                {
                    output.Route.Add(new RouteModel
                    {
                        Id = item.id,
                        Name = item.name,
                        InProcess = item.inProcess
                    });
                }
            }
            return output;
        }

        public async Task<ProductionPlanModel> Load(string id, int routeId)
        {
            return await Task.Run(() =>
            {
                var output = new ProductionPlanModel();
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                {
                    {"@planid", id},
                    {"@routeid", routeId}
                };
                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_ActiveProductionPlan_Info", parameterList);
                if (dt.Rows.Count > 0)
                    output = dt.ToModel<ProductionPlanModel>()[0];
                return output;
            });
        }

        public async Task<List<ProductionPlanListModel>> ListByMonth(int month, int year)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@month", month},
                                            {"@year", year}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProductionPlanBYMonth", parameterList);

                return (dt.ToModel<ProductionPlanListModel>());
            });
        }

        public async Task<PagingModel<ProductionPlanListModel>> ListByDate(DateTime date, int page, int howmany)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@date", date},
                                            {"@howmany", howmany},
                                            { "@page", page}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListProductionPlanByDate", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<ProductionPlanListModel>(), totalCount, page, howmany);
            });
        }
    }
}