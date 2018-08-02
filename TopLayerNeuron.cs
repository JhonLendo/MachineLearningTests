using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class OutputNeuron : Neuron
    {

        public int responceid;
        private String name;

        public String getName()
        {
            return name;
        }

        public void setName(String n)
        {
            name = n;
        }

        public Boolean hasName()
        {
            return name != null;
        }

        public OutputNeuron(AINetwork ai, int responceid) : base (ai, ai.maxNeuralLayers - 1)
        {
            this.responceid = responceid;
        }

        public OutputNeuron(AINetwork ai, int responceid, int layer) : base(ai, layer)
        {
            this.responceid = responceid;
        }

    public Neuron generateNeuron(AINetwork ai)
        {
            OutputNeuron n = new OutputNeuron(ai, responceid);
            return n;
        }

    public Neuron clone(AINetwork ai)
        {
            OutputNeuron clone = (OutputNeuron)generateNeuron(ai);
            clone.responceid = responceid;
            return clone;
        }

    public Boolean isTriggered()
        {
            if (base.allowNegativeVals())
                return getTriggeredStength() > 0.0;
            else
                return getTriggeredStength() > 0.5;
        }

        public OutputNeuron(Dictionary<String, Object> map) : base (map)
        {
            this.name = (String)map["n"];
            this.responceid = (int)map["rid"];
        }

    public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> m = base.serialize();
            m["n"] = this.name;
            m["rid"] = this.responceid;
            return m;
        }

    }
}
