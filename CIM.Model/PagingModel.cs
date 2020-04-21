using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class PagingModel<T>
    {
        public int Page { get; set; }

        public int HowMany { get; set; }

        public int NextPage { get; set; }

        public int PreviousPage { get; set; }

        public List<T> Data { get; set; } = new List<T>();
        public int Total { get; set; }
    }
}
