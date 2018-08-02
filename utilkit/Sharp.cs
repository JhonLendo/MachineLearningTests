using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public class Sharp
    {
        Boolean[] sharpness;

        int index = 0;

        double percentage = 0;

        public Sharp(int maximum)
        {
            sharpness = new Boolean[maximum];
        }

        public double getAccuracy()
        {
            return percentage;
        }
        public int getAccuracyAsInt()
        {
            return (int)(getAccuracy() * 100);
        }

        public void addPoolMember(Boolean results)
        {
            Boolean previous = sharpness[index];
            sharpness[index] = results;
            if (previous != results)
            {
                this.percentage += (results ? 1.0 : -1.0) / sharpness.Length;
            }
            index++;
            if (index == sharpness.Length)
            {
                index = 0;
                foreach (Boolean b in sharpness)
                {
                    if (b)
                        percentage += 1 / sharpness.Length;
                }
            }
        }

    }
}
