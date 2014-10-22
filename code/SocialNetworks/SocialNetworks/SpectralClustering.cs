using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Decompositions;

namespace SocialNetworks
{
    public class SpectralClustering
    {
        private double[,] createIdentityMatrix(int size)
        {
            double[,] identityMatrix = new double[size, size];
            for (int i = 0; i < size; i++)
                identityMatrix[i,i] = 1.0;
            return identityMatrix;
        }

        private double[,] createAdjacencyMatrix(Person[] persons)
        {
            double[,] adjacencyMatrix = new double[persons.Length, persons.Length];
            for (int i = 0; i < adjacencyMatrix.Length; i++)
                for (int j = 0; j < persons[i].Friends.Length; j++)
                {
                    int friendIndex = persons.BinarySearch(persons[i].Friends[j], (a, b) => a.Name.CompareTo(b.Name));
                    adjacencyMatrix[i, friendIndex] = 1;
                }
            return adjacencyMatrix;
                
        }



    }
}
