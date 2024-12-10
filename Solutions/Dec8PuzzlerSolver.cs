using System.Runtime.InteropServices;
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
                        (int, int) p1 = antennae[c][k];
                        (int, int) p2 = antennae[c][l];

                        int rowDistance = p2.Item1 - p1.Item1;
                        int colDistance = p2.Item2 - p1.Item2;

                        var antinode1 = (p1.Item1 - rowDistance, p1.Item2 - colDistance);
                        var antinode2 = (p2.Item1 + rowDistance, p2.Item2 + colDistance);

                        if (antinode1.Item1 >= 0 && antinode1.Item1 < numRows &&
                            antinode1.Item2 >= 0 && antinode1.Item2 < numCols)
                        {
                            antinodeLocations.Add(antinode1);
                        }

                        if (antinode2.Item1 >= 0 && antinode2.Item1 < numRows &&
                            antinode2.Item2 >= 0 && antinode2.Item2 < numCols)
                        {
                            antinodeLocations.Add(antinode2);
                        }
                    }
                }
            } 

            return antinodeLocations.Count().ToString();
        }

        public string SolvePartTwo(bool test)
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
                        (int, int) p1 = antennae[c][k];
                        (int, int) p2 = antennae[c][l];

                        int rowDistance = p2.Item1 - p1.Item1;
                        int colDistance = p2.Item2 - p1.Item2;

                        // Starting point to search for antinodes on the other side of p1.
                        int x1 = p1.Item1 - rowDistance;
                        int y1 = p1.Item2 - colDistance;

                        // Starting point to search for antinodes on the other side of p2.
                        int x2 = p2.Item1 + rowDistance;
                        int y2 = p2.Item2 + colDistance;

                        // Starting point to search for antinodes between p1 and p2.
                        int x3 = p1.Item1;
                        int y3 = p1.Item2;

                        while (x1 >= 0 && x1 < numRows && y1 >= 0 && y1 < numCols)
                        {
                            antinodeLocations.Add((x1, y1));

                            x1 -= rowDistance;
                            y1 -= colDistance;
                        }

                        while (x2 >= 0 && x2 < numRows && y2 >= 0 && y2 < numCols)
                        {
                            antinodeLocations.Add((x2, y2));

                            x2 += rowDistance;
                            y2 += colDistance;
                        }

                        while (x3 >= 0 && x3 < numRows && y3 >= 0 && y3 < numCols)
                        {
                            antinodeLocations.Add((x3, y3));

                            x3 += rowDistance;
                            y3 += colDistance;
                        }

                    }
                }
            }

            return antinodeLocations.Count().ToString();
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
