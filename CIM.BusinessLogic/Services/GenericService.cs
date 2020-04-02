using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class GenericService : IGenericService
    {
        //public async Task<ProcessReponseModel<T>> Run<T>(Func<T> p, T model) 
        //    where T : new()
        //{
        //    var output = new ProcessReponseModel<T>();

        //    //var log = _activityLogService.LogInbound(HttpContext.Current.Request, model);

        //    try
        //    {
        //        var result = (T)p.Invoke();
        //        output.Data = result;
        //        output.IsSuccess = true;
        //        var now = DateTime.Now;
        //        //_activityLogService.LogOutbound(result, log);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ProcessReponseModel<T> { IsSuccess = false, Message = ex.Message };
        //        //ErrorService.Log(ex);
        //        //output.Message = MessageDictionary.ProcessError + Environment.NewLine + ex.ToString();
        //    }

        //    return output;
        //}

        //public async Task<ProcessReponseModel<T>> Void<T>(Func<T> p) where T : new()
        //{
        //    var output = new ProcessReponseModel<T>();
        //    try
        //    {
        //        p.Invoke();
        //        output.IsSuccess = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        return new ProcessReponseModel<T> { IsSuccess = false, Message = ex.Message };
        //    }
        //    return output;

        //}
    }
}
