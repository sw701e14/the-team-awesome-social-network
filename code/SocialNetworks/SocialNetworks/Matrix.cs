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

        public Matrix(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size");

            this.size = size;
            this.data = new double[size, size];
        }
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

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.size != b.size)
                throw new ArgumentException("Matrices must be of same size!");

            Matrix c = new Matrix(a.size);
            for (int y = 0; y < a.size; y++)
                for (int x = 0; x < a.size; x++)
                    c.data[x, y] = a.data[x, y] - b.data[x, y];

            return c;
        }
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.size != b.size)
                throw new ArgumentException("Matrices must be of same size!");

            Matrix c = new Matrix(a.size);
            for (int y = 0; y < a.size; y++)
                for (int x = 0; x < a.size; x++)
                    c.data[x, y] = a.data[x, y] + b.data[x, y];

            return c;
        }

        public static implicit operator double[,] (Matrix m)
        {
            return m.Data;
        }
        public double[,] Data
        {
            get { return data.Clone() as double[,]; }
        }

        public static Matrix CreateIdentityMatrix(int size)
        {
            Matrix matrix = new Matrix(size);
            for (int i = 0; i < size; i++)
                matrix.data[i, i] = 1.0;
            return matrix;
        }

        public static Matrix CreateAdjacencyMatrix(Person[] persons)
        {
            double[,] adjacencyMatrix = new double[persons.Length, persons.Length];
            for (int i = 0; i < persons.Length; i++)
                for (int j = 0; j < persons[i].Friends.Length; j++)
                {
                    int friendIndex = persons.BinarySearch(persons[i].Friends[j], (a, b) => a.Name.CompareTo(b.Name));
                    adjacencyMatrix[i, friendIndex] = 1;
                }

            return new Matrix(adjacencyMatrix);
        }
        public static Matrix CreateDegreeMatrix(Person[] persons)
        {
            Matrix m = new Matrix(persons.Length);
            for (int i = 0; i < persons.Length; i++)
                m.data[i, i] = persons[i].Friends.Length;
            return m;
        }
    }
}
