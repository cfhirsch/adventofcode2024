using System.Xml;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec18PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            List<(int, int)> data = GetPuzzleData(test);
            int maxX = test ? 6 : 70;
            int maxY = test ? 6 : 70;
            int numSteps = test ? 12 : 1024;

            var queue = new PriorityQueue<QueueNode, int>();
            (int, int) goal = (maxX, maxY);
            var start = new QueueNode { Position = (0, 0), Goal = goal };

            queue.Enqueue(start, start.Score);
            var dict = new Dictionary<(int, int), int>();

            int cost = Int32.MaxValue;
            while (queue.Count > 0)
            {
                QueueNode current = queue.Dequeue();
                if (current.Position == goal)
                {
                    cost = current.Cost;
                    break;
                }

                foreach ((int, int) neighbor in GetNeighbors(current.Position, maxX, maxY, numSteps, data))
                {
                    var neighborNode = new QueueNode { Cost = current.Cost + 1, Position = neighbor, TimeSteps = current.TimeSteps + 1 };

                    if (!dict.ContainsKey(neighbor))
                    {
                        dict[neighbor] = Int32.MaxValue;
                    }

                    if (neighborNode.Cost < dict[neighbor])
                    {
                        dict[neighbor] = neighborNode.Cost;
                        queue.Enqueue(neighborNode, neighborNode.Score);
                    }
                }
            }

            return cost.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<(int, int)> GetNeighbors((int, int) p, int maxX, int maxY, int timeStep, List<(int, int)> data)
        {
            foreach ((int x, int y) in GetNeighboringPoints(p))
            {
                if (x < 0 || x > maxX || y < 0 || y > maxY)
                {
                    continue;
                }

                if (data.Take(timeStep).Any(d => d == (x, y)))
                {
                    continue;
                }

                yield return (x, y);
            }
        }

        private IEnumerable<(int, int)> GetNeighboringPoints((int, int) p)
        {
            (int x, int y) = p;

            yield return (x - 1, y);
            yield return (x + 1, y);
            yield return (x, y - 1);
            yield return (x, y + 1);
        }

        private static List<(int, int)> GetPuzzleData(bool test)
        {
            var data = new List<(int, int)>();

            foreach (string line in PuzzleReader.GetPuzzleInput(18, test))
            {
                string[] parts = line.Split(',');
                data.Add((Int32.Parse(parts[0]), Int32.Parse(parts[1])));
            }

            return data;
        }

        private class QueueNode : IComparable<QueueNode>
        {
            public (int, int) Position { get; set; }

            public int Cost { get; set; }

            public (int, int) Goal { get; set; }

            public int TimeSteps { get; set; }

            public int Score
            {
                get
                {
                    return this.Cost + Math.Abs(Goal.Item1 - this.Position.Item1) + Math.Abs(Goal.Item2 - this.Position.Item2);
                }
            }

            public int CompareTo(QueueNode? other)
            {
                return this.Score.CompareTo(other.Score);
            }
        }
    }
}
