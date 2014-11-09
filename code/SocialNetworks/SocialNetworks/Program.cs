using Accord.Math.Decompositions;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialNetworks
{
    class Program
    {
        static void MainOld(string[] args)
        {
            Person[] people = Person.ParseFile(@"C:\Users\Mikkel\Documents\Git\sw7\the-team-awesome-social-network\code\friendships.txt").ToArray();

            Matrix D = Matrix.CreateDegreeMatrix(people);
            Matrix A = Matrix.CreateAdjacencyMatrix(people);

            Matrix L = D - A;

            Console.WriteLine("Started at {0}", DateTime.Now);
            DateTime start = DateTime.Now;
            var decomp = new GeneralizedEigenvalueDecomposition(L, Matrix.CreateIdentityMatrix(people.Length));
            DateTime end = DateTime.Now;

            Console.WriteLine("Completed in: {0}", (end - start).TotalSeconds);
            Console.ReadKey(true);
        }
        static void Main(string[] args)
        {
            Control.UseNativeMKL();
            Control.UseMultiThreading();

            Person[] people = Person.ParseFile(@"C:\Users\Mikkel\Documents\Git\sw7\the-team-awesome-social-network\code\friendships.txt").ToArray();
            //GetHTMLFile(null);
            //GetFile(people);
            //return;

            //ClusterTree<Person> tree = ClusterTree<Person>.FromData(people, buildAdjacencyMatrix, compare);
            //for (int i = 0; i < 4; i++)
            //    tree.SplitLargestLeaf();

            GetImageFile(people);

            //var tree = ClusterTree<Person>.Build(people, buildAdjacencyMatrix, (x, y) => x.Name.CompareTo(y.Name));
        }

        public static void GetFile(Person[] persons)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph {");
            sb.AppendLine("    rankdir=LR;");

            foreach (var p in persons)
                sb.AppendLine(string.Format("    {1}[label=\"{0}\"]", p.Name, p.Name.Replace("-", "QWERTY")));

            foreach (var p in persons)
                foreach (var f in p.Friends)
                {
                    sb.AppendLine(string.Format("    {0} -> {1}[label=\"\"];", p.Name.Replace("-", "QWERTY"), f.Name.Replace("-", "QWERTY")));
                }

            sb.AppendLine("}");
            System.IO.File.WriteAllText(@"C:\Users\Mikkel\Desktop\graphviz-2.38\friends.dot", sb.ToString());
        }

        public static void GetImageFile(Person[] persons)
        {
            Bitmap bmp = new Bitmap(persons.Length, persons.Length, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

                int y;

                #region Clusters

                //Queue<Tuple<ClusterTree<Person>, bool>> clusters = new Queue<Tuple<ClusterTree<Person>, bool>>();
                //clusters.Enqueue(Tuple.Create(persons, false));
                //do
                //{
                //    bool output = clusters.Any(x => x.Item2);

                //    Queue<Tuple<ClusterTree<Person>, bool>> newQ = new Queue<Tuple<ClusterTree<Person>, bool>>();
                //    if (output)
                //    {
                //        writer.WriteLine("<tr>");
                //        writer.WriteLine("<td class=\"lines\"></td>");
                //    }
                //    while (clusters.Count > 0)
                //    {
                //        var item = clusters.Dequeue();
                //        var c = item.Item1;
                //        var show = item.Item2;

                //        if (c is ClusterNode<Person>)
                //        {
                //            var node = c as ClusterNode<Person>;
                //            newQ.Enqueue(Tuple.Create(node.LeftCluster, true));
                //            newQ.Enqueue(Tuple.Create(node.RightCluster, true));
                //        }
                //        else
                //            newQ.Enqueue(Tuple.Create(c, false));

                //        if (output)
                //        {
                //            if (show)
                //                writer.WriteLine("<td class=\"line\" colspan=\"" + c.Count + "\">&nbsp;</td>");
                //            else
                //                writer.WriteLine("<td class=\"lineoff\" colspan=\"" + c.Count + "\">&nbsp;</td>");
                //        }
                //    }
                //    if (output)
                //        writer.WriteLine("</tr>");
                //    clusters = newQ;
                //} while (clusters.Any(x => x.Item2));

                #endregion

                y = 0;
                foreach (var p in persons)
                {
                    int x = 0;
                    foreach (var f in persons)
                    {
                        if (p.Friends.Contains(f))
                            g.FillRectangle(Brushes.Black, y, x, 1, 1);
                        x++;
                    }
                    y++;
                }

            }

            bmp.Save(@"C:\Users\Mikkel\Desktop\graphviz-2.38\output2.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void GetHTMLFile(ClusterTree<Person> persons)
        {
            string original = System.IO.File.ReadAllText(@"C:\Users\Mikkel\Desktop\graphviz-2.38\clusters.htm");

            string start = Regex.Match(original, @"^.*<!-- start -->", RegexOptions.Singleline).Value;
            string end = Regex.Match(original, @"<!-- end -->.*$", RegexOptions.Singleline).Value;

            System.IO.StreamWriter writer = new System.IO.StreamWriter(@"C:\Users\Mikkel\Desktop\graphviz-2.38\output.htm", false, Encoding.ASCII);

            writer.Write(start);

            writer.WriteLine("<table>");

            writer.WriteLine("<tr>");
            writer.WriteLine("<th></th>");
            foreach (var p in persons)
                writer.WriteLine("<th><div class=\"rotate\">" + p.Name + "</div></th>");
            writer.WriteLine("</tr>");

            Queue<Tuple<ClusterTree<Person>, bool>> clusters = new Queue<Tuple<ClusterTree<Person>, bool>>();
            clusters.Enqueue(Tuple.Create(persons, false));
            do
            {
                bool output = clusters.Any(x => x.Item2);

                Queue<Tuple<ClusterTree<Person>, bool>> newQ = new Queue<Tuple<ClusterTree<Person>, bool>>();
                if (output)
                {
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<td class=\"lines\"></td>");
                }
                while (clusters.Count > 0)
                {
                    var item = clusters.Dequeue();
                    var c = item.Item1;
                    var show = item.Item2;

                    if (c is ClusterNode<Person>)
                    {
                        var node = c as ClusterNode<Person>;
                        newQ.Enqueue(Tuple.Create(node.LeftCluster, true));
                        newQ.Enqueue(Tuple.Create(node.RightCluster, true));
                    }
                    else
                        newQ.Enqueue(Tuple.Create(c, false));

                    if (output)
                    {
                        if (show)
                            writer.WriteLine("<td class=\"line\" colspan=\"" + c.Count + "\">&nbsp;</td>");
                        else
                            writer.WriteLine("<td class=\"lineoff\" colspan=\"" + c.Count + "\">&nbsp;</td>");
                    }
                }
                if (output)
                    writer.WriteLine("</tr>");
                clusters = newQ;
            } while (clusters.Any(x => x.Item2));

            foreach (var p in persons)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("<td class=\"name\">" + p.Name + "</td>");
                foreach (var f in persons)
                {
                    if (p.Friends.Contains(f))
                        writer.WriteLine("<td class=\"on\">&bull;</td>");
                    else
                        writer.WriteLine("<td class=\"off\">&nbsp;</td>");
                }
                writer.WriteLine("</tr>");
            }

            writer.WriteLine("</table>");
            writer.Write(end);
            writer.Close();
            writer.Dispose();
        }

        public static int compare(Person x, Person y)
        {
            return x.Name.CompareTo(y.Name);
        }
        public static Matrix<double> buildAdjacencyMatrix(Person[] persons)
        {
            Matrix<double> matrix = Matrix<double>.Build.Dense(persons.Length, persons.Length);
            for (int i = 0; i < persons.Length; i++)
                foreach (var f in persons[i].Friends)
                {
                    int fIndex = persons.BinarySearch(f, (x, y) => x.Name.CompareTo(y.Name));
                    if (fIndex >= 0)
                        matrix[i, fIndex] = 1;
                }

            return matrix;
        }
    }
}
