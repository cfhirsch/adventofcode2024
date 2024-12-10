using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec10PuzzlerSolver : IPuzzleSolver
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
            (int[,] grid, List<(int, int)> zeroes) = ReadMap(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            long sum = 0;

            foreach ((int, int) zero in zeroes)
            {
                sum += isPartTwo ? GetRating(grid, numRows, numCols, zero.Item1, zero.Item2) : GetScore(grid, numRows, numCols, zero.Item1, zero.Item2).Count();
            }

            return sum.ToString();
        }

        private static HashSet<(int, int)> GetScore(int[,] grid, int numRows, int numCols, int i, int j)
        {
            if (grid[i, j] == 9)
            {
                var hashset = new HashSet<(int, int)>();
                hashset.Add((i, j));
                return hashset;
            }
            else
            {
                var set = new HashSet<(int, int)>();

                IEnumerable<(int, int)> neighbors = GetNeighbors(grid, numRows, numCols, i, j);
                foreach ((int, int) neighbor in neighbors)
                {
                    set = set.Union(GetScore(grid, numRows, numCols, neighbor.Item1, neighbor.Item2)).ToHashSet();
                }

                return set;
            }
        }

        private static long GetRating(int[,] grid, int numRows, int numCols, int i, int j)
        {
            if (grid[i, j] == 9)
            {
                return 1;
            }
            else
            {
                long sum = 0;
                IEnumerable<(int, int)> neighbors = GetNeighbors(grid, numRows, numCols, i, j);
                foreach ((int, int) neighbor in neighbors)
                {
                    sum += GetRating(grid, numRows, numCols, neighbor.Item1, neighbor.Item2);
                }

                return sum;
            }
        }

        private static IEnumerable<(int, int)> GetNeighbors(int[,] grid, int numRows, int numCols, int i, int j)
        {
            if (i > 0 && grid[i - 1, j] == grid[i, j] + 1)
            {
                yield return (i - 1, j);
            }

            if (i < numRows - 1 && grid[i + 1, j] == grid[i, j] + 1)
            {
                yield return (i + 1, j);
            }

            if (j > 0 && grid[i, j - 1] == grid[i, j] + 1)
            {
                yield return (i, j - 1);
            }

            if (j < numCols - 1 && grid[i, j + 1] == grid[i, j] + 1)
            {
                yield return (i, j + 1);
            }
        }

        private static (int[,], List<(int, int)>) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(10, test).ToList();
            int numRows = lines.Count;
            int numCols = lines[0].Length;

            var zeroes = new List<(int, int)>();

            int x = -1, y = -1;

            var grid = new int[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = Int32.Parse(lines[i][j].ToString());

                    if (grid[i, j] == 0)
                    {
                        zeroes.Add((i, j));
                    }
                }
            }

            return (grid, zeroes);
        }
    }
}
