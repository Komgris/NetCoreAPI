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

        public DataTable GetProductionSummary(string planid, int routeid, DateTime? from, DateTime? to) {

            var plist = new List<SqlParameter>();
            
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeid));
            if (from != null) plist.Add(new SqlParameter("@from", from));
            if (to != null) plist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionsummary", plist);
        }

        public DataTable GetMachineSpeed(string planid, int routeid, DateTime? from, DateTime? to) {
            
            var plist = new List<SqlParameter>();
            
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeid));
            if (from != null) plist.Add(new SqlParameter("@from", from));
            if (to != null) plist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_machinespeed", plist);
        }

        public DataTable GetProductionEvents(string planid, int routeid, DateTime? from, DateTime? to) {
            
            var plist = new List<SqlParameter>();
            
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeid));
            if (from != null) plist.Add(new SqlParameter("@from", from));
            if (to != null) plist.Add(new SqlParameter("@to", to));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionevents", plist);
        }

        public DataTable GetProductionOperators(string planid, int routeid) {
            var plist = new List<SqlParameter>();
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeid));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productionoperators", plist);
        }

        public DataTable GetProductionPlanInfomation(string planid, int routeid) {
            
            var plist = new List<SqlParameter>();
            
            plist.Add(new SqlParameter("@planid", planid));
            plist.Add(new SqlParameter("@routeid", routeid));

            return _directSqlRepository.ExecuteSPWithQuery("sp_report_productioninfo", plist);
        }


    }
}
