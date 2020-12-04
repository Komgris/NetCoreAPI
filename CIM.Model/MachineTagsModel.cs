using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class MachineTagsModel
    {
        public MachineTagsModel()
        {

        }

        public MachineTagsModel(int id, string name, string runningStatus="", string Speed= "", string counterOut="", string counterReset="")
        {
            this.Id = id;
            this.Name = name;
            this.RunningStatus = new KepwareTagModel<bool>(runningStatus, false);
            this.Speed = new KepwareTagModel<int>(Speed, 0);
            this.CounterOut = new KepwareTagModel<int>(counterOut, 0);
            this.CounterReset = new KepwareTagModel<bool>(counterReset, false);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public KepwareTagModel<bool> RunningStatus { get; set; }
        public KepwareTagModel<int> Speed { get; set; }
        public KepwareTagModel<int> CounterOut { get; set; }
        public KepwareTagModel<bool> CounterReset { get; set; }
    }

    public class KepwareTagModel<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }

        public KepwareTagModel()
        {
        }

        public KepwareTagModel (string tn, T tv)
        {
            this.Name = tn;
            this.Value = tv;
        }

        public void Init(string tn, T tv)
        {
            this.Name = tn;
            this.Value = tv;
        }
    }
}
