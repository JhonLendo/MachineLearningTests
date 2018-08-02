using MachineLearning.ml.utilkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml
{
    public class InputNeuronBoolean : InputNeuron
    {
        public InputNeuronBoolean(AINetwork ai, Boolean_Motor sl)
        {
            this.s = sl;
        }

        public InputNeuronBoolean(AINetwork ai, int row, int col, Motor sl) : base(ai, row, col, sl)
        {
            this.s = sl;
        }

    public InputNeuron generateNeuron(AINetwork ai, Motor word)
        {
            return InputNeuronBoolean.createNeuron(ai, 0, ' ',
                     (Motor)word);
        }

        public static InputNeuron createNeuron(AINetwork ai, int row,
                int col, Motor sl)
        {
            InputNeuron link = new InputNeuronBoolean(ai, row, col, (Motor)sl);
            return link;
        }
        public InputNeuronBoolean(Dictionary<String, Object> map) : base(map)
        {
        }
    }
}
