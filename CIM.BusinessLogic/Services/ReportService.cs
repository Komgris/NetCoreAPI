using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services {
    public class ReportService : BaseService, IReportService {

        private IConfiguration _configuration;
        private IDirectSqlRepository repo;
        Dictionary<string, string> InMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:CIMDatabase", "Server=103.70.6.198;Initial Catalog=cim_db;Persist Security Info=False;User ID=cim;Password=4dev@psec;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"},
        };

        public ReportService() {
            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(InMemorySettings)
            .Build();
            repo = new DirectSqlRepository(_configuration);
        }

        public string GetMachineSpeed(int planid, int routeid, DateTime from, DateTime to) {
            throw new NotImplementedException();
        }

        public string GetProductionEvents(int planid, int routeid, DateTime from, DateTime to) {
            throw new NotImplementedException();
        }

        public string GetProductionOperators(int planid, int routeid) {
            throw new NotImplementedException();
        }

        public string GetProductionPlanInfomation(int planid, int routeid) {
            throw new NotImplementedException();
        }

        public string GetProductionSummary(int planid, int routeid, DateTime from, DateTime to) {
            var result = repo.ExecuteReader("select * from sites ", null);
            return result;
        }

    }
}
