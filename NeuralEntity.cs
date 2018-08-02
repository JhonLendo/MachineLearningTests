using MachineLearning.ml;
using MachineLearning.ml.utilkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class NeuralEntity
    {

        public AINetwork ai
        {
            get;
            set;
        }

        public void backPropNeuronConnections(Neuron newNeuron, Boolean randomize)
        {
            if (newNeuron.getLayer() > 0)
            {
                foreach (Neuron n in getAI().getNeuronsInNeuralLayer(newNeuron.getLayer() - 1))
                {
                    if (n.allowNegativeVals())
                        n.setStrengthForNeuron(newNeuron, randomize ? (rnd.NextDouble() * 2) - 1 : 0.1);
                    else
                        n.setStrengthForNeuron(newNeuron, randomize ? rnd.NextDouble() : 0.1);
                }
            }
        }

        protected Boolean doLearning = false;
        protected MasterNeuralManager manager;

        protected Sharp sharp = new Sharp(500);

        public Sharp getSharpness()
        {
            return this.sharp;
        }

        public NeuralEntity() { }

        public NeuralEntity(Boolean createAI)
        {
            if (createAI)
            this.ai = new AINetwork(this);
        }

        public NeuralEntity(Boolean createAI, int sharpness)
        {
            if (createAI)
            this.ai = new AINetwork(this);
            this.sharp = new Sharp(sharpness);
        }

        public void connectNeurons()
        {
            foreach (Neuron n in ai.getAllNeurons())
            {
                connectNeuron(n);
            }
        }

        // When We connect the Neurons, We're able to transfer data between them
        public void connectNeuron(Neuron n)
        {
            if (ai.maxNeuralLayers > n.layer + 1)
            {
                n.setWeight((rnd.NextDouble() * 2) - 1);
                n.setWeight(0.1);
                foreach (Neuron output in ai.getNeuronsInNeuralLayer(n.layer + 1))
                {
                    if (output is ByteNeuron)
					continue;
                n.setStrengthForNeuron(output, 0);
            }
        }
    }

        Random rnd = new Random();

    public void randomizeNeurons()
    {
        foreach (Neuron n in ai.getAllNeurons())
        {
            shuffle(n);
        }
    }

    public void shuffle(Neuron n)
    {
        if (ai.maxNeuralLayers > n.layer + 1)
        {
            if (n.allowNegativeVals())
            {
                n.setWeight((rnd.NextDouble() * 2) - 1);
                n.setThreshold((rnd.NextDouble() * 2) - 1);
            }
            else
            {
                n.setWeight((rnd.NextDouble()));
                n.setThreshold((rnd.NextDouble()));
            }
            foreach (Neuron output in ai.getNeuronsInNeuralLayer(n.layer + 1))
            {
                if (n.allowNegativeVals())
                    n.setStrengthForNeuron(output, (rnd.NextDouble() * 2) - 1);
                else
                    n.setStrengthForNeuron(output, (rnd.NextDouble()));
            }
        }
    }

    public Boolean[] tickAndThink()
    {
        return ai.TryToThink();
    }

    public NeuralEntity clone()
    {
        return null;
    }

    public MasterNeuralManager getControler()
    {
        return this.manager;
    }

    public Boolean shouldLearn()
    {
        return this.doLearning;
    }

    public void setShouldLearn(Boolean b)
    {
        doLearning = b;
    }

    public void setNeuronsPerRow(int row, int amount)
    {
        getAI().setNeuronsEachRow(row, amount);
    }

    public int getNeuronsPerRow(int row)
    {
        return getAI().neuronsEachRow(row);
    }

    public AINetwork getAI()
    {
        return ai;
    }

    public NeuralEntity(Dictionary<String, Object> map)
    {
        this.ai = (AINetwork)map["ai"];
        this.ai.entity = this;
        if (map.ContainsKey("c"))
        {
            this.manager = (MasterNeuralManager)map["c"];
        }
        else if (this is MasterNeuralManager) {
            this.manager = (MasterNeuralManager)this;
        }
    }

    public Dictionary<String, Object> serialize()
    {
        Dictionary<String, Object> m = new Dictionary<String, Object>();
        if (this.manager != this)
            m["c"] = manager;
        m["ai"] = ai;
        return m;
    }

}
}
