using System.Collections.Generic;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec20PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (char[,] grid, (int, int) start, (int, int) end) = ReadMap(test);

            var usedCheats = new HashSet<((int, int), (int, int))>();

            bool newCheat = true;
            var startNode = new GridNode { Position = start, Goal = end };

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var cheatSaves = new List<int>();

            // First, get the shortest path from start to end.
            // Memoize shortest distance from each square along the shortest path to end.
            var shortest = new Dictionary<(int, int), int>();

            List<(int, int)> path = ShortestPathLength(grid, start, end, shortest);

            int minPath = shortest[start];

            // Now look for all the cheats.
            for (int i = 0; i < path.Count; i++)
            {
                (int x, int y)  = path[i];
                foreach (((int, int) cheatStart, (int, int) cheatEnd) in GetCheats(grid, x, y))
                {
                    if (!shortest.ContainsKey(cheatEnd))
                    {
                        ShortestPathLength(grid, cheatEnd, end, shortest);
                    }

                    int finalCost = i + 2 + shortest[cheatEnd];
                    int saved = minPath - finalCost;
                    if (saved > 0)
                    {
                        cheatSaves.Add(saved);
                    }

                    usedCheats.Add((cheatStart, cheatEnd));
                }
            }

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

            return cheatSaves.Count(c => c >= 100).ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
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
            while(pred.ContainsKey(curr) && curr != start)
            {
                distance++;
                memoized[pred[curr]] = distance;
                curr = pred[curr];
                path.Insert(0, curr);
            }

            memoized[start] = minCost;

            return path;
        }

        private static IEnumerable<((int, int), (int, int))> GetCheats(char[,] grid, int i, int j)
        {
            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            foreach (((int startX, int startY), (int endX, int endY)) in GetCheatCandidates((i, j)))
            {
                if (OutOfBounds(startX, startY, numRows, numCols) || OutOfBounds(endX, endY, numRows, numCols))
                {
                    continue;
                }

                if (grid[startX, startY] == '#' && grid[endX, endY] != '#')
                {
                    yield return ((i, j), (endX, endY));
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

        private static IEnumerable<((int, int), (int, int))> GetCheatCandidates((int, int) current)
        {
            (int x, int y) = current;

            // up, up
            yield return ((x - 1, y), (x - 2, y));

            // up, right
            yield return ((x - 1, y), (x - 1, y + 1));

            // up, left
            yield return ((x - 1, y), (x - 1, y - 1));

            // right, right
            yield return ((x, y + 1), (x, y + 2));

            // right, up
            yield return ((x, y + 1), (x - 1, y + 1));

            // right, down
            yield return ((x, y + 1), (x + 1, y + 1));

            // down, down
            yield return ((x + 1, y), (x + 2, y));

            // down, right
            yield return ((x + 1, y), (x + 1, y + 1));

            // down, left
            yield return ((x + 1, y), (x + 1, y - 1));

            // left, left
            yield return ((x, y - 1), (x, y - 2));

            // left, up
            yield return ((x, y - 1), (x - 1, y - 1));

            // left, down
            yield return ((x, y - 1), (x + 1, y - 1));
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
