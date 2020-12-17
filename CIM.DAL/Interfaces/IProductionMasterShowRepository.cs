using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;

namespace CIM.DAL.Interfaces
{
    public interface IProductionMasterShowRepository : IRepository< ProductionPlan, ProductionMasterShowModel>
    {
    }
}