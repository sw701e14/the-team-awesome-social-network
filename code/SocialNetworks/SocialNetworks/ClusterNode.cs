using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class ClusterNode<T> : ClusterTree<T>
    {
        private ClusterTree<T> left, right;

        public static ClusterNode<T> Create(ClusterNode<T> parent, Func<T[], Matrix<double>> buildAdjacencyMatrix, Comparison<T> comparer, Func<ClusterNode<T>, ClusterTree<T>> left, Func<ClusterNode<T>, ClusterTree<T>> right)
        {
            ClusterNode<T> node = new ClusterNode<T>(parent, buildAdjacencyMatrix, comparer);

            node.left = left(node);
            node.right = right(node);

            return node;
        }

        private ClusterNode(ClusterNode<T> parent, Func<T[], Matrix<double>> buildAdjacencyMatrix, Comparison<T> comparer)
            : base(parent, buildAdjacencyMatrix, comparer)
        {
        }

        public void Replace(ClusterTree<T> oldTree, ClusterTree<T> newTree)
        {
            if (left == oldTree)
                left = newTree;
            else if (right == oldTree)
                right = newTree;
            else
                throw new ArgumentException("Tree is not a child in this node.", "oldTree");
        }

        public ClusterTree<T> LeftCluster
        {
            get { return left; }
        }
        public ClusterTree<T> RightCluster
        {
            get { return right; }
        }

        public override int Count
        {
            get { return left.Count + right.Count; }
        }

        public override string ToString()
        {
            return string.Format("Node<{0}>{{{1}}}", typeof(T).Name, Count);
        }

        public override ClusterLeaf<T> FindLargestLeaf()
        {
            var l = left.FindLargestLeaf();
            var r = right.FindLargestLeaf();

            return l.Count > r.Count ? l : r;
        }

        protected override IEnumerable<T> getData()
        {
            foreach (var t in left)
                yield return t;

            foreach (var t in right)
                yield return t;
        }
    }
}
