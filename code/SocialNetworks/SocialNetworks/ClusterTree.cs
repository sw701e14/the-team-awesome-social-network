using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public abstract class ClusterTree<T> : IEnumerable<T>
    {
        private ClusterNode<T> parent;
        protected readonly Func<T[], Matrix<double>> buildAdjacencyMatrix;
        protected readonly Comparison<T> comparer;

        private static double[] getVectorValues(T[] data, Func<T[], Matrix<double>> buildAdjacencyMatrix)
        {
            Matrix<double> A = buildAdjacencyMatrix(data);
            Matrix<double> D = Matrix<double>.Build.DenseOfDiagonalArray(A.ColumnAbsoluteSums().ToArray());
            Matrix<double> L = D - A;

            var evd = L.Evd();
            var eigenvectors = evd.EigenVectors;

            double[] vectorvalues = new double[data.Length];

            for (int i = 0; i < vectorvalues.Length; i++)
                vectorvalues[i] = eigenvectors[i, 1];

            return vectorvalues;
        }
        protected static void SplitData(T[] input, out T[] set1, out T[] set2, Func<T[], Matrix<double>> buildAdjacencyMatrix)
        {
            double[] values = getVectorValues(input, buildAdjacencyMatrix);
            List<Tuple<T, double>> data = new List<Tuple<T, double>>(input.Length);

            for (int i = 0; i < input.Length; i++) data.Add(Tuple.Create(input[i], values[i]));
            data.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            double diff = double.NegativeInfinity;
            int index = -1;

            for (int i = 1; i < data.Count; i++)
            {
                double d = Math.Abs(data[i - 1].Item2 - data[i].Item2);
                if (d > diff)
                {
                    diff = d;
                    index = i - 1;
                }
            }

            set1 = new T[index + 1];
            set2 = new T[input.Length - index - 1];
            for (int i = 0; i < data.Count; i++)
                if (i < set1.Length)
                    set1[i] = data[i].Item1;
                else
                    set2[i - index - 1] = data[i].Item1;
        }

        public static ClusterTree<T> FromData(T[] data, Func<T[], Matrix<double>> buildAdjacencyMatrix, Comparison<T> comparer)
        {
            T[] p1, p2;
            SplitData(data, out p1, out p2, buildAdjacencyMatrix);

            return ClusterNode<T>.Create(null, buildAdjacencyMatrix, comparer,
                n => new ClusterLeaf<T>(n, p1, buildAdjacencyMatrix, comparer),
                n => new ClusterLeaf<T>(n, p2, buildAdjacencyMatrix, comparer));
        }
        protected ClusterTree(ClusterNode<T> parent, Func<T[], Matrix<double>> buildAdjacencyMatrix, Comparison<T> comparer)
        {
            this.parent = parent;
            this.buildAdjacencyMatrix = buildAdjacencyMatrix;
            this.comparer = comparer;
        }

        public ClusterNode<T> Parent
        {
            get { return parent; }
        }
        public abstract int Count
        {
            get;
        }

        public abstract ClusterLeaf<T> FindLargestLeaf();
        public void SplitLargestLeaf()
        {
            FindLargestLeaf().SplitAndReplace();
        }

        protected abstract IEnumerable<T> getData();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (var t in getData())
                yield return t;
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var t in getData())
                yield return t;
        }
    }
}
