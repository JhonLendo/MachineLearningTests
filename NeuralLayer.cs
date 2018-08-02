using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class NeuralLayer
    {
        // How many Neurons should this Layer handle? It is yet to be defined by the master class
        // The value is -2 by default because we want it undefined for now
        // This can't be unsigned of course, We want it negative in case of undefinition
        private int neuronsNumberEachRow = 2;
        // The value of the Layer
        private int value;
        // All Neurons handled by this layer
        public List<Neuron> neurons = new List<Neuron>();

        public NeuralLayer(int value)
        {
            this.value = value;
        }

        public int getLayerValue()
        {
            return this.value;
        }
        
        public int getNeuronsEachRow()
        {
            return this.neuronsNumberEachRow;
        }

        public void setNeuronsEachRow(int neurons)
        {
            this.neuronsNumberEachRow = neurons;
        }

        public NeuralLayer(Dictionary<String, Object> map)
        {
            this.neurons = (List<Neuron>)map["n"];
            this.value = (int)map["l"];
            this.neuronsNumberEachRow = (int)map["npr"];
        }

        public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> m = new Dictionary<String, Object>();
            m["n"] = this.neurons;
            m["l"] = this.value;
            m["npr"] = this.neuronsNumberEachRow;
            return m;
        }

    }
}
