using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec8PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (char[,] grid, Dictionary<char, List<(int, int)>> antennae) = ReadMap(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var antinodeLocations = new HashSet<(int, int)>();
            foreach (char c in antennae.Keys)
            {
                for (int k = 0; k < antennae[c].Count - 1; k++)
                {
                    for (int l = k + 1; l < antennae[c].Count; l++)
                    {
                        // We need to look at all the points on the grid that are colinear with
                        // both antennae.
                        (int, int) p1 = antennae[c][k];
                        (int, int) p2 = antennae[c][l];

                        double a = (p2.Item2 - p1.Item2) / (1.0 * (p2.Item1 - p1.Item1));
                        double b = p1.Item2 - a * p1.Item1;

                        for (int row = 0; row < numRows; row++)
                        {
                            (int, int) p3 = (row, (int)(a * row + b));

                            if (p3.Item2 < 0 || p3.Item2 > numCols - 1)
                            {
                                continue;
                            }

                            double d1 = Distance(p3.Item1, p3.Item2, p1.Item1, p1.Item2);
                            double d2 = Distance(p3.Item1, p3.Item2, p2.Item1, p2.Item2);

                            if (d1 == 2 * d2 || d2 == 2 * d1)
                            {
                                antinodeLocations.Add((p3.Item1, p3.Item2));
                            }
                        }
                    }
                }
            } 

            return antinodeLocations.Count().ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1)*(x2 - x1) + (y2 - y1)*(y2 - y1));
        }

        private static int Manhattan(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }

        private static (char[,], Dictionary<char, List<(int, int)>>) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(8, test).ToList();
            int numRows = lines.Count;
            int numCols = lines[0].Length;

            var antennae = new Dictionary<char, List<(int, int)>>();

            var grid = new char[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = lines[i][j];

                    char c = grid[i, j];
                    if (c != '.')
                    {
                        if (!antennae.ContainsKey(c))
                        {
                            antennae[c] = new List<(int, int)>();
                        }

                        antennae[c].Add((i, j));
                    }
                }
            }

            return (grid, antennae);
        }
    }
}
