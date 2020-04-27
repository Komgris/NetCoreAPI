﻿using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IDirectSqlRepository _directSqlRepository;

        public ReportService(IDirectSqlRepository directSqlRepository) {
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
        public Dictionary<string,int> GetActiveProductionPlanOutput()
        {
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

        public DataTable GetWasteHistory(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new Dictionary<string, object>() {
                {"@planid", planId },
                {"@routeid", routeId },
                {"@from", from },
                {"@to", to }
            };

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_waste_history", paramsList);
        }

        #endregion
    }
}
