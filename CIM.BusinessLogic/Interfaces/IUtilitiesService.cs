using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IUtilitiesService : IBaseService
    {
        Task<string> UploadImage(IFormFile image, string pathName, string savePath);
    }
}
