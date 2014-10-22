using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class Person
    {
        public Person(string name)
        {
            this.name = name;
        }

        private string name;

        public string Name
        {
            get { return name; }
        }


        
    }
}
