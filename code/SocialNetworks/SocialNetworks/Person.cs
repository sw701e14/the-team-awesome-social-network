using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworks
{
    public class Person
    {
        public static IEnumerable<Person> ParseFile(string filename)
        {
            var persons = new List<Tuple<Person, List<string>>>();

            string[] inp = File.ReadAllLines(filename);

            for (int i = 0; i < inp.Length; i += 5)
                persons.Add(getPersonAndFriendnames(inp, i));

            persons.Sort((x, y) => x.Item1.name.CompareTo(y.Item1.name));

            for (int i = 0; i < persons.Count; i++)
            {
                for (int j = 0; j < persons[i].Item2.Count; j++)
                {
                    string findName = persons[i].Item2[j];
                    int index = persons.BinarySearch(findName, (x, y) => x.CompareTo(y), x => x.Item1.name);
                    if (index < 0)
                        throw new ArgumentException("Person " + findName + " not found!");
                    persons[i].Item1.friends.Add(persons[index].Item1);
                }
                yield return persons[i].Item1;
            }
        }
        private static Tuple<Person, List<string>> getPersonAndFriendnames(string[] input, int index)
        {
            return Tuple.Create(
                new Person(getContent(input[index]), getContent(input[index + 2]), getContent(input[index + 3])),
                getFriends(input[index + 1]).ToList());
        }

        private static string getContent(string line)
        {
            return line.Substring(line.IndexOf(' ')).Trim();
        }
        private static string[] getFriends(string line)
        {
            return getContent(line).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }


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

        public override string ToString()
        {
            return name;
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

            public int Length
            {
                get { return friends.Count; }
            }

            public Person this[int index] { get { return this.friends[index]; } }
            
        }


    }
}
