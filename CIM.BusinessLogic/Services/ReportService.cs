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

            var parlist = new List<SqlParameter>();
            
            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));
            if (from != null) parlist.Add(new SqlParameter("@from", from));
            if (to != null) parlist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionsummary", parlist);
        }

        public DataTable GetMachineSpeed(string planId, int routeId, DateTime? from, DateTime? to) {
            
            var parlist = new List<SqlParameter>();
            
            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));
            if (from != null) parlist.Add(new SqlParameter("@from", from));
            if (to != null) parlist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_machinespeed", parlist);
        }

        public DataTable GetProductionEvents(string planId, int routeId, DateTime? from, DateTime? to) {
            
            var parlist = new List<SqlParameter>();
            
            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));
            if (from != null) parlist.Add(new SqlParameter("@from", from));
            if (to != null) parlist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionevents", parlist);
        }

        public DataTable GetProductionOperators(string planid, int routeId) {
            var plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionoperators", plist);
        }

        public DataTable GetProductionPlanInfomation(string planId, int routeId) {
            
            var parlist = new List<SqlParameter>();
            
            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productioninfo", parlist);
        }

        #endregion

        #region  Cim-Oper Mc-Loss
        public DataTable GetProductionWCMLoss(string planId, int routeId, int? lossLv, int? mcId, DateTime? from, DateTime? to) {

            var parlist = new List<SqlParameter>();

            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));
            if (lossLv != null)parlist.Add(new SqlParameter("@losslv", lossLv));
            if (mcId != null)parlist.Add(new SqlParameter("@mcid", mcId));
            if (from != null)parlist.Add(new SqlParameter("@from", from));
            if (to != null) parlist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_WCMLosses", parlist);
        }

        #endregion
        #region  Cim-Oper dashboard
        public DataTable GetProductionDasboard(string planId, int routeId, int mcId) {

            var parlist = new List<SqlParameter>();

            parlist.Add(new SqlParameter("@planid", planId));
            parlist.Add(new SqlParameter("@routeid", routeId));
            parlist.Add(new SqlParameter("@mcid", mcId));

            return _directSqlRepository.ExecuteSPWithQuery("sp_Report_Production_Dashboard", parlist);
        }

        #endregion
    }
}
