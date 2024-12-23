using System.Diagnostics.Tracing;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec23PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            List<string> lines = PuzzleReader.GetPuzzleInput(23, test).ToList();

            var graph = new Graph();
            foreach (string line in lines)
            {
                string[] lineParts = line.Split('-');
                graph.Vertices.Add(lineParts[0]);
                graph.Vertices.Add(lineParts[1]);

                graph.AddEdge(lineParts[0], lineParts[1]);
            }

            HashSet<string> cliques = graph.FindCliques(3);

            int count = cliques.Count(c => c.Split(",").Any(v => v.StartsWith("t")));
            return count.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private class Graph
        {
            public Graph()
            {
                this.Vertices = new HashSet<string>();
                this.Edges = new HashSet<(string, string)>();
            }

            public HashSet<string> Vertices { get; set; }

            public HashSet<(string, string)> Edges { get; set; }

            public void AddEdge(string source, string target)
            {
                if (!this.Edges.Any(e => e.Item1 == target && e.Item2 == source))
                {
                    this.Edges.Add((source, target));
                }
            }

            public HashSet<string> FindCliques(int size)
            {
                var cliques = new HashSet<string>();
                if (size < 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(size));
                }

                if (size == 2)
                {
                    foreach ((string source, string target) in this.Edges)
                    {
                        var vertices = new string[] { source, target };

                        cliques.Add(string.Join(",", vertices.OrderBy(v => v)));
                    }

                    return cliques;
                }


                List<string> subcliques = FindCliques(size - 1).ToList();
                for (int i = 0; i < subcliques.Count - 1; i++)
                {
                    for (int j = i + 1; j < subcliques.Count; j++)
                    {
                        HashSet<string> clique1 = subcliques[i].Split(",").ToHashSet();
                        HashSet<string> clique2 = subcliques[j].Split(",").ToHashSet();

                        var intersection = clique1.Intersect(clique2).ToHashSet();
                        if (intersection.Count == size - 2)
                        {
                            var vertex1 = clique1.Except(clique2).First();
                            var vertex2 = clique2.Except(clique1).First();

                            if (this.Edges.Any(e => (e.Item1 == vertex1 && e.Item2 == vertex2) ||
                                                    (e.Item1 == vertex2 && e.Item2 == vertex1)))
                            {
                                HashSet<string> clique = intersection;
                                clique.Add(vertex1);
                                clique.Add(vertex2);

                                cliques.Add(string.Join(",", clique.OrderBy(v => v).ToArray()));

                                /*clique = clique.OrderBy(v => v).ToHashSet();

                                if (!cliques.Contains(clique))
                                {
                                    cliques.Add(clique);
                                }*/
                            }
                        }
                    }
                }

                return cliques;
            }
        }
    }
}
