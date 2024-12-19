using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec16PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (int score, _) = Solve(test, isPartTwo: false);
            return score.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            /* (_, int numSquares) = Solve(test, isPartTwo: true);
             return numSquares.ToString();*/

            return "NotSolved";
        }

        public static (int, int) Solve(bool test, bool isPartTwo)
        {
            (char[,] grid, (int, int) start, (int, int) goal) = ReadMap(test);

            ReindeerDirection dir = ReindeerDirection.East;

            var queue = new PriorityQueue<ReindeerNode, long>();

            var node = new ReindeerNode(0, start, goal, ReindeerDirection.East);
            queue.Enqueue(node, node.Score);

            var dict = new Dictionary<ReindeerNode, long>();
            dict.Add(node, 0);

            var onShortestPath = new HashSet<(int, int)>();
            onShortestPath.Add(goal);

            // Use A* search to find the length of the shortest path from start to goal.
            int score = Int32.MaxValue;
            while (queue.Count > 0)
            {
                ReindeerNode current = queue.Dequeue();
                if (current.Location == goal)
                {
                    score = current.Score;

                    if (isPartTwo)
                    {
                        onShortestPath = onShortestPath.Union(current.Predecessors).ToHashSet();
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (ReindeerNode neighbor in GetNeighbors(grid, current))
                {
                    if (!dict.ContainsKey(neighbor))
                    {
                        dict[neighbor] = Int64.MaxValue;
                    }

                    bool enqueue = isPartTwo ? (neighbor.Cost <= dict[neighbor]) : (neighbor.Cost < dict[neighbor]);
                    if (enqueue)
                    {
                        dict[neighbor] = neighbor.Cost;
                        queue.Enqueue(neighbor, neighbor.Score);
                    }
                }
            }

            return (score, onShortestPath.Count());
        }

        private static IEnumerable<ReindeerNode> GetNeighbors(char[,] grid, ReindeerNode current)
        {
            int i, j;

            (int, int) reindeerPos = current.Location;
            HashSet<(int, int)> predecessors = current.Predecessors.Select(x => x).ToHashSet();
            switch (current.Direction)
            {
                case ReindeerDirection.North:
                    (i, j) = (reindeerPos.Item1 - 1, reindeerPos.Item2);
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.East,
                        predecessors);

                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.West,
                        predecessors);

                    break;

                case ReindeerDirection.East:
                    (i, j) = (reindeerPos.Item1, reindeerPos.Item2 + 1);
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.North,
                        predecessors);

                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.South,
                        predecessors);

                    break;

                case ReindeerDirection.South:
                    (i, j) = (reindeerPos.Item1 + 1, reindeerPos.Item2);
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.East,
                        predecessors);
                   
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.West,
                        predecessors);
                   
                    break;

                case ReindeerDirection.West:
                    (i, j) = (reindeerPos.Item1, reindeerPos.Item2 - 1);
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.North,
                        predecessors);
                    
                    yield return new ReindeerNode(
                        current.Cost + 1000,
                        current.Location,
                        current.Goal,
                        ReindeerDirection.South,
                        predecessors);

                    break;

                default:
                    throw new ArgumentException($"Unexpected direction {current.Direction}.");
            }

            predecessors.Add(current.Location);
            if (i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1) &&
                grid[i, j] != '#')
            {
                yield return new ReindeerNode(
                        current.Cost + 1,
                        (i, j),
                        current.Goal,
                        current.Direction,
                        predecessors);
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

        public class ReindeerNode : IComparable<ReindeerNode>, IEquatable<ReindeerNode>
        {
            public ReindeerNode(
                int cost, 
                (int, int) location,
                (int, int) goal, 
                ReindeerDirection direction,
                HashSet<(int, int)> predecessors = null)
            {
                this.Cost = cost;
                this.Location = location;
                this.Goal = goal;
                this.Direction = direction;

                if (predecessors == null)
                {
                    this.Predecessors = new HashSet<(int, int)>();
                }
                else
                {
                    this.Predecessors = predecessors.Select(x => x).ToHashSet();
                }
            }

            public (int, int) Location { get; set; }

            public (int, int) Goal { get; set; }

            public int Cost { get; set; }

            public ReindeerDirection Direction { get; set; }

            public HashSet<(int, int)> Predecessors { get; set; }

            public int Score
            {
                get
                {
                    int score = Math.Abs(Goal.Item1 - this.Location.Item1) + Math.Abs(Goal.Item2 - this.Location.Item2);
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

        public enum ReindeerDirection
        {
            North,
            East,
            South,
            West
        }
    }
}
