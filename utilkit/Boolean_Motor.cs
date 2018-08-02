using MachineLearning.ml.utilkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.ml.utilkit
{
    public class Boolean_Motor : Motor
    {

        private Boolean[,] pv;

        public double getPowerFor(int x, int y)
        {
            return pv[x,y] ? 1 : 0;
        }

        public Boolean_Motor(int rows, int col)
        {
            this.pv = new Boolean[rows,col];
        }

        public bool getBooleanAt(int row, int col)
        {
            return pv[row,col];
        }

        public void changeMatrix(bool[,] newValues)
        {
            this.pv = newValues;
        }
        public bool[,] getMatrix()
        {
            return this.pv;
        }
        public void changeValueAt(int x, int y, bool value)
        {
            this.pv[x,y] = value;
        }

        public Boolean_Motor(Dictionary<String, Object> map)
        {
            pv = new bool[(int)map["x"],(int)map["y"]];
        }

    public Dictionary<String, Object> serialize()
        {
            Dictionary<String, Object> v = new Dictionary<String, Object>();
            v["x"] = pv.GetLength(0);
            v["y"] = pv.GetLength(1);
            return v;
        }
    }
}
