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

        public static Matrix CreateIdentityMatrix(int size)
        {
            Matrix matrix = new Matrix() { data = new double[size, size], size = size };
            for (int i = 0; i < size; i++)
                matrix.data[i, i] = 1.0;
            return matrix;
        }

        public Matrix CreateAdjacencyMatrix(Person[] persons)
        {
            double[,] adjacencyMatrix = new double[persons.Length, persons.Length];
            for (int i = 0; i < adjacencyMatrix.Length; i++)
                for (int j = 0; j < persons[i].Friends.Length; j++)
                {
                    int friendIndex = persons.BinarySearch(persons[i].Friends[j], (a, b) => a.Name.CompareTo(b.Name));
                    adjacencyMatrix[i, friendIndex] = 1;
                }

            return new Matrix(adjacencyMatrix);
        }
    }
}
