using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public struct Matrix
    {
        private int size;
        private double[,] data;

        public Matrix(double[,] data)
        {
            if (data.GetLength(0) != data.GetLength(1))
                throw new ArgumentException("Array must be square!");

            this.size = data.GetLength(0);

            this.data = new double[data.GetLength(0), data.GetLength(0)];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    this.data[x, y] = data[x, y];
        }
    }
}
