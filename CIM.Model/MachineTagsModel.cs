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

        public MachineTagsModel(int id, string name, string runningStatus="", string Speed= "", string counterOut="", string counterReset="", string OEE ="", string Performance ="", string Availability ="", string Quality ="",string ProductionPlanId= "",string ProductCode="",string ShopNo ="", string Sequence ="", string Target ="")
        {
            this.Id = id;
            this.Name = name;
            this.RunningStatus = new KepwareTagModel<bool>(runningStatus, false);
            this.Speed = new KepwareTagModel<int>(Speed, 0);
            this.CounterOut = new KepwareTagModel<int>(counterOut, 0);
            this.CounterReset = new KepwareTagModel<bool>(counterReset, false);
            this.Oee = new KepwareTagModel<float>(OEE, 0);
            this.Performance = new KepwareTagModel<float>(Performance, 0);
            this.Availability = new KepwareTagModel<float>(Availability, 0);
            this.Quality = new KepwareTagModel<float>(Quality, 0);
            this.ProductionPlanId = new KepwareTagModel<string>(ProductionPlanId, "");
            this.ProductCode = new KepwareTagModel<string>(ProductCode, "");
            this.ShopNo = new KepwareTagModel<string>(ShopNo, "");
            this.Sequence = new KepwareTagModel<int>(Sequence, 0);
            this.Target = new KepwareTagModel<int>(Target, 0);

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public KepwareTagModel<bool> RunningStatus { get; set; }
        public KepwareTagModel<int> Speed { get; set; }
        public KepwareTagModel<int> CounterOut { get; set; }
        public KepwareTagModel<bool> CounterReset { get; set; }
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public KepwareTagModel<float> Oee { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Performance { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Availability { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Quality { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<string> ProductionPlanId { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<string> ProductCode { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<string> ShopNo { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<int> Sequence { get; set; } = new KepwareTagModel<int>();
        public KepwareTagModel<int> Target { get; set; } = new KepwareTagModel<int>();

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

