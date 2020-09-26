using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductMaterialRepository : IRepository<ProductMaterial, object>
    {
        Task<List<ProductMaterialModel>> ListMaterialByProduct(int productId);
    }
}
