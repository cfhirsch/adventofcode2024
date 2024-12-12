using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec12PuzzleSolver : IPuzzleSolver
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
            char[,] grid = ReadMap(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var plots = new List<HashSet<(int, int)>>();
            var visited = new HashSet<(int, int)>();

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (!visited.Contains((i, j)))
                    {
                        plots.Add(GetPlot(grid[i, j], grid, (i, j), visited));
                    }
                }
            }

            long price = 0;
            foreach (HashSet<(int, int)> plot in plots)
            {
                (int perimeter, int sides) = Calculate(plot);

                price += plot.Count() * (isPartTwo ? sides : perimeter);
            }

            return price.ToString();
        }

        private static HashSet<(int, int)> GetPlot(char c, char[,] grid, (int, int) start, HashSet<(int, int)> visited)
        {
            var squares = new HashSet<(int, int)>();
            var queue = new Queue<(int, int)>();
            queue.Enqueue(start);
            squares.Add(start);
            
            while (queue.Count > 0)
            {
                (int, int) current = queue.Dequeue();
                visited.Add(current);
                foreach ((int, int) neighbor in GetNeighbors(c, grid, current))
                {
                    if (!squares.Contains(neighbor))
                    {
                        squares.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return squares;
        }

        private static (int, int) Calculate(HashSet<(int, int)> squares)
        {
            int perimeter = 0;
            int numSides = 0;

            var lineSegments = new List<((int, int), SegmentPosition)>();

            
            foreach ((int, int) square in squares)
            {
                var nextSegments = new List<List<(int, int)>>();

                // Is there a square above the current one?
                if (!squares.Any(s => s.Item1 == square.Item1 - 1 && s.Item2 == square.Item2))
                {
                    perimeter++;
                    lineSegments.Add((square, SegmentPosition.Top));
                }

                // Is there a square below the current one?
                if (!squares.Any(s => s.Item1 == square.Item1 + 1 && s.Item2 == square.Item2))
                {
                    perimeter++;
                    lineSegments.Add((square, SegmentPosition.Bottom));
                }

                // Is there a square to the left of the current one?
                if (!squares.Any(s => s.Item1 == square.Item1 && s.Item2 == square.Item2 - 1))
                {
                    perimeter++;
                    lineSegments.Add((square, SegmentPosition.Left));
                }

                // Is there a square to the right of the current one?
                if (!squares.Any(s => s.Item1 == square.Item1 && s.Item2 == square.Item2 + 1))
                {
                    perimeter++;
                    lineSegments.Add((square, SegmentPosition.Right));
                }
            }

            int sides = 0;

            // Now we need to collapse the line segments into equivalence classes.
            // Two segments are in the same class if they are in the same position, and
            // they are on the same line.

            // Let's look at the segments along the top
            var points = lineSegments.Where(l => l.Item2 == SegmentPosition.Top).Select(l => l.Item1).OrderBy(l => l).ToList();

            while (points.Any())
            {
                var current = points.First();
                points.Remove(current);

                while (points.Any(p => p.Item1 == current.Item1 && p.Item2 == current.Item2 + 1))
                {
                    current = points.First(p => p.Item1 == current.Item1 && p.Item2 == current.Item2 + 1);
                    points.Remove(current);
                }

                sides++;
            }

            // Let's look at the segments along the bottom.
            points = lineSegments.Where(l => l.Item2 == SegmentPosition.Bottom).Select(l => l.Item1).OrderBy(l => l).ToList();

            while (points.Any())
            {
                var current = points.First();
                points.Remove(current);

                while (points.Any(p => p.Item1 == current.Item1 && p.Item2 == current.Item2 + 1))
                {
                    current = points.First(p => p.Item1 == current.Item1 && p.Item2 == current.Item2 + 1);
                    points.Remove(current);
                }

                sides++;
            }

            // Let's look at the segments along the left.
            points = lineSegments.Where(l => l.Item2 == SegmentPosition.Left).Select(l => l.Item1).OrderBy(l => l).ToList();

            while (points.Any())
            {
                var current = points.First();
                points.Remove(current);

                while (points.Any(p => p.Item1 == current.Item1 + 1 && p.Item2 == current.Item2))
                {
                    current = points.First(p => p.Item1 == current.Item1 + 1 && p.Item2 == current.Item2);
                    points.Remove(current);
                }

                sides++;
            }

            // Let's look at the segments along the right.
            points = lineSegments.Where(l => l.Item2 == SegmentPosition.Right).Select(l => l.Item1).OrderBy(l => l).ToList();

            while (points.Any())
            {
                var current = points.First();
                points.Remove(current);

                while (points.Any(p => p.Item1 == current.Item1 + 1 && p.Item2 == current.Item2))
                {
                    current = points.First(p => p.Item1 == current.Item1 + 1 && p.Item2 == current.Item2);
                    points.Remove(current);
                }

                sides++;
            }

            return (perimeter, sides);
        }

        private static IEnumerable<(int, int)> GetNeighbors(char c, char[,] grid, (int, int) current)
        {
            (int x, int y) = current;

            // Up
            if (x > 0 && grid[x - 1, y] == c)
            {
                yield return (x - 1, y);
            }

            // Down
            if (x < grid.GetLength(0) - 1 && grid[x + 1, y] == c)
            {
                yield return (x + 1, y);
            }

            // Left
            if (y > 0 && grid[x, y - 1] == c)
            {
                yield return (x, y - 1);
            }

            // Right
            if (y < grid.GetLength(1) - 1 && grid[x, y + 1] == c)
            {
                yield return (x, y + 1);
            }
        }

        private static char[,] ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(12, test).ToList();
            int numRows = lines.Count;
            int numCols = lines[0].Length;

            int x = -1, y = -1;

            var grid = new char[numRows, numCols];
            bool guardFound = false;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            return grid;
        }

        private enum SegmentPosition
        {
            Top,
            Right,
            Bottom,
            Left
        }
    }
}
