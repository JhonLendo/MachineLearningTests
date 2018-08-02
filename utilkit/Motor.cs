using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public interface Motor
    {
        double getPowerFor(int x, int y);
    }
}
