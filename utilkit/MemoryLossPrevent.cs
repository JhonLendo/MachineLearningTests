using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public class MemoryLossPrevent
    {
        public Dictionary<int, Double> inputValues;
        public Dictionary<int, Double> previousOutputValues;
        public Dictionary<int, Double> suggestOutputValues;

        private bool requireAllNeuronsTrainWithThis = false;

        public MemoryLossPrevent(Dictionary<int, Double> enter, Dictionary<int, Double> exit,
                Dictionary<int, Double> suggested)
        {
            this.inputValues = enter;
            this.previousOutputValues = exit;
            this.suggestOutputValues = suggested;
        }

        public MemoryLossPrevent(Dictionary<int, Double> enter, Dictionary<int, Double> suggested)
        {
            this.inputValues = enter;
            this.suggestOutputValues = suggested;
        }

        public void updatePreviousOutputs(Dictionary<int, Double> newOutput)
        {
            this.previousOutputValues = newOutput;
        }
        public bool needsToUse()
        {
            return requireAllNeuronsTrainWithThis;
        }
        public void setNeedToUse(bool b)
        {
            this.requireAllNeuronsTrainWithThis = b;
        }
    }
}
