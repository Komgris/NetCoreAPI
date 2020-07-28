using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CIM.Model.Constans;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMasterDataService
    {
        Task<MasterDataModel> GetData();
        Task<MasterDataModel> Refresh(MasterDataType masterdataType);
        Task Clear();
        MasterDataModel Data { get; set; }
    }
}
