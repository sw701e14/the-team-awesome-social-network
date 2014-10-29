using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class ClusterLeaf<T>
    {
        private T item;

        public ClusterLeaf(T item)
        {
            this.item = item;
        }

        public T Item
        {
            get { return item; }
        }
    }
}
