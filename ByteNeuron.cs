using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml
{
    public class ByteNeuron : OutputNeuron
    {

        public ByteNeuron(AINetwork ai, int layer) : base(ai, layer)
        { }

        public double getTriggeredStength()
        {
            if (droppedOut())
                return 0;
            return 1;
        }

        public Neuron generateNeuron(AINetwork ai)
        {
            return ByteNeuron.generateNeuronStatically(ai, this.layer);
        }

        public static ByteNeuron generateNeuronStatically(AINetwork ai, int layer)
        {
            return new ByteNeuron(ai, layer);
        }

        public double forceSuddenStenghtUpdt()
        {
            this.tickUpdated = getAI().getCurrentTick();
            return lastResult = 1;
        }
        public ByteNeuron(Dictionary<String, Object> map) : base(map)
        {
            // Pass it to Master Class
        }

        public Boolean isTriggered()
        {
            if (droppedOut())
                return false;
            if (!useThreshold())
                return true;
            return getThreshold() < 0.5;
        }

    }
}
