using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class LossRecordService : BaseService, ILossRecordService
    {

        /// <summary>
        ///  Flow: https://files.slack.com/files-pri/TQ8CQV90S-F011ACJ39CJ/messageimage_1586272635453.jpg
        /// </summary>
        /// <param name="activeProcess"> Production plan that are in process</param>
        /// <param name="machibeComponentId"></param>
        /// <param name="machineComponentStatusId"></param>
        /// <returns></returns>
        public async Task Create(ActiveProcessModel activeProcess, int machibeComponentId, int machineComponentStatusId)
        {

            // Create Record_Machine_Component_Status

            //Stop
            // If status is stop and Record_Machine_Component_Loss doesn't have any record of production plan's component
            // -> create record

            //Running
            //If has Record_Machine_Component_Loss and all production plan's component is running
            // -> stamp endtime

        }


        /// <summary>
        /// Flow: https://files.slack.com/files-pri/TQ8CQV90S-F011ACJ39CJ/messageimage_1586272635453.jpg
        /// </summary>
        /// <param name="activeProcess"></param>
        /// <param name="machibeComponentId"></param>
        /// <param name="machineComponentStatusId"></param>
        /// <returns></returns>
        public async Task ManualCreate(ActiveProcessModel activeProcess, int machibeComponentId, int machineComponentStatusId)
        {

            // Create Record_Machine_Component_Status

            // If status is stop and Record_Machine_Component_Loss doesn't have any record of production plan's component
            // -> Create
            // If has record and is not the same component
            // -> Stamp end time of current and create new Record_Machine_Component_Loss

        }
    }
}
