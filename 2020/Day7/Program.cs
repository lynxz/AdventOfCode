using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using QuickGraph;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var prg = new Program();
            prg.FirstStar();
            prg.SecondStar();
        }

        void FirstStar()
        {
            var graph = GetGraph();
            var result = graph.Vertices.Where(v => v != "shiny gold").Count(v => graph.IsConnected(v, "shiny gold"));
            System.Console.WriteLine(result);
        }

        void SecondStar()
        {
            var graph = GetGraph();
            var result = graph.SumEdges("shiny gold");
            System.Console.WriteLine(result);
        }

        AdjacencyGraph<string, TaggedEdge<string, int>> GetGraph()
        {
            var data = File.ReadAllLines("data.txt");
            var graph = new AdjacencyGraph<string, TaggedEdge<string, int>>();
            var regEx = new Regex(@"(\d+)\s(\w+\s\w+)\sbag");

            foreach (var l in data)
            {
                var bag = l.Substring(0, l.IndexOf("bags") - 1);
                if (!graph.ContainsVertex(bag))
                {
                    graph.AddVertex(bag);
                }
                var subBags = regEx.Matches(l.Split("contain")[1]);
                foreach (Match match in subBags)
                {
                    var subBag = match.Groups[2].Value;
                    if (!graph.ContainsVertex(subBag))
                    {
                        graph.AddVertex(subBag);
                    }
                    var count = int.Parse(match.Groups[1].Value);
                    graph.AddEdge(new TaggedEdge<string, int>(bag, subBag, count));
                }
            }

            return graph;
        }
    }

    static class Extensions
    {
        public static bool IsConnected(this AdjacencyGraph<string, TaggedEdge<string, int>> graph, string start, string end)
            => graph.OutEdges(start).Any(e => e.Target == end) || graph.OutEdges(start).Any(e => IsConnected(graph, e.Target, end));

        public static int SumEdges(this AdjacencyGraph<string, TaggedEdge<string, int>> graph, string vertex)
            => graph.OutEdges(vertex).Sum(e => e.Tag + e.Tag * graph.SumEdges(e.Target));

    }
}
