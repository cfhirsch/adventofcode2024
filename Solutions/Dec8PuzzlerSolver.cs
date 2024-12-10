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
            return "NotSolved";
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
