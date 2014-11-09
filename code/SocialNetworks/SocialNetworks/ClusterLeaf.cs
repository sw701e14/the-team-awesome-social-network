using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class ClusterLeaf<T> : ClusterTree<T>
    {
        private const int TOSTRING_LENGTH = 3;
        private T[] items;

        public ClusterLeaf(ClusterNode<T> parent, IEnumerable<T> items, Func<T[], Matrix<double>> buildAdjacencyMatrix, Comparison<T> comparer)
            : base(parent, buildAdjacencyMatrix, comparer)
        {
            this.items = items.ToArray();
            Array.Sort<T>(this.items, comparer);
        }

        public void SplitAndReplace()
        {
            if (items.Length == 1)
                return;

            T[] p1, p2;
            SplitData(items, out p1, out p2, buildAdjacencyMatrix);

            var par = this.Parent;

            var node = ClusterNode<T>.Create(par, buildAdjacencyMatrix, comparer,
                n => new ClusterLeaf<T>(n, p1, buildAdjacencyMatrix, comparer),
                n => new ClusterLeaf<T>(n, p2, buildAdjacencyMatrix, comparer));

            par.Replace(this, node);
        }

        public T this[int index]
        {
            get { return items[index]; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

        public override string ToString()
        {
            T[] temp = new T[items.Length < TOSTRING_LENGTH ? items.Length : TOSTRING_LENGTH];
            Array.Copy(items, temp, temp.Length);
            return string.Format("Leaf<{0}>: {{{1}{2}}}",
                typeof(T).Name,
                string.Join(", ", temp),
                items.Length > TOSTRING_LENGTH ? ", ..." : "");
        }

        public override ClusterLeaf<T> FindLargestLeaf()
        {
            return this;
        }

        protected override IEnumerable<T> getData()
        {
            foreach (var t in items)
                yield return t;
        }
    }
}
