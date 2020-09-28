using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ITriggerQueueService : IBaseService
    {
        void TriggerQueueing(TriggerType trigertype, int par);
    }
}
