using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineTagsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MachineTagsSup<bool> RunningStatus { get; set; } = new MachineTagsSup<bool>();
        public MachineTagsSup<int> CounterIn { get; set; } = new MachineTagsSup<int>();
        public MachineTagsSup<int> CounterOut { get; set; } = new MachineTagsSup<int>();
        public MachineTagsSup<bool> CounterReset { get; set; } = new MachineTagsSup<bool>();
    }

    public class MachineTagsSup<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public MachineTagsSup (string tn, T tv)
        {
            this.Name = tn;
            this.Value = tv;
        }

        public MachineTagsSup()
        {
        }

        public void Init(string tn, T tv)
        {
            this.Name = tn;
            this.Value = tv;
        }
    }
}
