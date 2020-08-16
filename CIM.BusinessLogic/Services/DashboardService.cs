using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services {
    public class DashboardService: BaseService, IDashboardService {

        private IDirectSqlRepository _directSqlRepository;
        private IResponseCacheService _responseCacheService;
        public DashboardService(IDirectSqlRepository directSqlRepository,
            IResponseCacheService responseCacheService)
        {
            _directSqlRepository = directSqlRepository;
            _responseCacheService = responseCacheService;
        }

        #region Config

        private Dictionary<ManagementDashboardType, getdataConfig> ManagementDashboardConfig
            = new Dictionary<ManagementDashboardType, getdataConfig>()
            {
                                { ManagementDashboardType.KPI, new getdataConfig("KPI","sp_dashboard_kpi")},//MachineLossHighLight
                                { ManagementDashboardType.ProductionSummary, new getdataConfig("ProductionSummary","sp_Dashboard_Output")},
                                { ManagementDashboardType.WasteByMaterial, new getdataConfig("WasteByMaterial","sp_Dashboard_WasteByMaterial")},
                                { ManagementDashboardType.WasteBySymptom, new getdataConfig("WasteBySymptom","sp_Dashboard_WasteBySymptom")},
                                { ManagementDashboardType.MachineLossTree, new getdataConfig("MachineLossTree","sp_Dashboard_MachineLossTree")},
                                { ManagementDashboardType.MachineLossLvl1, new getdataConfig("MachineLossLvl1","-")},
                                { ManagementDashboardType.MachineLossLvl2, new getdataConfig("MachineLossLvl2","sp_Dashboard_MachineLossLvl2")},
                                { ManagementDashboardType.MachineLossLvl3, new getdataConfig("MachineLossLvl3","-")},
                                { ManagementDashboardType.CapacityUtilization, new getdataConfig("CapacityUtilization","sp_Dashboard_Utilization")},
                                { ManagementDashboardType.MachineLossHighLight, new getdataConfig("MachineLossHighLight","sp_Dashboard_MachineLoss_Highlight")},
            };

        private Dictionary<BoardcastType, getdataConfig> DashboardConfig
            = new Dictionary<BoardcastType, getdataConfig>()
            {
                { BoardcastType.KPI, new getdataConfig("KPI","sp_dashboard_kpi")},
                { BoardcastType.Output, new getdataConfig("Output","sp_dashboard_output")},
                { BoardcastType.Waste, new getdataConfig("Waste","sp_dashboard_waste")},
                { BoardcastType.Loss, new getdataConfig("MachineLoss","sp_dashboard_machineLoss")},
                { BoardcastType.TimeUtilisation, new getdataConfig("Utilization","sp_dashboard_utilization")},
                { BoardcastType.ActiveKPI, new getdataConfig("KPI","sp_Report_Production_Dashboard")},
                { BoardcastType.ActiveProductionSummary, new getdataConfig("ProductionSummary","sp_report_productionsummary")},
                { BoardcastType.ActiveProductionOutput, new getdataConfig("ProductionOutput","sp_dashboard_output")},
                { BoardcastType.ActiveWasteMat, new getdataConfig("WastebyMat","sp_report_waste_materials")},
                { BoardcastType.ActiveWasteCase, new getdataConfig("WastebyCase","sp_report_waste_cases")},
                { BoardcastType.ActiveWasteMC, new getdataConfig("WastebyMC","sp_report_waste_machines")},
                { BoardcastType.ActiveWasteTime, new getdataConfig("WastebyTime","sp_report_waste_cost_time")},
                { BoardcastType.ActiveLoss, new getdataConfig("MachineLoss","sp_Report_WCMLosses")},
                { BoardcastType.ActiveTimeUtilisation, new getdataConfig("Utilization","sp_report_capacity_ultilisation")},
                { BoardcastType.ActiveProductionEvent, new getdataConfig("ProductionEvent","sp_report_productionevents")},
                { BoardcastType.ActiveOperator, new getdataConfig("Operator","sp_report_productionoperators")},
                { BoardcastType.ActiveMachineInfo, new getdataConfig("MachineInfo","sp_report_active_machineinfo")},
                { BoardcastType.ActiveMachineSpeed, new getdataConfig("MachineSpeed","sp_report_machinespeed")},
                { BoardcastType.ActiveMachineStatus, new getdataConfig("MachineStatus","sp_Report_Machine_Status")},
                { BoardcastType.ActiveMachineLossEvent, new getdataConfig("MachineLossEvent","sp_report_active_machineevent")},
            };

        #endregion

        #region  Cim-Mng dashboard

        public async Task<BoardcastModel> GenerateBoardcastManagementData(DataFrame timeFrame, BoardcastType updateType)
        {
            var boardcastData = new BoardcastModel(timeFrame);
            var paramsList = new Dictionary<string, object>() { { "@timeFrame", timeFrame } };
            return await Task.Run(() =>
            {
                try
                {
                    switch (updateType)
                    {
                        case BoardcastType.All:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.KPI
                                                                , BoardcastType.Output
                                                                , BoardcastType.Loss
                                                                , BoardcastType.TimeUtilisation
                                                                , BoardcastType.Waste}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.KPI:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.KPI }
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Output:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.Output
                                                                    , BoardcastType.KPI}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Loss:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.Loss
                                                                    , BoardcastType.KPI
                                                                    , BoardcastType.TimeUtilisation}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.TimeUtilisation:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.TimeUtilisation }
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Waste:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.Waste }
                                                            , timeFrame, paramsList);
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

        public async Task<BoardcastModel> GetManagementDashboard(DataFrame frame)
        {
            var cacheKey = $"mngdashboard-{frame}";
            var dashboard = await _responseCacheService.GetAsTypeAsync<BoardcastModel>(cacheKey);
            if (dashboard == null || dashboard.LastUpdate.AddMinutes(1) < DateTime.Now)
            {
                dashboard = new BoardcastModel();
                var paramsList = new Dictionary<string, object>() { { "@timeFrame", (int)frame } };
                switch (frame)
                {
                    case DataFrame.Default:
                        dashboard = GenerateDashboardData(
                             new[]{ ManagementDashboardType.KPI
                                 , ManagementDashboardType.WasteByMaterial
                                 , ManagementDashboardType.ProductionSummary
                                 , ManagementDashboardType.MachineLossTree
                                 , ManagementDashboardType.MachineLossHighLight
                                 , ManagementDashboardType.CapacityUtilization}
                             , frame, paramsList);
                        break;
                    case DataFrame.Daily:
                        break;
                    case DataFrame.Weekly:
                        break;
                    case DataFrame.Monthly:
                        break;
                    case DataFrame.Yearly:
                        break;
                    case DataFrame.Custom:
                        break;
                }
            }

            //return  _directSqlRepository.ExecuteSPWithQuery("sp_Dashboard_Management", null);
            return dashboard;
        }

        private BoardcastModel GenerateDashboardData(ManagementDashboardType[] dashboardType, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var managementData = new BoardcastModel(timeFrame);
            foreach (var dbtype in dashboardType)
            {
                managementData.Data.Add(
                                        GetDashboardData(ManagementDashboardConfig[dbtype]
                                        , timeFrame, paramsList));
            }
            return managementData;
        }

        private BoardcastDataModel GetDashboardData(getdataConfig dashboardConfig, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var dashboarddata = new BoardcastDataModel();
            try
            {
                dashboarddata.Name = dashboardConfig.Name;
                dashboarddata.JsonData = JsonConvert.SerializeObject(
                                        _directSqlRepository.ExecuteSPWithQuery(dashboardConfig.StoreName, paramsList));
            }
            catch (Exception ex)
            {
                dashboarddata.JsonData = null;
                dashboarddata.IsSuccess = false;
                dashboarddata.Message = ex.Message;
            }
            return dashboarddata;
        }

        //private BoardcastDataModel GetData(DashboardConfig dashboardConfig, DataFrame timeFrame, Dictionary<string, object> paramsList)
        //{
        //    var dashboarddata = new BoardcastDataModel();
        //    try
        //    {
        //        dashboarddata.Name = dashboardConfig.Name;
        //        dashboarddata.JsonData = JsonConvert.SerializeObject(
        //                                _directSqlRepository.ExecuteSPWithQuery(dashboardConfig.StoreName, paramsList));
        //    }
        //    catch (Exception ex)
        //    {
        //        dashboarddata.JsonData = null;
        //        dashboarddata.IsSuccess = false;
        //        dashboarddata.Message = ex.Message;
        //    }
        //    return dashboarddata;
        //}


        #endregion

        #region Cim-Oper boardcast data

        public async Task<ActiveProductionPlanModel> GenerateBoardcastOperationData(BoardcastType updateType, string productionPlan, int routeId)
        {
            var boardcastData = await GenerateBoardcastData(updateType, productionPlan, routeId);
            if (boardcastData.Data.Count > 0)
            {
                //to add data to activeproduction plan
                var activeProductionPlan = new ActiveProductionPlanModel(productionPlan);
            }
            return null;
        }

        public async Task<BoardcastModel> GenerateBoardcastData(BoardcastType updateType, string productionPlan, int routeId)
        {
            var boardcastData = new BoardcastModel();
            var paramsList = new Dictionary<string, object>() { { "@planid", productionPlan }, { "@routeid", routeId } };
            return await Task.Run(() =>
            {
                try
                {
                    switch (updateType)
                    {
                        case BoardcastType.All:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveProductionSummary
                                                                , BoardcastType.ActiveProductionOutput
                                                                , BoardcastType.ActiveWasteMat
                                                                , BoardcastType.ActiveWasteCase
                                                                , BoardcastType.ActiveWasteMC
                                                                , BoardcastType.ActiveWasteTime
                                                                , BoardcastType.ActiveLoss
                                                                , BoardcastType.ActiveTimeUtilisation
                                                                , BoardcastType.ActiveProductionEvent
                                                                , BoardcastType.ActiveOperator
                                                                , BoardcastType.ActiveMachineInfo
                                                                , BoardcastType.ActiveMachineSpeed
                                                                , BoardcastType.ActiveMachineLossEvent}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveKPI:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.ActiveKPI }
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveProductionSummary:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveMachineInfo
                                                                , BoardcastType.ActiveProductionSummary
                                                                , BoardcastType.ActiveProductionOutput
                                                                , BoardcastType.ActiveMachineSpeed}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveLoss:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveLoss
                                                                , BoardcastType.ActiveTimeUtilisation
                                                                , BoardcastType.ActiveProductionEvent
                                                                , BoardcastType.ActiveMachineInfo}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveTimeUtilisation:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.ActiveTimeUtilisation }
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveWaste:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveWasteMat
                                                                , BoardcastType.ActiveWasteCase
                                                                , BoardcastType.ActiveWasteMC
                                                                , BoardcastType.ActiveWasteTime}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveProductionEvent:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.ActiveProductionEvent }
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveOperator:
                            boardcastData = GenerateBoardcastData(
                                                            new[] { BoardcastType.ActiveOperator }
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveMachineInfo:
                        case BoardcastType.ActiveMachineSpeed:
                        case BoardcastType.ActiveMachineLossEvent:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{BoardcastType.ActiveMachineInfo
                                                                , BoardcastType.ActiveMachineSpeed
                                                                , BoardcastType.ActiveMachineLossEvent
                                                                , BoardcastType.ActiveProductionEvent
                                                                , BoardcastType.ActiveTimeUtilisation}
                                                            , DataFrame.Default, paramsList);
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

        private BoardcastModel GenerateBoardcastData(BoardcastType[] dashboardType, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var boardcastData = new BoardcastModel(timeFrame);
            foreach (var dbtype in dashboardType)
            {
                boardcastData.SetData(
                                        GetDashboardData(DashboardConfig[dbtype]
                                        , timeFrame, paramsList));
            }
            return boardcastData;
        }


        #endregion

        #region Common fn


        #endregion

        #region  Cim-Oper Production overview

        public DataTable GetActiveMachineInfo(string planId, int routeId)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_active_machineinfo", paramsList);
        }

        public DataTable GetActiveMachineEvents(string planId, int routeId)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_active_machineevent", paramsList);
        }

        public DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? machineId, int? lossId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@losslv", lossLv },
                {"@mcid", machineId },
                {"@from", from },
                {"@to", to },
                {"@lossid", lossId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
        }

        public DataTable GetProductionDasboard(string planId, int routeId, int machineId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Dashboard", paramsList);
        }

        public DataTable GetProductionSummary(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionsummary", paramsList);
        }

        public DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionevents", paramsList);
        }

        public DataTable GetProductionOperators(string planId, int routeId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionoperators", paramsList);
        }

        public DataTable GetProductionPlanInfomation(string planId, int routeId)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productioninfo", paramsList);
        }

        public DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_machinespeed", paramsList);
        }

        public DataTable GetCapacityUtilisation(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_capacity_ultilisation", paramsList);
        }

        #endregion

        #region Cim-oper waste

        public DataTable GetWasteByMaterials(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_materials", paramsList);
        }

        public DataTable GetWasteByCases(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_cases", paramsList);
        }

        public DataTable GetWasteByMachines(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_machines", paramsList);
        }

        public DataTable GetWasteCostByTime(string planId, int routeId, DateTime? from, DateTime? to)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_cost_time", paramsList);
        }

        #endregion

    }
}
