using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public class MathUtil
    {

            public static double sigmoidNumber(double input)
            {
                return 1 / (1 + Math.Pow(Math.E, -input));
            }
            public static double sigmoidNumberPosAndNeg(double input)
            {
                return (sigmoidNumber(input) * 2) - 1;
            }
    }
}
