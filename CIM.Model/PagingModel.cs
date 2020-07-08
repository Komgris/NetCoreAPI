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

        public object DataObject { get; set; }

        public int Total { get; set; }
        public int LastPage { get; set; }
        public bool ShowNext { get; set; }
        public bool ShowPrevious { get; set; }
    }
}
