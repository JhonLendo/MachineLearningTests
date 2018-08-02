using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml
{
    public interface MasterNeuralManager
    {

        string Improve();

        void setBasedEntity(NeuralEntity entity);

        string ForceSuddenUpdate();

        void SetArgs(string[] args);

    }
}
