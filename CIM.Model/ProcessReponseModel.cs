using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class ProcessReponseModel<T>
        where T : new()
    {
        public T Data { get; set; } = new T();
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "Success";

    }
}
