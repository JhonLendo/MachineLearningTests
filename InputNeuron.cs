using MachineLearning.ml.utilkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml
{
    public class InputNeuron : Neuron
    {
        public int xlink;
        public int ylink;
        public int zlink;

        public Motor s;

        public bool isTriggeredLast = false;

        public InputNeuron() { }

        public InputNeuron(AINetwork ai, int xlink, int ylink, int zlink, Motor s) : base(ai, 0)
        {
            init(ai, xlink, ylink, zlink, s);
        }

        public InputNeuron(AINetwork ai, int xlink, int ylink, Motor s)
        {
            init(ai, xlink, ylink, 0, s);
        }

        public InputNeuron(AINetwork ai) : base(ai, 0)
        {
        }

        private void init(AINetwork ai, int xlink, int ylink, int zlink, Motor s)
        {
            this.xlink = xlink;
            this.ylink = ylink;
            this.zlink = zlink;
            this.s = s;
        }

        public void setIsTriggeredLast(bool b)
        {
            this.isTriggeredLast = b;
        }

        public bool getIsTriggeredLast()
        {
            return this.isTriggeredLast;
        }

    public Neuron clone(AINetwork ai)
        {
            InputNeuron n = (InputNeuron)base.clone(ai);
            n.xlink = xlink;
            n.ylink = ylink;
            return null;
        }

        public InputNeuron generateNeuron(AINetwork ai, Motor sensory)
        {
            return null;
        }

    public double forceTriggerStengthUpdate()
        {
            this.tickUpdated = getAI().getCurrentTick();

            double i = 0;

            if (s is Motor)
			i = lastResult = ((Motor)s).getPowerFor(xlink, ylink);
            if (s is Motor3D)
			i = lastResult = ((Motor3D)s).getPowerFor(xlink, ylink, zlink);
            isTriggeredLast = i > 0;
            return i;
        }

    public double getTriggeredStength()
        {
            if (this.tickUpdated == this.getAI().getCurrentTick())
                return lastResult;
            if (s is Motor)
			return lastResult = ((Motor)s).getPowerFor(xlink, ylink);
            if (s is Motor3D)
			return lastResult = ((Motor3D)s).getPowerFor(xlink, ylink, zlink);
            return 0;
        }

        public Motor getSenses()
        {
            return s;
        }

        public void stSenses(Motor s)
        {
            this.s = s;
        }

        public InputNeuron(Dictionary<String, Object> map) : base(map)
        {
            this.xlink = map.ContainsKey("xl") ? (int)map["xl"] : 0;
            this.ylink = map.ContainsKey("yl") ? (int)map["yl"] : 0;
            this.zlink = map.ContainsKey("zl") ? (int)map["zl"] : 0;
            this.s = (Motor)map["s"];
        }

    public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> m = base.serialize();// new HashMap<String, Object>();
            if (this.xlink != 0)
                m["xl"] = this.xlink;
            if (this.ylink != 0)
                m["yl"] = this.ylink;
            if (this.zlink != 0)
                m["zl"] = this.zlink;
            m["s"] = this.s;
            return m;
        }
    }
}
