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
        private IResponseCacheService _responseCacheService;

        private JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ReportService(IDirectSqlRepository directSqlRepository, 
            IResponseCacheService responseCacheService)
        {
            _directSqlRepository = directSqlRepository;
            _responseCacheService = responseCacheService;
        }

        #region Cim-oper active operation info

        public PagingModel<object> GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to, int page)
        {

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
        public PagingModel<object> GetProductionWCMLossHistory(string planId, int routeId, DateTime? from, DateTime? to, int page)
        {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to },
                {"@page", page }
            };

            var dt = _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
            var totalcnt = dt.Rows.Count > 0 ? dt.Rows[0].Field<int>("totalcount") : 0;
            var pagingmodel = ToPagingModel<object>(null, totalcnt, page, 10);
            pagingmodel.DataObject = dt;

            return pagingmodel;
        }

        public PagingModel<object> GetMachineStatusHistory(int howMany, int page, string planId, int routeId, int? machineId, DateTime? from = null, DateTime? to = null)
        {
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

        public Dictionary<string, int> GetActiveProductionPlanOutput()
        {
            return _directSqlRepository.ExecuteSPWithQuery("sp_report_activeproductionplan_output", null).AsEnumerable().ToDictionary<DataRow, string, int>(row => row.Field<string>(0), r => r.Field<int>(1)); ;
        }
        public BoardcastDataModel GetActiveMachineInfo(string planId, int routeId, int machineId)
        {
            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@mcid", machineId }
            };

            var dset = _directSqlRepository.ExecuteSPWithQueryDSet("sp_Report_Active_Machine_Details", paramsList);
            var result = new BoardcastDataModel
            {
                Name = "MachineDetails",
                JsonData = JsonConvert.SerializeObject(dset.Tables[0]),
                JsonSpecificData = JsonConvert.SerializeObject(dset.Tables[1])
            };

            return result;
        }

        #endregion

        #region Cim-Mng Report
        private Dictionary<string, object> ReportCreiteria(ReportTimeCriteriaModel data)
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

            return paramsList;
        }

        public DataTable GetOEEReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_OEE", paramsList);
        }

        public DataTable GetOutputReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Output", paramsList);
        }

        public DataTable GetWasteReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Waste", paramsList);
        }

        public DataTable GetMachineLossReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Machine_Loss", paramsList);
        }

        public DataTable GetQualityReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Quality", paramsList);
        }

        public DataTable GetSPCReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_SPC", paramsList);
        }

        public DataTable GetElectricityReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Electricity", paramsList);
        }

        public DataTable GetProductionSummaryReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Summary", paramsList);
        }

        public DataTable GetOperatingTimeReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Operating_Time", paramsList);
        }

        public DataTable GetActualDesignSpeedReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Actual_Design_Speed", paramsList);
        }

        public DataTable GetMaintenanceReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Maintenance", paramsList);
        }

        public DataTable GetCostAnalysisReport(ReportTimeCriteriaModel data)
        {
            var paramsList = ReportCreiteria(data);
            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Cost_Analysis", paramsList);
        }


        #endregion

    }
}
