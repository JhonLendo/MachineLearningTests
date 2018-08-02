using MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class AINetwork
    {
        public static int idTotal = 0;
        private int id = 0;

        public NeuralEntity entity
        {
            get; set;
        }


        private Dictionary<int, Neuron> allNeurons = new Dictionary<int, Neuron>();
        private List<NeuralLayer> NeuralLayers = new List<NeuralLayer>();

        private int currentNeuronId = 0;
        private int currentTick = 1;
        public int maxNeuralLayers = -1;

        public AINetwork(NeuralEntity entity) : this(entity, 3) { }

        public AINetwork(NeuralEntity entity, int NeuralLayers_amount)
        {
            id = idTotal;
            idTotal++;
            entity = entity;
            entity.ai = this;

            this.maxNeuralLayers = NeuralLayers_amount;

            for (int i = 0; i < maxNeuralLayers; i++)
            {
                NeuralLayers.Add(new NeuralLayer(i));
            }
        }

        public AINetwork(NeuralEntity e, int id, Boolean addToTotal) : this(e, id, addToTotal, 3) { }

        public AINetwork(NeuralEntity e, int id, Boolean addToTotal, int NeuralLayers_amount)
        {
            this.id = id;
            if (addToTotal)
                idTotal++;
            entity = e;
            e.ai = this;

            this.maxNeuralLayers = NeuralLayers_amount;

            for (int i = 0; i < maxNeuralLayers; i++)
            {
                NeuralLayers.Add(new NeuralLayer(i));
            }
        }
        public void setNeuronsEachRow(int row, int amount)
        {
            this.getNeuralLayer(row).setNeuronsEachRow(amount);
        }

        public int neuronsEachRow(int row)
        {
            return this.getNeuralLayer(row).getNeuronsEachRow();
        }

        public Neuron neuronsByID(int n)
        {
            return allNeurons[n];
        }

        Random rnd = new Random();

        

        public void KillDropOutTask()
        {
            for (int i = 1; i < maxNeuralLayers - 1; i++)
            {
                foreach (Neuron n in getNeuronsInNeuralLayer(i))
                {
                    if (n.droppedOut())
                        n.setTemperaryDropout(false);
                }
            }
        }


        public void ForceUpdateInLayer(int NeuralLayer)
        {
            for (int i = NeuralLayer; i < maxNeuralLayers; i++)
            {
                foreach (Neuron n in getNeuronsInNeuralLayer(i))
                {
                    n.forceTriggerStengthUpdate();
                }
            }
        }

        public List<Neuron> NeuronsByID(List<int> set)
        {
            List<Neuron> neurons = new List<Neuron>();
            foreach (int n in set)
            {
                neurons.Add(allNeurons[n]);
            }
            return neurons;
        }

        public static AINetwork createAIInstance(NeuralEntity entity, int numberOfMotorNeurons, String[] names)
        {
            AINetwork ai = new AINetwork(entity);
            for (int i = 0; i < numberOfMotorNeurons; i++)
            {
                OutputNeuron omn = new OutputNeuron(ai, i);
                if (i < names.Length)
                    omn.setName(names[i]);
            }
            return ai;
        }

        public static AINetwork createAIInstance(NeuralEntity entity, int numberOfMotorNeurons, int NeuralLayers, params String[] names)
        {
            AINetwork ai = new AINetwork(entity, NeuralLayers);
            for (int i = 0; i < numberOfMotorNeurons; i++)
            {
                OutputNeuron omn = new OutputNeuron(ai, i);
                if (i < names.Length)
                    omn.setName(names[i]);
            }
            return ai;
        }

        public void dropOutStartChance(double chanceForDropout)
        {
            for (int i = 1; i < maxNeuralLayers - 1; i++)
            {
                foreach (Neuron n in getNeuronsInNeuralLayer(i))
                {
                    if (rnd.NextDouble() < chanceForDropout)
                        n.setTemperaryDropout(true);
                }
            }
        }

        public AINetwork Clone(NeuralEntity ent)
        {
            AINetwork ai = new AINetwork(ent, maxNeuralLayers);
            for (int i = 0; i < allNeurons.Count; i++)
            {
                allNeurons[i].generateNeuron(ai);
            }
            return ai;
        }

        public void tick()
        {
            currentTick++;
        }

        public List<NeuralLayer> getNeuralLayers()
        {
            return NeuralLayers;
        }

        public NeuralLayer getNeuralLayer(int NeuralLayer)
        {
            return NeuralLayers[NeuralLayer];
        }

        public List<Neuron> getInputNeurons()
        {
            return NeuralLayers[0].neurons;
        }

        public List<Neuron> getAllNeurons()
        {
            return new List<Neuron>(allNeurons.Values.ToList());
        }

        public void addNeuron(Neuron n)
        {
            this.allNeurons[n.getID()] = n;
        }

        public int getCurrentTick()
        {
            return currentTick;
        }

        public void setCurrentTick(int i)
        {
            currentTick = i;
        }

        public int generateNeuronId()
        {
            return currentNeuronId++;
        }

        public int getNewestId()
        {
            return currentNeuronId;
        }

        public Boolean[] TryToThink()
        {
            tick();
            for (int i = 0; i < NeuralLayers.Count; i++)
            {
                foreach (Neuron n in getNeuronsInNeuralLayer(i))
                {
                    n.forceTriggerStengthUpdate();
                }
            }
            Boolean[] points = new Boolean[getOutputNeurons().Count];
            foreach (Neuron n in getOutputNeurons())
            {
                if (n.isTriggered())
                {
                    if (n is OutputNeuron)
                    {
                        points[((OutputNeuron)n).responceid] = true;
                    }
                }
            }
            return points;
        }

        public List<Neuron> getNeuronsInNeuralLayer(int NeuralLayer)
        {
            return NeuralLayers[NeuralLayer].neurons;
        }

        public List<Neuron> getOutputNeurons()
        {
            return getNeuronsInNeuralLayer(NeuralLayers.Count - 1);
        }

        public AINetwork(Dictionary<String, Object> map)
        {
            this.NeuralLayers = (List<NeuralLayer>)map["l"];
            foreach (NeuralLayer l in NeuralLayers)
            {
                foreach (Neuron n in l.neurons)
                {
                    if (n != null)
                    {
                        n.setAI(this);
                        addNeuron(n);
                    }
                }
            }
            this.maxNeuralLayers = (int)map["ml"];
            this.id = (int)map["id"];
        }

        public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> m = new Dictionary<String, Object>();
            m["l"] = this.NeuralLayers;
            m["ml"] = this.maxNeuralLayers;
            m["id"] = this.id;
            return m;
        }
    }
}
