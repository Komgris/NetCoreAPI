using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class TriggerQueueService : BaseService, ITriggerQueueService {

        private IDirectSqlRepository _directSqlRepository;

        public TriggerQueueService(
            IDirectSqlRepository directSqlRepository
            )
        {
            _directSqlRepository = directSqlRepository;
        }

        public void TriggerQueueing(Constans.TriggerType trigertype, string par)
        {
            var paramsList = new Dictionary<string, object>() 
            { 
                { "@trig", trigertype },
                { "@pars", par }
            };
            _directSqlRepository.ExecuteSPNonQuery("a_sys_call_api_trigger_queueing", paramsList);
        }
    }
}
