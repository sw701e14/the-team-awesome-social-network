using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class Person
    {
        public Person(string name, string summary, string review)
        {
            this.name = name;
            this.summary = summary;
            this.review = review;
            this.friends = new PersonCollection();
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private string summary;

        public string Summary
        {
            get { return summary; }
        }

        private string review;

        public string Review
        {
            get { return review; }
        }

        private PersonCollection friends;

        public PersonCollection Friends
        {
            get { return friends; }
        }  

        

        public class PersonCollection : IEnumerable<Person>
        {
            private List<Person> friends;
            public PersonCollection()
            {
                this.friends = new List<Person>();
            }

            public void Add(Person person)
            {
                this.friends.Add(person);
            }

            public void Remove(Person person)
            {
                this.friends.Remove(person);
            }

            public IEnumerator<Person> GetEnumerator()
            {
                return this.friends.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.friends.GetEnumerator();
            }
        }

        
    }
}
