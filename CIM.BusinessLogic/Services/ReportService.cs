using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IDirectSqlRepository _directSqlRepository;

        public ReportService(IDirectSqlRepository directSqlRepository) {
            _directSqlRepository = directSqlRepository;
        }

        #region  Cim-Oper Production overview

        public DataTable GetProductionSummary(string planId, int routeId, DateTime? from, DateTime? to) {

            var paramsList = new List<SqlParameter>();
            
            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));
            if (from != null) paramsList.Add(new SqlParameter("@from", from));
            if (to != null) paramsList.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionsummary", paramsList);
        }

        public DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to) {
            
            var paramsList = new List<SqlParameter>();
            
            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));
            if (from != null) paramsList.Add(new SqlParameter("@from", from));
            if (to != null) paramsList.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_machinespeed", paramsList);
        }

        public DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to) {
            
            var paramsList = new List<SqlParameter>();
            
            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));
            if (from != null) paramsList.Add(new SqlParameter("@from", from));
            if (to != null) paramsList.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionevents", paramsList);
        }

        public DataTable GetProductionOperators(string planid, int routeId) {
            var plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionoperators", plist);
        }

        public DataTable GetProductionPlanInfomation(string planId, int routeId) {
            
            var paramsList = new List<SqlParameter>();
            
            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productioninfo", paramsList);
        }

        #endregion

        #region  Cim-Oper Mc-Loss
        public DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? machineId, DateTime? from, DateTime? to) {

            var paramsList = new List<SqlParameter>();

            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));
            if (lossLv != null)paramsList.Add(new SqlParameter("@losslv", lossLv));
            if (machineId != null)paramsList.Add(new SqlParameter("@mcid", machineId));
            if (from != null)paramsList.Add(new SqlParameter("@from", from));
            if (to != null) paramsList.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", paramsList);
        }

        #endregion
        #region  Cim-Oper dashboard
        public DataTable GetProductionDasboard(string planId, int routeId, int machineId) {

            var paramsList = new List<SqlParameter>();

            paramsList.Add(new SqlParameter("@planid", planId));
            paramsList.Add(new SqlParameter("@routeid", routeId));
            paramsList.Add(new SqlParameter("@mcid", machineId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Dashboard", paramsList);
        }

        #endregion
    }
}
