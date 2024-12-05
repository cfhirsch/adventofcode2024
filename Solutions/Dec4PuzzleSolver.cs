using System.Text;
using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec4PuzzleSolver : IPuzzleSolver
    {
        private static Regex xmas = new Regex("XMAS", RegexOptions.Compiled);
        private static Regex samx = new Regex("SAMX", RegexOptions.Compiled);

        public string SolvePartOne(bool test)
        {
            char[,] grid = ReadGrid(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            int numHits = 0;

            string stringToSearch;
            // Look horizontally along each row.
            for (int i = 0; i < numRows; i++)
            {
                stringToSearch = GetString(grid, i, 0, numRows, numCols, Direction.Horizontal);
                numHits += GetWordCount(stringToSearch);
            }

            // Look vertically along each column.
            for (int j = 0; j < numRows; j++)
            {
                stringToSearch = GetString(grid, 0, j, numRows, numCols, Direction.Vertical);
                numHits += GetWordCount(stringToSearch);
            }

            // Look along all diagonals from top left to bottom right,
            // starting at cells along the leftmost column.
            for (int i = numRows - 1; i >= 1; i--)
            {
                stringToSearch = GetString(grid, i, 0, numRows, numCols, Direction.DiagonalRight);
                numHits += GetWordCount(stringToSearch);
            }

            // Look along all diagonals from top left to bottom right,
            // and from top right to bottom left, starting at cells along the first row.
            for (int j = 0; j < numCols; j++)
            {
                stringToSearch = GetString(grid, 0, j, numRows, numCols, Direction.DiagonalRight);
                numHits += GetWordCount(stringToSearch);

                stringToSearch = GetString(grid, 0, j, numRows, numCols, Direction.DiagonalLeft);
                numHits += GetWordCount(stringToSearch);
            }

            // Look along all diagonals from top left to bottom right,
            // starting at the rightmost column.
            for (int i = numRows - 1; i >= 1; i--)
            {
                stringToSearch = GetString(grid, i, numCols - 1, numRows, numCols, Direction.DiagonalLeft);
                numHits += GetWordCount(stringToSearch);
            }

            return numHits.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            char[,] grid = ReadGrid(test);
            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var pat1 = new char[,] { { 'M', '.', 'S' }, { '.', 'A', '.' }, { 'M', '.', 'S' } };
            var pat2 = new char[,] { { 'S', '.', 'S' }, { '.', 'A', '.' }, { 'M', '.', 'M' } };
            var pat3 = new char[,] { { 'M', '.', 'M' }, { '.', 'A', '.' }, { 'S', '.', 'S' } };
            var pat4 = new char[,] { { 'S', '.', 'M' }, { '.', 'A', '.' }, { 'S', '.', 'M' } };

            int numHits = 0;
            for (int i = 0; i < numRows - 2; i++)
            {
                for (int j = 0; j < numCols - 2; j++)
                {
                    numHits += FindPattern(grid, i, j, pat1);
                    numHits += FindPattern(grid, i, j, pat2);
                    numHits += FindPattern(grid, i, j, pat3);
                    numHits += FindPattern(grid, i, j, pat4);
                }
            }

            return numHits.ToString();
        }

        public static int FindPattern(char[,] grid, int startI, int startJ, char[,] pattern)
        {
            for (int i = 0; i <= 2; i++)
            {
                
                for (int j = 0; j <= 2; j++)
                {
                    if (pattern[i, j] == '.')
                    {
                        continue;
                    }

                    if (pattern[i, j] != grid[startI + i, startJ + j])
                    {
                        return 0;
                    }
                }
            }

            return 1;
        }

        private static char[,] ReadGrid(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(4, test).ToList();

            int numRows = lines.Count;
            int numCols = lines[0].Length;
            var grid = new char[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            return grid;
        }

        private static string GetString(char[,] grid, int startI, int startJ, int numRows, int numCols, Direction dir)
        {
            int i = startI;
            int j = startJ;
            var sb = new StringBuilder();
            while (i >= 0 && i < numRows && j >= 0 && j < numCols)
            {
                sb.Append(grid[i, j]);
                
                (i, j) = GetNextCoord(i, j, numRows, numCols, dir);
            }

            return sb.ToString();
        }

        private static (int, int) GetNextCoord(int i, int j, int numRows, int numCols, Direction dir)
        {
            switch(dir)
            {
                case Direction.Horizontal:
                    return (i, j + 1);

                case Direction.Vertical:
                    return (i + 1, j);

                case Direction.DiagonalRight:
                    return (i + 1, j + 1);
                    
                case Direction.DiagonalLeft:
                    return (i + 1, j - 1);
                    
                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static int GetWordCount(string word)
        {
            if (word.Length < 4)
            {
                return 0;
            }

            return xmas.Matches(word).Count() + samx.Matches(word).Count();
        }

        private enum Direction
        {
            Horizontal,
            Vertical,
            DiagonalLeft,
            DiagonalRight
        }
    }   
}
