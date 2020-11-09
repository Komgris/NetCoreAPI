using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Interfaces {
    public interface IReportService : IBaseService {
        PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page);
        Dictionary<string, int> GetActiveProductionPlanOutput();
        PagingModel<object> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null);
        BoardcastDataModel GetActiveMachineInfo(string planId, int routeId, int machineId);
        DataTable GetOEEReport(ReportTimeCriteriaModel data);
        DataTable GetOutputReport(ReportTimeCriteriaModel data);
        DataTable GetWasteReport(ReportTimeCriteriaModel data);
        DataTable GetMachineLossReport(ReportTimeCriteriaModel data);
        DataTable GetQualityReport(ReportTimeCriteriaModel data);
        DataTable GetSPCReport(ReportTimeCriteriaModel data);
        DataTable GetElectricityReport(ReportTimeCriteriaModel data);
        DataTable GetProductionSummaryReport(ReportTimeCriteriaModel data);
        DataTable GetOperatingTimeReport(ReportTimeCriteriaModel data);
        DataTable GetActualDesignSpeedReport(ReportTimeCriteriaModel data);
        DataTable GetMaintenanceReport(ReportTimeCriteriaModel data);
        DataTable GetCostAnalysisReport(ReportTimeCriteriaModel data);
        DataTable GetHSEReport(ReportTimeCriteriaModel data);
        DataTable GetAttendantReport(ReportTimeCriteriaModel data);

    }

}