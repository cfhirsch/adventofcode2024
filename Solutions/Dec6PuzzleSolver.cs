using System.Net.NetworkInformation;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec6PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (char[,] grid, (int, int) guardPos) = ReadMap(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            Direction guardDir = Direction.Up;
            var visited = new HashSet<(int, int)>();
            visited.Add(guardPos);
            while (true)
            {
                PrintMap(grid, test);

                (int, int) nextPos = GetMove(guardPos.Item1, guardPos.Item2, guardDir);
                if (!(nextPos.Item1 >= 0 && nextPos.Item1 < numRows && nextPos.Item2 >= 0 && nextPos.Item2 < numCols))
                {
                    break;
                }

                if (grid[nextPos.Item1, nextPos.Item2] == '#')
                {
                    guardDir = TurnRight(guardDir);
                }
                else
                {
                    grid[guardPos.Item1, guardPos.Item2] = '.';
                    guardPos = nextPos;
                    visited.Add(guardPos);
                }

                grid[guardPos.Item1, guardPos.Item2] = GetSymbol(guardDir);
            }

            return visited.Count().ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static (int, int) GetMove(int x, int y, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return (x - 1, y);

                case Direction.Right:
                    return (x, y + 1);

                case Direction.Down:
                    return (x + 1, y);

                case Direction.Left:
                    return (x, y - 1);

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static char GetSymbol(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return '^';

                case Direction.Right:
                    return '>';

                case Direction.Down:
                    return 'v';

                case Direction.Left:
                    return '<';

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static void PrintMap(char[,] grid, bool test)
        {
            if (test)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        Console.Write(grid[i, j]);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        private static (char[,], (int x, int y)) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(6, test).ToList();
            int numRows = lines.Count;
            int numCols = lines[0].Length;

            int x = -1, y = -1;

            var grid = new char[numRows, numCols];
            bool guardFound = false;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i,j] = lines[i][j];

                    int guardPos = lines[i].IndexOf('^');
                    if (!guardFound && guardPos > -1)
                    {
                        x = i;
                        y = guardPos;
                    }
                }
            }

            return (grid, (x, y));
        }

        private static Direction TurnRight(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Right;

                case Direction.Right:
                    return Direction.Down;

                case Direction.Down:
                    return Direction.Left;

                case Direction.Left:
                    return Direction.Up;

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }
    }

    internal enum Direction
    {
        Up,
        Left,
        Right,
        Down
    }
}
