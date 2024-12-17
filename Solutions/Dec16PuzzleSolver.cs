using System.ComponentModel;
using System.Security;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec16PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (char[,] grid, (int, int) start, (int, int) goal) = ReadMap(test);

            ReindeerDirection dir = ReindeerDirection.East;

            var queue = new PriorityQueue<ReindeerNode, long>();

            var node = new ReindeerNode { Goal = goal, Location = start, Direction = ReindeerDirection.East };
            queue.Enqueue(node, node.Score);

            var dict = new Dictionary<ReindeerNode, long>();
            dict.Add(node, 0);

            long score = Int64.MaxValue;
            while (queue.Count > 0)
            {
                ReindeerNode current = queue.Dequeue();
                if (current.Location == goal)
                {
                    score = current.Score;
                    break;
                }

                foreach (ReindeerNode neighbor in GetNeighbors(grid, current))
                {
                    if (!dict.ContainsKey(neighbor))
                    {
                        dict[neighbor] = Int64.MaxValue;
                    }

                    if (neighbor.Cost < dict[neighbor])
                    {
                        dict[neighbor] = neighbor.Cost;
                        queue.Enqueue(neighbor, neighbor.Score);
                    }
                }
            }

            return score.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<ReindeerNode> GetNeighbors(char[,] grid, ReindeerNode current)
        {
            int i, j;

            (int, int) reindeerPos = current.Location;
            switch (current.Direction)
            {
                case ReindeerDirection.North:
                    (i, j) = (reindeerPos.Item1 - 1, reindeerPos.Item2);
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.East, Location = current.Location };
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.West, Location = current.Location };

                    break;

                case ReindeerDirection.East:
                    (i, j) = (reindeerPos.Item1, reindeerPos.Item2 + 1);
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.North, Location = current.Location };
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.South, Location = current.Location };

                    break;

                case ReindeerDirection.South:
                    (i, j) = (reindeerPos.Item1 + 1, reindeerPos.Item2);
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.East, Location = current.Location };
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.West, Location = current.Location };

                    break;

                case ReindeerDirection.West:
                    (i, j) = (reindeerPos.Item1, reindeerPos.Item2 - 1);
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.North, Location = current.Location };
                    yield return new ReindeerNode { Cost = current.Cost + 1000, Goal = current.Goal, Direction = ReindeerDirection.South, Location = current.Location };

                    break;

                default:
                    throw new ArgumentException($"Unexpected direction {current.Direction}.");
            }

            if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1) &&
                grid[i, j] != '#')
            {
                yield return new ReindeerNode { Cost = current.Cost + 1, Direction = current.Direction, Location = (i, j), Goal = current.Goal };
            }
        }

        private static (char[,], (int, int), (int, int)) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(16, test).ToList();

            var grid = new char[lines.Count, lines[0].Length];

            int numRows = lines.Count;
            int numCols = lines[0].Length;

            (int, int) reindeerPos = (-1, -1);
            (int, int) goalPos = (-1, -1);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = lines[i][j];
                    if (grid[i, j] == 'S')
                    {
                        reindeerPos = (i, j);
                    }

                    if (grid[i, j] == 'E')
                    {
                        goalPos = (i, j);
                    }
                }
            }

            return (grid, reindeerPos, goalPos);
        }

        private class ReindeerNode : IComparable<ReindeerNode>, IEquatable<ReindeerNode>
        {
            public (int, int) Location { get; set; }

            public (int, int) Goal { get; set; }

            public int Cost { get; set; }

            public ReindeerDirection Direction { get; set; }

            public long Score
            {
                get
                {
                    long score = Math.Abs(Goal.Item1 - this.Location.Item1) + Math.Abs(Goal.Item2 - this.Location.Item2);
                    return this.Cost + score;
                }
            }

            public int CompareTo(ReindeerNode? other)
            {
                return this.Score.CompareTo(other.Score);
            }

            public bool Equals(ReindeerNode? other)
            {
                return this.Location == other.Location && this.Direction == other.Direction;
            }

            public override bool Equals(object? obj)
            {
                var other = obj as ReindeerNode;
                if (other == null)
                {
                    return false;
                }

                return this.Equals(other);
            }

            public override int GetHashCode()
            {
                return this.Location.GetHashCode() ^ this.Direction.GetHashCode();
            }
        }

        private enum ReindeerDirection
        {
            North,
            East,
            South,
            West
        }
    }
}
