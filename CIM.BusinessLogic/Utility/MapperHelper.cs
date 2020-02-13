using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Utility
{
    public static class MapperHelper
    {

        public static TDestination AsModel<TSource, TDestination>(TSource item, TDestination model, string[] customException = null)
        {
            var exception = new string[] {
                "CreateDate", "CreateBy", "IsDelete"
            };
            if (customException != null)
                exception = exception.Union(customException).ToArray();

            foreach (var model_property in model.GetType().GetProperties())
            {
                var name = model_property.Name;
                if (!exception.Any(x => x == name))
                {
                    var prop = item.GetType().GetProperty(name);
                    if (prop != null)
                    {
                        var value = prop.GetValue(item, null);
                        model_property.SetValue(model, value, null);
                    }
                }

            }
            return model;
        }

    }
}
