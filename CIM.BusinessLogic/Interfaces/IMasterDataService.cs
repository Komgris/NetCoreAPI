using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMasterDataService
    {
        Task<MasterDataModel> GetData();
        Task<MasterDataModel> Refresh();
        Task Clear();

        MasterDataModel Data { get; set; }

    }
}
