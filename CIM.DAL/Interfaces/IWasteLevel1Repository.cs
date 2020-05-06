using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IWasteLevel1Repository : IRepository<WasteLevel1>
    {
        Task<IList<WasteDictionaryModel>> ListAsDictionary();
    }
}
