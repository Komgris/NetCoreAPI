using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IDirectSqlRepository _directSqlRepository;

        public ReportService(IDirectSqlRepository directSqlRepository)
        {
            _directSqlRepository = directSqlRepository;
        }

        #region  Cim-Oper Production overview

        public DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionevents", paramsList);
        }

        public DataTable GetProductionOperators(string planId, int routeId) {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionoperators", paramsList);
        }

        public DataTable GetProductionPlanInfomation(string planId, int routeId) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productioninfo", paramsList);
        }

        public DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_machinespeed", paramsList);
        }

        public DataTable GetCapacityUtilisation(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_capacity_ultilisation", paramsList);
        }
        #endregion

        #region  Cim-Oper Mc-Loss

        public DataTable GetProductionSummary(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionsummary", paramsList);
        }

        public DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? machineId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@losslv", lossLv },
                {"@mcid", machineId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
        }

        public PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to },
                {"@page", page }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
            var totalcnt = dt.Rows[0].Field<int>("totalcount");
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, 10);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }

        #endregion

        #region  Cim-Oper dashboard
        public DataTable GetProductionDasboard(string planId, int routeId, int machineId) {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Dashboard", paramsList);
        }
        public Dictionary<string, int> GetActiveProductionPlanOutput() {
            return _directSqlRepository.ExecuteSPWithQuery("sp_report_activeproductionplan_output", null).AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), r => r.Field<int>(1)); ;
        }

        #endregion

        #region Cim-oper waste

        public DataTable GetWasteByMaterials(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_materials", paramsList);
        }

        public DataTable GetWasteByCases(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_cases", paramsList);
        }

        public DataTable GetWasteByMachines(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_machines", paramsList);
        }

        public DataTable GetWasteCostByTime(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_cost_time", paramsList);
        }

        public PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to },
                {"@page", page }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_history", paramsList);
            var totalcnt = dt.Rows[0].Field<int>("totalcount");
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, 10);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }

        #endregion

        #region Cim-oper mc status
        
        public DataTable GetActiveMachineInfo(string planId, int routeId) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_active_machineinfo", paramsList);
        }
        public DataTable GetActiveMachineEvents(string planId, int routeId) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_active_machineevent", paramsList);
        }

        public PagingModel<object> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null) {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId },
                {"@from", from },
                {"@to", to },
                {"@page", page },
                {"@howmany", howMany }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_Machine_Status", paramsList);
            var totalcnt = dt.Rows[0].Field<int>("totalcount");
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, howMany);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }
        #endregion

        #region Cim-Oper boardcast data
        public async Task<BoardcastModel> GenerateBoardcastOperationData(DashboardDataFrame type, DashboardType updateType, string productionPlan, int routeId)
        {
            var boardcastData = new BoardcastModel(type);
            return await Task.Run(() =>
            {
                try
                {
                    var paramsList = new Dictionary<string, object>() { { "@type", type } };
                    switch (updateType)
                    {
                        case DashboardType.All:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.KPI], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Output], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Loss], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.TimeUtilisation], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Waste], type, paramsList));
                            break;
                        case DashboardType.KPI:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.KPI], type, paramsList));
                            break;
                        case DashboardType.Output:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Output], type, paramsList));
                            break;
                        case DashboardType.Loss:
                        case DashboardType.TimeUtilisation:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Loss], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.TimeUtilisation], type, paramsList));
                            break;
                        case DashboardType.Waste:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Waste], type, paramsList));
                            break;
                    }

                }
                catch (Exception ex)
                {
                    boardcastData.IsSuccess = false;
                    boardcastData.Message = ex.Message;
                }

                return boardcastData;
            }

        #endregion

        #region Cim-Mng boardcast data

        public async Task<BoardcastModel> GenerateBoardcastManagementData(DashboardDataFrame type, DashboardType updateType)
        {
            var boardcastData = new BoardcastModel(type);
            return await Task.Run(() =>
            {
                try
                {
                    var paramsList = new Dictionary<string, object>() { { "@type", type } };
                    switch (updateType)
                    {
                        case DashboardType.All:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.KPI], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Output], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Loss], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.TimeUtilisation], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Waste], type, paramsList));
                            break;
                        case DashboardType.KPI:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.KPI], type, paramsList));
                            break;
                        case DashboardType.Output:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Output], type, paramsList));
                            break;
                        case DashboardType.Loss:
                        case DashboardType.TimeUtilisation:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Loss], type, paramsList));
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.TimeUtilisation], type, paramsList));
                            break;
                        case DashboardType.Waste:
                            boardcastData.SetDashboard(GetDashboardData(Dashboard[DashboardType.Waste], type, paramsList));
                            break;
                    }

                }
                catch (Exception ex)
                {
                    boardcastData.IsSuccess = false;
                    boardcastData.Message = ex.Message;
                }

                return boardcastData;
            });
        }

        private BoardcastDataModel GetDashboardData(DashboardConfig dashboardConfig, DashboardDataFrame type, Dictionary<string, object> paramsList)
        {
            var dashboarddata = new BoardcastDataModel();
            try
            {
                dashboarddata.Name = dashboardConfig.Name;
                dashboarddata.JsonData = JsonConvert.SerializeObject(_directSqlRepository.ExecuteSPWithQuery(dashboardConfig.StoreName, paramsList));
            }
            catch (Exception ex)
            {
                dashboarddata.JsonData = null;
                dashboarddata.IsSuccess = false;
                dashboarddata.Message = ex.Message;
            }
            return dashboarddata;
        }

        #endregion
    
    }
}
