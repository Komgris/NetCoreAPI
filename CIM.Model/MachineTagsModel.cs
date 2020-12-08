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

        public MachineTagsModel(int id, string name, string runningStatus=""
            , string IdleStatus = ""
            , string counterOut=""
            , string counterReset=""
            , string OEE =""
            , string Performance =""
            , string Availability =""
            , string Quality =""

            , string ProductSKU = ""
            , string ShopNo = ""
            , string Description = ""
            , string Defect = ""
            , string Target = ""
            , string ProductionRate = "")
        {
            this.Id = id;
            this.Name = name;
            this.RunningStatus = new KepwareTagModel<bool>(runningStatus, false);
            this.IdleStatus = new KepwareTagModel<bool>(IdleStatus, false);
            this.CounterOut = new KepwareTagModel<int>(counterOut, 0);
            this.CounterReset = new KepwareTagModel<bool>(counterReset, false);

            this.OEE = new KepwareTagModel<float>(OEE, 0);
            this.Performance = new KepwareTagModel<float>(Performance, 0);
            this.Availability = new KepwareTagModel<float>(Availability, 0);
            this.Quality = new KepwareTagModel<float>(Quality, 0);

            this.ProductSKU = new KepwareTagModel<string>(ProductSKU, "");
            this.ShopNo = new KepwareTagModel<string>(ShopNo, "");
            this.Description = new KepwareTagModel<string>(Description, "");
            this.Defect = new KepwareTagModel<int>(Defect, 0);
            this.Target = new KepwareTagModel<int>(Target, 0);
            this.ProductionRate = new KepwareTagModel<float>(ProductionRate, 0);

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastChanged { get; set; } = DateTime.Now;
        public KepwareTagModel<bool> RunningStatus { get; set; }
        public KepwareTagModel<bool> IdleStatus { get; set; }
        public KepwareTagModel<int> CounterOut { get; set; }
        public KepwareTagModel<bool> CounterReset { get; set; }

        public KepwareTagModel<float> OEE { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Performance { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Availability { get; set; } = new KepwareTagModel<float>();
        public KepwareTagModel<float> Quality { get; set; } = new KepwareTagModel<float>();

        public KepwareTagModel<string> ProductSKU { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<string> ShopNo { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<string> Description { get; set; } = new KepwareTagModel<string>();
        public KepwareTagModel<int> Target { get; set; } = new KepwareTagModel<int>();
        public KepwareTagModel<int> Defect { get; set; } = new KepwareTagModel<int>();
        public KepwareTagModel<float> ProductionRate { get; set; } = new KepwareTagModel<float>();

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

