using MachineLearning.ml.utilkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public class Neuron
    {

        private AINetwork ai;
        private int id;
        public int layer;

        private Dictionary<int, double> outputStength = new Dictionary<int, double>();
        private double weight = 0.5;
        private double threshold = -0.5;

        private double inheritBias = 0;

        private Boolean trainingThisNeuron = false;

        private Boolean haveThreshold = true;

        private Boolean allowNegativeValues = true;

        private Boolean dropout = false;

        public Neuron setAllowNegativeValues(Boolean b)
        {
            this.allowNegativeValues = b;
            return this;
        }

        public Boolean allowNegativeVals()
        {
            return allowNegativeValues;
        }

        public void setTemperaryDropout(Boolean b)
        {
            this.dropout = b;
        }

        public Boolean droppedOut()
        {
            return dropout;
        }

        public Neuron setUseThreshold(Boolean b)
        {
            this.haveThreshold = b;
            return this;
        }

        public Boolean useThreshold()
        {
            return haveThreshold;
        }

        protected int tickUpdated = -1;
        protected double lastResult = -1;

        public Neuron(AINetwork ai, int layer)
        {
            this.ai = ai;
            this.layer = layer;
            id = ai.generateNeuronId();
            ai.addNeuron(this);
            ai.getNeuronsInNeuralLayer(layer).Add(this);
        }

        public Boolean isTriggered()
        {
            if (dropout)
                return false;
            if (!haveThreshold)
                return true;
            return getThreshold() < getTriggeredStength();
        }

        public double getOutputForNeuron(Neuron n)
        {
            if (!outputStength.ContainsKey(n.id))
                return 0;
            return weight * outputStength[n.id] * getTriggeredStength();
        }

        public double getTriggeredStength()
        {
            if (tickUpdated == ai.getCurrentTick())
            {
                return lastResult;
            }
            if (dropout)
                return 0;
            return forceTriggerStengthUpdate();
        }

        public int getLayer()
        {
            return layer;
        }

        public void forceLastResultChange(double value)
        {
            this.tickUpdated = ai.getCurrentTick();
            this.lastResult = value;
        }

        public double getLastResult()
        {
            return lastResult;
        }

        public Boolean hasOutputTo(Neuron n)
        {
            return outputStength.ContainsKey(n.getID());
        }

        public double forceTriggerStengthUpdate()
        {
            double signal = 0;
            foreach (Neuron i in ai.getNeuronsInNeuralLayer(layer - 1))
                if (i.isTriggered())
                    signal += (i.getOutputForNeuron(this));
            tickUpdated = ai.getCurrentTick();
            return lastResult = MathUtil.sigmoidNumberPosAndNeg(signal + inheritBias);
        }

        public void forceTriggerUpdateTree()
        {
            this.forceTriggerStengthUpdate();
            for (int layers = this.layer + 1; layers < this.ai.maxNeuralLayers; layers++)
                foreach (Neuron n in this.ai.getNeuronsInNeuralLayer(layers))
                    n.forceTriggerStengthUpdate();
        }

        public Neuron clone(AINetwork ai)
        {
            return generateNeuron(ai);
        }

        public Neuron generateNeuron(AINetwork ai)
        {
            return Neuron.generateNeuronStatically(ai, this.layer);
        }

        public static Neuron generateNeuronStatically(AINetwork ai, int layer)
        {
            Neuron link = new Neuron(ai, layer);
            return link;
        }

        public Boolean isTraining()
        {
            return trainingThisNeuron;
        }

        public void setIsTraining(Boolean b)
        {
            this.trainingThisNeuron = b;
        }

        public double getWeight()
        {
            return weight;
        }

        public void setWeight(double w)
        {
            weight = w;
        }

        public double getThreshold()
        {
            return threshold;
        }

        public double getBias()
        {
            return inheritBias;
        }

        public void setBias(double bias)
        {
            this.inheritBias = bias;
        }

        public void setThreshold(double t)
        {
            threshold = t;
        }

        public List<int> getStrengthIDs()
        {
            return outputStength.Keys.ToList();
        }

        public double getStrengthForNeuron(Neuron n)
        {
            return outputStength[n.id];
        }

        public double getStrengthForNeuron(int id)
        {
            return outputStength[id];
        }

        public void setStrengthForNeuron(Neuron n, double v)
        {
            outputStength[n.id] = v;
        }

        public void setStrengthForNeuron(int n, double v)
        {
            outputStength[n] = v;
        }

        public AINetwork getAI()
        {
            return ai;
        }

        public int getID()
        {
            return id;
        }

        public void setAI(AINetwork ai)
        {
            this.ai = ai;
        }

        public Neuron() { }

        public Neuron(Dictionary<String, Object> map)
        {
            this.weight = map.ContainsKey("w") ? (double)map["w"] : 0;
            this.threshold = map.ContainsKey("t") ? (double)map["t"] : -0.5;
            this.inheritBias = map.ContainsKey("bias") ? (double)map["bias"] : 0.0;

            this.setUseThreshold(map.ContainsKey("uth"));
            this.setAllowNegativeValues((map.ContainsKey("anv") && (map["anv"].Equals(1))));
            if (map.ContainsKey("osC"))
            {
                this.outputStength = new Dictionary<int, Double>();

                Dictionary<int, List<int>> storedValues = (Dictionary<int, List<int>>)map["osC"];
                foreach (KeyValuePair<int, List<int>> keyset in storedValues)
                {
                    foreach (int i in keyset.Value)
                    {
                        outputStength[i] = ((double)keyset.Key / 10000);
                    }
                }

            }
            else if (map.ContainsKey("osB"))
            {
                this.outputStength = new Dictionary<int, Double>();

                Dictionary<int, List<String>> storedValues = (Dictionary<int, List<String>>)map["osB"];
                foreach (KeyValuePair<int, List<String>> e in storedValues)
                {
                    foreach (String parse in e.Value)
                    {
                        if (parse.Contains(","))
                        {
                            int startingVal = Convert.ToInt32(parse.Split(',')[0]);
                            int amountInSeries = Convert.ToInt32(parse.Split(',')[1]);
                            for (int i = 0; i < amountInSeries; i++)
                                outputStength[startingVal + i] = (((double)e.Key) / 10000);
                        }
                        else
                        {
                            outputStength[Convert.ToInt32(parse)] = (((double)e.Key) / 10000);
                        }
                    }
                }
            }
            else if (map.ContainsKey("osA"))
            {
                this.outputStength = new Dictionary<int, Double>();

                Dictionary<Double, List<String>> storedValues = (Dictionary<Double, List<String>>)map["osA"];
                foreach (KeyValuePair<Double, List<String>> e in storedValues)
                {
                    foreach (String parse in e.Value)
                    {
                        if (parse.Contains(","))
                        {
                            int startingVal = Convert.ToInt32(parse.Split(',')[0]);
                            int amountInSeries = Convert.ToInt32(parse.Split(',')[0]);
                            for (int i = 0; i < amountInSeries; i++)
                                outputStength[startingVal + i] = e.Key;
                        }
                        else
                        {
                            outputStength[Convert.ToInt32(parse)] = e.Key;
                        }
                    }
                }
            }
            else if (map.ContainsKey("os"))
            {
                // Legacy values:
                this.outputStength = (Dictionary<int, Double>)map["os"];
            }
            this.id = (int)map["id"];
            this.layer = (int)map["l"];
        }

    public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> m = new Dictionary<String, Object>();
            if (this.weight != 0)
                m["w"] = this.weight;
            if (this.weight != -0.5)
                m["t"] = this.threshold;
            if (this.useThreshold())
                m["uth"] = 1;
            if (this.allowNegativeValues)
                m["anv"] = 1;
            if (this.inheritBias != 0)
                m["bias"] = this.inheritBias;

            Dictionary<Double, List<int>> verseStrengths = new Dictionary<Double, List<int>>();
            foreach (KeyValuePair<int, Double> e in outputStength)
            {
                List<int> currentList = verseStrengths.ContainsKey(e.Value) ? verseStrengths[e.Value]
                        : new List<int>();
                currentList.Add(e.Key);
                verseStrengths[e.Value] = currentList;
            }

            List<int> verify = new List<int>();

            Dictionary<int, List<int>> savedOutputs = new Dictionary<int, List<int>>();
            foreach (Double val in verseStrengths.Keys.ToList())
            {
                List<int> vals = new List<int>();
                // Boolean first = true;
                foreach (int i in verseStrengths[val])
                {

                    verify.Add(i);
                    vals.Add(i);
                }
                int j = (int)(val * 10000);
                if (savedOutputs.ContainsKey(j))
                {
                    List<int> temp = vals;
                    vals = savedOutputs[j];
                    vals = vals.Union(temp).ToList();
                }
                savedOutputs[j] = vals;
            }
            // Somehow, neuron connections where not being saved. This should fix that
            if (ai.maxNeuralLayers != layer + 1)
            {
                foreach (Neuron n in ai.getNeuronsInNeuralLayer(layer + 1))
                {
                    if ((!verify.Contains(n.getID())) && this.hasOutputTo(n))
                    {
                        int j = (int)(this.getOutputForNeuron(n) * 10000);
                        List<int> vals = savedOutputs.ContainsKey(j) ? savedOutputs[j] : new List<int>();
                        vals.Add(n.getID());
                        savedOutputs[j] = vals;
                    }
                }
            }
            m["osC"] = savedOutputs;
            m["id"] = this.id;
            m["l"] = this.layer;
            return m;
        }

    }
}
