using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using CIM.DAL.Utility;

namespace CIM.DAL.Implements
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public MaterialRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration ) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<MaterialModel>> List(string keyword, int page, int howMany, bool isActive, string imagePath)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howMany},
                                            { "@page", page},
                                            { "@is_active", isActive},
                                            { "@imagepath", imagePath},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMaterial", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<MaterialModel>(), totalCount, page, howMany);
            });
        }

        public async Task<IList<MaterialDictionaryModel>> ListProductBOM()
        {
            return await ExecStoreProcedure<MaterialDictionaryModel>("sp_ListProductBOM", new Dictionary<string, object>());

        }
    }
}
