﻿using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IProductRepository: IRepository<Product, ProductModel>
    {
        Task<PagingModel<ProductModel>> Paging(string keyword, int page, int howMany, bool isActive, int? processTypeId);
        Task<IDictionary<int, ProductDictionaryModel>> ListAsDictionary(IList<MaterialDictionaryModel> productBOM);
    }
}
