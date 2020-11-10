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
                { ManagementDashboardType.KPI                   , new getdataConfig("KPI","sp_dashboard_kpi")},//MachineLossHighLight
                { ManagementDashboardType.ProductionSummary     , new getdataConfig("ProductionSummary","sp_Dashboard_Output")},
                { ManagementDashboardType.WasteByMaterial       , new getdataConfig("WasteByMaterial","sp_Dashboard_WasteByMaterial")},
                { ManagementDashboardType.WasteBySymptom        , new getdataConfig("WasteBySymptom","sp_Dashboard_WasteBySymptom")},
                { ManagementDashboardType.MachineLossTree       , new getdataConfig("MachineLossTree","sp_Dashboard_MachineLossTree")},
                { ManagementDashboardType.MachineLossLvl1       , new getdataConfig("MachineLossLvl1","-")},
                { ManagementDashboardType.MachineLossLvl2       , new getdataConfig("MachineLossLvl2","sp_Dashboard_MachineLossLvl2")},
                { ManagementDashboardType.MachineLossLvl3       , new getdataConfig("MachineLossLvl3","-")},
                { ManagementDashboardType.CapacityUtilization   , new getdataConfig("CapacityUtilization","sp_Dashboard_Utilization")},
                { ManagementDashboardType.MachineLossHighLight  , new getdataConfig("MachineLossHighLight","sp_Dashboard_MachineLoss_Highlight")},
            };

        private Dictionary<CustomDashboardType, getdataConfig> CustomDashboardConfig
            = new Dictionary<CustomDashboardType, getdataConfig>()
            {
                { CustomDashboardType.OEE           , new getdataConfig("OEE","sp_custom_dashboard_oee")},
                { CustomDashboardType.Production    , new getdataConfig("Production","sp_custom_dashboard_production")},
                { CustomDashboardType.HSE           , new getdataConfig("HSE","sp_custom_dashboard_hse")},
                { CustomDashboardType.Quality       , new getdataConfig("Quality","sp_custom_dashboard_quality")},
                { CustomDashboardType.Delivery      , new getdataConfig("Delivery","sp_custom_dashboard_delivery")},
                { CustomDashboardType.Spoilage      , new getdataConfig("Spoilage","sp_custom_dashboard_spoilage")},
                { CustomDashboardType.NonePrime     , new getdataConfig("NonePrime","sp_custom_dashboard_noneprime")},
                { CustomDashboardType.Attendance    , new getdataConfig("Attendance","sp_custom_dashboard_attendance")},
                { CustomDashboardType.MachineStatus , new getdataConfig("MachineStatus","sp_custom_dashboard_machine")},
                { CustomDashboardType.PlanvsActual  , new getdataConfig("PlanvsActual","sp_custom_dashboard_planactual")},
            };

        private Dictionary<BoardcastType, getdataConfig> DashboardConfig
            = new Dictionary<BoardcastType, getdataConfig>()
            {
                { BoardcastType.KPI                     , new getdataConfig("KPI","sp_dashboard_kpi")},
                { BoardcastType.Output                  , new getdataConfig("Output","sp_dashboard_output")},
                { BoardcastType.Waste                   , new getdataConfig("Waste","sp_dashboard_waste")},
                { BoardcastType.Loss                    , new getdataConfig("MachineLoss","sp_dashboard_machineLoss")},
                { BoardcastType.TimeUtilisation         , new getdataConfig("Utilization","sp_dashboard_utilization")},
                { BoardcastType.ActiveKPI               , new getdataConfig("KPI","sp_Report_Production_Dashboard")},
                { BoardcastType.ActiveProductionSummary , new getdataConfig("ProductionSummary","sp_report_productionsummary")},
                { BoardcastType.ActiveProductionOutput  , new getdataConfig("ProductionOutput","sp_dashboard_output")},
                { BoardcastType.ActiveWasteMat          , new getdataConfig("WastebyMat","sp_report_waste_materials")},
                { BoardcastType.ActiveWasteCase         , new getdataConfig("WastebyCase","sp_report_waste_cases")},
                { BoardcastType.ActiveWasteMC           , new getdataConfig("WastebyMC","sp_report_waste_machines")},
                { BoardcastType.ActiveWasteTime         , new getdataConfig("WastebyTime","sp_report_waste_cost_time")},
                { BoardcastType.ActiveLoss              , new getdataConfig("MachineLoss","sp_Report_WCMLosses")},
                { BoardcastType.ActiveTimeUtilisation   , new getdataConfig("Utilization","sp_report_capacity_ultilisation")},
                { BoardcastType.ActiveProductionEvent   , new getdataConfig("ProductionEvent","sp_report_productionevents")},
                { BoardcastType.ActiveOperator          , new getdataConfig("Operator","sp_report_productionoperators")},
                { BoardcastType.ActiveMachineInfo       , new getdataConfig("MachineInfo","sp_report_active_machineinfo")},
                { BoardcastType.ActiveMachineSpeed      , new getdataConfig("MachineSpeed","sp_report_machinespeed")},
                { BoardcastType.ActiveMachineStatus     , new getdataConfig("MachineStatus","sp_Report_Machine_Status")},
                { BoardcastType.ActiveMachineLossEvent  , new getdataConfig("MachineLossEvent","sp_report_active_machineevent")},
            };

        #endregion

        #region General

        private ProductionDataModel GetOptionData(BoardcastType[] dashboardType, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var boardcastData = new ProductionDataModel(timeFrame);
            foreach (var dbtype in dashboardType)
            {
                boardcastData.SetData(
                                       GetdData(DashboardConfig[dbtype]
                                            , paramsList));
            }
            return boardcastData;
        }

        private UnitDataModel GetdData(getdataConfig dashboardConfig, Dictionary<string, object> paramsList)
        {
            var dashboarddata = new UnitDataModel();
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

        private UnitDataModel GetdDataSpecific(getdataConfig dashboardConfig, Dictionary<string, object> paramsList)
        {
            var dashboarddata = new UnitDataModel();
            try
            {
                dashboarddata.Name = dashboardConfig.Name;
                var dSet = _directSqlRepository.ExecuteSPWithQueryDSet(dashboardConfig.StoreName, paramsList);

                if(dSet.Tables.Count > 0) dashboarddata.JsonData = JsonConvert.SerializeObject(dSet.Tables[0]);
                if(dSet.Tables.Count > 1) dashboarddata.JsonSpecificData = JsonConvert.SerializeObject(dSet.Tables[1]);
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

        #region CIM dashboard

        public async Task<ProductionDataModel> GenerateCustomDashboard(DataTypeGroup updateType)
        {
            var boardcastData = new ProductionDataModel();

            return await Task.Run(() =>
            {
                try
                {
                    switch (updateType)
                    {
                        case DataTypeGroup.All:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.OEE
                                                                , CustomDashboardType.Production
                                                                , CustomDashboardType.HSE
                                                                , CustomDashboardType.Quality
                                                                , CustomDashboardType.Delivery
                                                                , CustomDashboardType.Spoilage
                                                                , CustomDashboardType.NonePrime
                                                                , CustomDashboardType.Attendance
                                                                , CustomDashboardType.MachineStatus
                                                                , CustomDashboardType.PlanvsActual});
                            break;
                        case DataTypeGroup.HSE:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.HSE});
                            break;
                        case DataTypeGroup.Operators:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.Attendance
                                                                , CustomDashboardType.PlanvsActual});
                            break;

                        case DataTypeGroup.Machine:
                            boardcastData = CustomDashboard(
                                                            new[]{ 
                                                                CustomDashboardType.MachineStatus});
                            break;

                        case DataTypeGroup.PlanActual:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.Production
                                                                , CustomDashboardType.PlanvsActual});
                            break;
                        case DataTypeGroup.McCalc:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.OEE
                                                                , CustomDashboardType.Delivery});
                            break;
                        case DataTypeGroup.ProduceCalc:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.OEE
                                                                , CustomDashboardType.Delivery
                                                                , CustomDashboardType.PlanvsActual});
                            break;
                        case DataTypeGroup.Waste:
                            boardcastData = CustomDashboard(
                                                            new[]{ CustomDashboardType.Spoilage
                                                                , CustomDashboardType.NonePrime
                                                                , CustomDashboardType.Quality
                                                                , CustomDashboardType.PlanvsActual});
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

        private ProductionDataModel CustomDashboard(CustomDashboardType[] dashboardType)
        {
            var customtData = new ProductionDataModel();
            foreach (var dbtype in dashboardType)
            {
                customtData.UnitData.Add(
                                        GetdDataSpecific(CustomDashboardConfig[dbtype], null));
            }
            return customtData;
        }

        #endregion

        #region  Cim-Mng dashboard

        public async Task<ProductionDataModel> GenerateBoardcastManagementData(DataFrame timeFrame, BoardcastType updateType)
        {
            var boardcastData = new ProductionDataModel(timeFrame);
            var paramsList = new Dictionary<string, object>() { { "@timeFrame", timeFrame } };
            return await Task.Run(() =>
            {
                try
                {
                    switch (updateType)
                    {
                        case BoardcastType.All:
                            boardcastData = GetOptionData(
                                                            new[]{ BoardcastType.KPI
                                                                , BoardcastType.Output
                                                                , BoardcastType.Loss
                                                                , BoardcastType.TimeUtilisation
                                                                , BoardcastType.Waste}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.KPI:
                            boardcastData = GetOptionData(
                                                            new[] { BoardcastType.KPI }
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Output:
                            boardcastData = GetOptionData(
                                                            new[] { BoardcastType.Output
                                                                    , BoardcastType.KPI}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Loss:
                            boardcastData = GetOptionData(
                                                            new[] { BoardcastType.Loss
                                                                    , BoardcastType.KPI
                                                                    , BoardcastType.TimeUtilisation}
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.TimeUtilisation:
                            boardcastData = GetOptionData(
                                                            new[] { BoardcastType.TimeUtilisation }
                                                            , timeFrame, paramsList);
                            break;
                        case BoardcastType.Waste:
                            boardcastData = GetOptionData(
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

        public async Task<ProductionDataModel> GetManagementDashboard(DataFrame frame)
        {
            var cacheKey = $"mngdashboard-{frame}";
            var dashboard = await _responseCacheService.GetAsTypeAsync<ProductionDataModel>(cacheKey);
            if (dashboard == null || dashboard.LastUpdate.AddMinutes(1) < DateTime.Now)
            {
                dashboard = new ProductionDataModel();
                var paramsList = new Dictionary<string, object>() { { "@timeFrame", (int)frame } };
                switch (frame)
                {
                    case DataFrame.Default:
                        dashboard = await GenerateDashboardData(
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

        private async Task<ProductionDataModel> GenerateDashboardData(ManagementDashboardType[] dashboardType, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var managementData = new ProductionDataModel(timeFrame);
            foreach (var dbtype in dashboardType)
            {
                managementData.UnitData.Add(
                                        GetdData(ManagementDashboardConfig[dbtype]
                                        , paramsList));
            }
            return managementData;
        }

        #endregion

        #region Cim-Oper boardcast data

        public async Task<ActiveProductionPlanModel> GenerateBoardcastOperationData(DataTypeGroup updateType, string productionPlan, int routeId)
        {
            var boardcastData = await GenerateBoardcast(updateType, productionPlan, routeId);
            if (boardcastData.UnitData.Count > 0)
            {
                //to add data to activeproduction plan
                var activeProductionPlan = new ActiveProductionPlanModel(productionPlan);
            }
            return null;
        }

        public async Task<ProductionDataModel> GenerateBoardcast(DataTypeGroup relateType, string productionPlan, int routeId)
        {
            var boardcastData = new ProductionDataModel();
            var paramsList = new Dictionary<string, object>() { { "@planid", productionPlan } };
            return await Task.Run(() =>
            {
                try
                {
                    switch (relateType)
                    {
                        case DataTypeGroup.All:
                            boardcastData = GetOptionData(
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
                        case DataTypeGroup.Produce:
                            boardcastData = GetOptionData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveMachineInfo
                                                                , BoardcastType.ActiveProductionSummary
                                                                , BoardcastType.ActiveProductionOutput
                                                                , BoardcastType.ActiveMachineSpeed}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case DataTypeGroup.Loss:
                            boardcastData = GetOptionData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveLoss
                                                                , BoardcastType.ActiveTimeUtilisation
                                                                , BoardcastType.ActiveProductionEvent
                                                                , BoardcastType.ActiveMachineInfo}
                                                            , DataFrame.Default, paramsList);
                            break;

                        case DataTypeGroup.Waste:
                            boardcastData = GetOptionData(
                                                            new[]{ BoardcastType.ActiveKPI
                                                                , BoardcastType.ActiveWasteMat
                                                                , BoardcastType.ActiveWasteCase
                                                                , BoardcastType.ActiveWasteMC
                                                                , BoardcastType.ActiveWasteTime}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case DataTypeGroup.Process:
                            boardcastData = GetOptionData(
                                                            new[] {  BoardcastType.ActiveKPI
                                                                  , BoardcastType.ActiveProductionEvent
                                                                  , BoardcastType.ActiveTimeUtilisation}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case DataTypeGroup.Operators:
                            boardcastData = GetOptionData(
                                                            new[] { BoardcastType.ActiveOperator }
                                                            , DataFrame.Default, paramsList);
                            break;
                        case DataTypeGroup.Machine:
                            boardcastData = GetOptionData(
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
