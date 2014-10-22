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



    }
}
