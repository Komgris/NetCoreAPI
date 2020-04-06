using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class LookupItem<K,V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
    }
}
