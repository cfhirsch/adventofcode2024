using System.Reflection;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec20PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test)
        {
            return Solve(test, isPartTwo: true);
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            (char[,] grid, (int, int) start, (int, int) end) = ReadMap(test);

            var startNode = new GridNode { Position = start, Goal = end };

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var cheatSaves = new List<int>();

            var visitedCheats = new HashSet<((int, int), (int, int))>();

            // First, get the shortest path from start to end.
            // Memoize shortest distance from each square along the shortest path to end.
            var shortest = new Dictionary<(int, int), int>();

            List<(int, int)> path = ShortestPathLength(grid, start, end, shortest);

            int minPath = shortest[start];

            int minLength = 2;
            int maxLength = isPartTwo ? 20 : 2;

            // Now look for all the cheats.
            for (int i = 0; i < path.Count; i++)
            {
                (int x, int y) = path[i];

                foreach ((int z, int w, int l) in GetReachable(x, y, minLength, maxLength))
                {
                    if (OutOfBounds(z, w, numRows, numCols))
                    {
                        continue;
                    }

                    if (grid[z, w] == '#')
                    {
                        continue;
                    }

                    if (visitedCheats.Contains(((x, y), (z, w))))
                    {
                        continue;
                    }

                    if (!shortest.ContainsKey((z, w)))
                    {
                        ShortestPathLength(grid, (z, w), end, shortest);
                    }

                    int finalCost = i + l + shortest[(z, w)];
                    int saved = minPath - finalCost;
                    if (saved > 0)
                    {
                        cheatSaves.Add(saved);
                    }

                    visitedCheats.Add(((x, y), (z, w)));
                }
            }

            int threshold = test ? 50 : 100;
            if (test)
            {
                var query = cheatSaves.GroupBy(
                    cheat => cheat,
                    cheat => cheat,
                    (cheatKey, cheats) => new
                    {
                        Key = cheatKey,
                        Count = cheats.Count(),
                    });

                foreach (var result in query.OrderBy(q => q.Key))
                {
                    Console.WriteLine($"There are {result.Count} cheats that save {result.Key} picoseconds.");
                }
            }

            return cheatSaves.Count(c => c >= threshold).ToString();
        }

        private static List<(int, int)> ShortestPathLength(
            char[,] grid,
            (int, int) start,
            (int, int) end,
            Dictionary<(int, int), int> memoized)
        {
            var queue = new PriorityQueue<GridNode, int>();
            var startNode = new GridNode { Position = start, Goal = end, Cost = 0 };
            queue.Enqueue(startNode, startNode.Score);
            var dist = new Dictionary<(int, int), int>();

            var pred = new Dictionary<(int, int), (int, int)>();

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            int minCost = Int32.MaxValue;

            while (queue.Count > 0)
            {
                GridNode current = queue.Dequeue();

                if (current.Position == end)
                {
                    minCost = current.Cost;
                    break;
                }

                foreach ((int x, int y) in GetNeighboringPoints(current.Position.Item1, current.Position.Item2))
                {
                    if (OutOfBounds(x, y, numRows, numCols))
                    {
                        continue;
                    }

                    if (grid[x, y] != '#')
                    {
                        int cost = current.Cost + 1;

                        if (!dist.ContainsKey((x, y)))
                        {
                            dist[(x, y)] = Int32.MaxValue;
                        }

                        if (cost < dist[(x, y)])
                        {
                            dist[(x, y)] = cost;
                            pred[(x, y)] = current.Position;
                            var next = new GridNode { Position = (x, y), Goal = end, Cost = cost };
                            queue.Enqueue(next, next.Score);
                        }
                    }
                }
            }

            var path = new List<(int, int)>();
            var curr = end;
            int distance = 0;

            path.Add(end);
            while (pred.ContainsKey(curr) && curr != start)
            {
                distance++;
                memoized[pred[curr]] = distance;
                curr = pred[curr];
                path.Insert(0, curr);
            }

            memoized[start] = minCost;

            return path;
        }

        private static IEnumerable<(int, int, int)> GetReachable(int i, int j, int minLength, int maxLength)
        {
            for (int l = minLength; l <= maxLength; l++)
            {
                for (int deltax = 0; deltax <= l; deltax++)
                {
                    int deltay = l - deltax;

                    if (deltax == 0)
                    {
                        yield return (i, j + deltay, l);

                        yield return (i, j - deltay, l);
                    }
                    else if (deltay == 0)
                    {
                        yield return (i + deltax, j, l);

                        yield return (i - deltax, j, l);
                    }
                    else
                    {
                        yield return (i - deltax, j - deltay, l);

                        yield return (i - deltax, j + deltay, l);

                        yield return (i + deltax, j - deltay, l);

                        yield return (i + deltax, j + deltay, l);
                    }
                }
            }
        }

        private static bool OutOfBounds(int x, int y, int numRows, int numCols)
        {
            if (x < 0 || x > numRows - 1 || y < 0 || y > numCols - 1)
            {
                return true;
            }

            return false;
        }

        private static IEnumerable<(int, int)> GetNeighboringPoints(int x, int y)
        {
            // Up
            yield return (x - 1, y);

            // Right
            yield return (x, y + 1);

            // Down
            yield return (x + 1, y);

            // Left
            yield return (x, y - 1);
        }

        private static (char[,], (int, int), (int, int)) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(20, test).ToList();

            var grid = new char[lines.Count, lines[0].Length];

            int numRows = lines.Count;
            int numCols = lines[0].Length;

            (int, int) startPos = (-1, -1);
            (int, int) goalPos = (-1, -1);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = lines[i][j];
                    if (grid[i, j] == 'S')
                    {
                        startPos = (i, j);
                    }

                    if (grid[i, j] == 'E')
                    {
                        goalPos = (i, j);
                    }
                }
            }

            return (grid, startPos, goalPos);
        }

        private class GridNode : IComparable<GridNode>
        {
            public (int, int) Position { get; set; }

            public (int, int) Goal { get; set; }

            public int Cost { get; set; }

            public int Score
            {
                get
                {
                    return this.Cost + Math.Abs(Goal.Item1 - this.Position.Item1) + Math.Abs(Goal.Item2 - this.Position.Item2);
                }
            }

            public int CompareTo(GridNode? other)
            {
                return this.Score.CompareTo(other.Score);
            }

            public override int GetHashCode()
            {
                return this.Position.GetHashCode();
            }
        }
    }
}
