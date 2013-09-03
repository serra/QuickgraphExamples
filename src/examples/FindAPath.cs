using System;
using System.Collections.Generic;
using NUnit.Framework;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Graphviz;
using examples.Support;

namespace examples
{
    [TestFixture]
    public class FindAPath
    {
        private AdjacencyGraph<string, Edge<string>> _graph;
        private Dictionary<Edge<string>, double> _costs;

        [SetUp]
        public void SetUpEdgesAndCosts()
        {
            _graph = new AdjacencyGraph<string, Edge<string>>();
            _costs = new Dictionary<Edge<string>, double>();
   
            AddEdgeWithCosts("A", "D", 4.0);
            AddEdgeWithCosts("D", "E", 3.0);
            AddEdgeWithCosts("D", "B", 1.0);
            AddEdgeWithCosts("B", "D", 1.0);
            AddEdgeWithCosts("B", "E", 1.0);
            AddEdgeWithCosts("E", "A", 6.0);
            AddEdgeWithCosts("C", "E", 4.0);
            AddEdgeWithCosts("C", "B", 1.0);
        }

        private void AddEdgeWithCosts(string source, string target, double cost)
        {
            var edge = new Edge<string>(source, target);
            _graph.AddVerticesAndEdge(edge);
            _costs.Add(edge, cost);
        }

        void CostEdgeFormatter(object sender, FormatEdgeEventArgs<string, Edge<string>> e)
        {
            e.EdgeFormatter.Label.Value = _costs[e.Edge].ToString("0.0");
        }

        [Test]
        public void Visualize()
        {
            Console.WriteLine(_graph.ToDotNotation());
            _graph.Visualize("distance", CostEdgeFormatter);
        }

        [Test]
        public void ShortesDistanceTest()
        {
            PrintShortestPath("A", "E");
        }

        private void PrintShortestPath(string @from, string to)
        {
            var edgeCost = AlgorithmExtensions.GetIndexer(_costs);
            var tryGetPath = _graph.ShortestPathsDijkstra(edgeCost, @from);

            IEnumerable<Edge<string>> path;
            if (tryGetPath(to, out path))
            {
                PrintPath(@from, to, path);
            }
            else
            {
                Console.WriteLine("No path found from {0} to {1}.");
            }
        }

        private static void PrintPath(string @from, string to, IEnumerable<Edge<string>> path)
        {
            Console.Write("Path found from {0} to {1}: {0}", @from, to);
            foreach (var e in path)
                Console.Write(" > {0}", e.Target);
            Console.WriteLine();
        }
    }
}
