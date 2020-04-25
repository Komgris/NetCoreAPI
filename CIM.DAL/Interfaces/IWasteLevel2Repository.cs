using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IWasteLevel2Repository : IRepository<WasteLevel2>
    {
        Task<IList<WasteDictionaryModel>> ListAsDictionary();
    }
}