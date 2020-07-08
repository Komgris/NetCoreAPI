using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IDirectSqlRepository _directSqlRepository;

        private JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private Dictionary<BoardcastType, DashboardConfig> DashboardConfig
            = new Dictionary<BoardcastType, DashboardConfig>()
            {
                { BoardcastType.KPI, new DashboardConfig("KPI","sp_dashboard_kpi")},
                { BoardcastType.Output, new DashboardConfig("Output","sp_dashboard_output")},
                { BoardcastType.Waste, new DashboardConfig("Waste","sp_dashboard_waste")},
                { BoardcastType.Loss, new DashboardConfig("MachineLoss","sp_dashboard_machineLoss")},
                { BoardcastType.TimeUtilisation, new DashboardConfig("Utilization","sp_dashboard_utilization")},

                { BoardcastType.ActiveKPI, new DashboardConfig("KPI","sp_Report_Production_Dashboard")},
                { BoardcastType.ActiveProductionSummary, new DashboardConfig("ProductionSummary","sp_report_productionsummary")},
                { BoardcastType.ActiveProductionOutput, new DashboardConfig("ProductionOutput","sp_dashboard_output")},
                { BoardcastType.ActiveWasteMat, new DashboardConfig("WastebyMat","sp_report_waste_materials")},
                { BoardcastType.ActiveWasteCase, new DashboardConfig("WastebyCase","sp_report_waste_cases")},
                { BoardcastType.ActiveWasteMC, new DashboardConfig("WastebyMC","sp_report_waste_machines")},
                { BoardcastType.ActiveWasteTime, new DashboardConfig("WastebyTime","sp_report_waste_cost_time")},
                { BoardcastType.ActiveLoss, new DashboardConfig("MachineLoss","sp_Report_WCMLosses")},
                { BoardcastType.ActiveTimeUtilisation, new DashboardConfig("Utilization","sp_report_capacity_ultilisation")},
                { BoardcastType.ActiveProductionEvent, new DashboardConfig("ProductionEvent","sp_report_productionevents")},
                { BoardcastType.ActiveOperator, new DashboardConfig("Operator","sp_report_productionoperators")},
                { BoardcastType.ActiveMachineInfo, new DashboardConfig("MachineInfo","sp_report_active_machineinfo")},
                { BoardcastType.ActiveMachineSpeed, new DashboardConfig("MachineSpeed","sp_report_machinespeed")},
                { BoardcastType.ActiveMachineStatus, new DashboardConfig("MachineStatus","sp_Report_Machine_Status")},
                { BoardcastType.ActiveMachineLossEvent, new DashboardConfig("MachineLossEvent","sp_report_active_machineevent")},
            };

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

        public DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? machineId,int? lossId, DateTime? from, DateTime? to) {

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

            var dt =  _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_history", paramsList);
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
                                                            new[]{ BoardcastType.ActiveKPI}
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
                                                            new[]{ BoardcastType.ActiveTimeUtilisation}
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
                                                            new[]{BoardcastType.ActiveProductionEvent}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveOperator:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{ BoardcastType.ActiveOperator}
                                                            , DataFrame.Default, paramsList);
                            break;
                        case BoardcastType.ActiveMachineInfo:
                        case BoardcastType.ActiveMachineSpeed:
                        case BoardcastType.ActiveMachineLossEvent:
                            boardcastData = GenerateBoardcastData(
                                                            new[]{BoardcastType.ActiveMachineInfo
                                                                , BoardcastType.ActiveMachineSpeed
                                                                , BoardcastType.ActiveMachineLossEvent}
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

        #region Cim-Mng boardcast data

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
                                                                new []{ BoardcastType.KPI
                                                                , BoardcastType.Output
                                                                , BoardcastType.Loss
                                                                , BoardcastType.TimeUtilisation
                                                                , BoardcastType.Waste}
                                                                , timeFrame, paramsList);
                                break;
                            case BoardcastType.KPI:
                                boardcastData = GenerateBoardcastData(
                                                                new[]{ BoardcastType.KPI}
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
                                                                new[] { BoardcastType.TimeUtilisation}
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

        private BoardcastModel GenerateBoardcastData(BoardcastType[] dashboardType, DataFrame timeFrame, Dictionary<string, object> paramsList)
        {
            var boardcastData = new BoardcastModel(timeFrame);
            foreach (var dbtype in dashboardType)
            {
                boardcastData.SetData(
                                        GetData(DashboardConfig[dbtype]
                                        , timeFrame, paramsList));
            }
            return boardcastData;
        }

        private BoardcastDataModel GetData(DashboardConfig dashboardConfig, DataFrame timeFrame, Dictionary<string, object> paramsList)
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

        #endregion

        #region Cim-Mng Report

        public DataTable GetOEEReport(ReportTimeCriteriaModel data)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_OEE", paramsList);
        }

        public DataTable GetOutputReport(ReportTimeCriteriaModel data)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Output", paramsList);
        }

        public DataTable GetWasteReport(ReportTimeCriteriaModel data)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Waste", paramsList);
        }

        public DataTable GetMachineLossReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Machine_Loss", paramsList);
        }

        public DataTable GetQualityReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Quality", paramsList);
        }

        public DataTable GetSPCReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_SPC", paramsList);
        }

        public DataTable GetElectricityReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Electricity", paramsList);
        }

        public DataTable GetProductionSummaryReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Summary", paramsList);
        }

        public DataTable GetOperatingTimeReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Operating_Time", paramsList);
        }

        public DataTable GetActualDesignSpeedReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Actual_Design_Speed", paramsList);
        }

        public DataTable GetMaintenanceReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Maintenance", paramsList);
        }

        public DataTable GetCostAnalysisReport(ReportTimeCriteriaModel data)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@report_type", data.ReportType },
                {"@date_from", data.DateFrom },
                {"@date_to", data.DateTo },
                {"@week_from", data.WeekFrom },
                {"@week_to", data.WeekTo },
                {"@month_from", data.MonthFrom },
                {"@month_to", data.MonthTo },
                {"@year_from", data.YearFrom },
                {"@year_to", data.YearTo },
                {"@ipd_from", data.IPDFrom },
                {"@ipd_to", data.IPDTo },
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Cost_Analysis", paramsList);
        }

        #endregion

    }
}
