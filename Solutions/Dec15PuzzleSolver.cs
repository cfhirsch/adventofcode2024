using System.Runtime.CompilerServices;
using System.Windows.Markup;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec15PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (char[,] grid, (int, int) robotPos, IEnumerable<char> moves) = ReadMap(test);
            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            foreach (char move in moves)
            {
                var movesToMake = new List<((int, int),(int, int))>();
                bool cont = true;

                (int, int) current = robotPos;

                foreach ((int, int) next in GetNext(robotPos, move, numRows, numCols))
                {
                    if (!cont)
                    {
                        break;
                    }

                    switch (grid[next.Item1, next.Item2])
                    {
                        case '.':
                            cont = false;
                            movesToMake.Add((current, next));
                            break;

                        case '#':
                            cont = false;
                            movesToMake.Clear();
                            break;

                        case 'O':
                            movesToMake.Add((current, next));
                            break;

                        default:
                            throw new ArgumentException($"Unexpected char {grid[next.Item1, next.Item2]}");
                    }

                    current = next;
                }
                
                for (int i = movesToMake.Count - 1; i >= 0; i--)
                {
                    grid[movesToMake[i].Item2.Item1, movesToMake[i].Item2.Item2] = grid[movesToMake[i].Item1.Item1, movesToMake[i].Item1.Item2];
                }

                if (movesToMake.Count > 0)
                {
                    grid[movesToMake[0].Item1.Item1, movesToMake[0].Item1.Item2] = '.';
                    robotPos = movesToMake[0].Item2;
                }

                PrintMap(test, grid);
            }

            long sum = 0;

            // The GPS coordinate of a box is equal to 100 times its distance from the top edge of the map
            // plus its distance from the left edge of the map.
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (grid[i, j] == 'O')
                    {
                        sum += (100 * i) + j;
                    }
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            return "NotSolved";

            (char[,] grid, (int, int) robotPos, IEnumerable<char> moves) = ReadMap(test);

            (grid, robotPos) = Expand(grid);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            foreach (char move in moves)
            {
                var movesToMake = new List<((int, int), (int, int))>();
                bool cont = true;

                (int, int) current = robotPos;

                foreach ((int, int) next in GetNext(robotPos, move, numRows, numCols))
                {
                    if (!cont)
                    {
                        break;
                    }

                    switch (grid[next.Item1, next.Item2])
                    {
                        case '.':
                            cont = false;
                            movesToMake.Add((current, next));
                            break;

                        case '#':
                            cont = false;
                            movesToMake.Clear();
                            break;

                        case '[':
                        case ']':
                            if (move == '>' || move == '<')
                            {
                                movesToMake.Add((current, next));
                            }
                            else
                            {
                                List<((int, int), (int, int))> finalMoves = GetBoxMoves(current, move, grid);
                                cont = false;
                                if (!finalMoves.Any())
                                {
                                    movesToMake.Clear();
                                }
                                else
                                {
                                    movesToMake.AddRange(finalMoves);
                                }

                                break;
                            }

                            break;

                        default:
                            throw new ArgumentException($"Unexpected char {grid[next.Item1, next.Item2]}");
                    }

                    current = next;
                }

                for (int i = movesToMake.Count - 1; i >= 0; i--)
                {
                    grid[movesToMake[i].Item2.Item1, movesToMake[i].Item2.Item2] = grid[movesToMake[i].Item1.Item1, movesToMake[i].Item1.Item2];
                }

                if (movesToMake.Count > 0)
                {
                    grid[movesToMake[0].Item1.Item1, movesToMake[0].Item1.Item2] = '.';
                    robotPos = movesToMake[0].Item2;
                }

                // HACK:
                if (grid[robotPos.Item1, robotPos.Item2 + 1] == ']')
                {
                    grid[robotPos.Item1, robotPos.Item2 + 1] = '.';
                }

                if (grid[robotPos.Item1, robotPos.Item2 - 1] == '[')
                {
                    grid[robotPos.Item1, robotPos.Item2 - 1] = '.';
                }

                PrintMap(test, grid);
            }

            long sum = 0;

            // The GPS coordinate of a box is equal to 100 times its distance from the top edge of the map
            // plus its distance from the left edge of the map.
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (grid[i, j] == 'O')
                    {
                        sum += (100 * i) + j;
                    }
                }
            }

            return sum.ToString();
        }

        private static List<((int, int), (int, int))> GetBoxMoves((int, int) current, char move, char[,] grid)
        {
            if (move != '^' && move != 'v')
            {
                throw new ArgumentException("Move must be ^ or v.");
            }

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            (int x, int y) = current;

            var queue = new Queue<(int, int)>();
            queue.Enqueue((move == '^' ? x - 1 : x + 1, y));

            var visited = new HashSet<(int, int)>();

            var moves = new List<((int, int), (int, int))>();

            bool cont = true;
            while (queue.Count > 0 && cont)
            {
                current = queue.Dequeue();
                (x, y) = current;

                visited.Add(current);

                if (x < 0 || x >= numRows - 1)
                {
                    moves = new List<((int, int), (int, int))>();
                    cont = false;
                    break;
                }

                int prevRow = move == '^' ? x + 1 : x - 1;
                int prevCol = y;

                if (grid[prevRow, y - 1] != '@' && grid[prevRow, y + 1] != '@')
                {
                    (int, int) previous = (prevRow, prevCol);
                    moves.Add((previous, current));
                }

                (int, int) next1, next2;

                switch (grid[x, y])
                {
                    case '#':
                        moves = new List<((int, int), (int, int))>();
                        cont = false;
                    break;

                    case '.':
                        break;

                    case '[':
                        // Enqueue the square above/below the current one,
                        // and the one to the right, which contains the other 
                        // side of the box.

                        next1 = (move == '^' ? x - 1 : x + 1, y);
                        next2 = (x, y + 1);

                        if (!visited.Contains(next1))
                        { 
                            queue.Enqueue(next1);
                        }

                        if (!visited.Contains(next2))
                        {
                            queue.Enqueue(next2);
                        }

                        break;

                    case ']':
                        // Enqueue the square above/below the current one,
                        // and the one to the left, which contains the other 
                        // side of the box.

                        next1 = (move == '^' ? x - 1 : x + 1, y);
                        next2 = (x, y - 1);

                        if (!visited.Contains(next1))
                        {
                            queue.Enqueue(next1);
                        }

                        if (!visited.Contains(next2))
                        {
                            queue.Enqueue(next2);
                        }

                        break;

                    default:
                        throw new ArgumentException($"Unexpected value {grid[x, y]}.");
                }
            }

            return moves;
        }
        private static void PrintMap(bool test, char[,] grid)
        {
            if (!test)
            {
                return;
            }

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j]);
                }

                Console.WriteLine();
            }
        }

        private static (char[,], (int, int)) Expand(char[,] grid)
        {
            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            var expanded = new char[numRows, numCols * 2];

            int robotX = -1, robotY = -1;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    switch(grid[i, j])
                    {
                        case '#':
                            expanded[i, 2 * j] = expanded[i, 2 * j + 1] = '#';
                            break;

                        case 'O':
                            expanded[i, 2 * j] = '[';
                            expanded[i, 2 * j + 1] = ']';
                            break;

                        case '.':
                            expanded[i, 2 * j] = expanded[i, 2 * j + 1] = '.';
                            break;

                        case '@':
                            expanded[i, 2 * j] = '@';
                            expanded[i, 2 * j + 1] = '.';
                            robotX = i;
                            robotY = 2 * j;
                            break;

                        default:
                            throw new ArgumentException($"Unexpected value {grid[i, j]}");
                    }
                }
            }

            return (expanded, (robotX, robotY));
        }

        private static IEnumerable<(int, int)> GetNext((int, int) current, char dir, int numRows, int numCols)
        {
            (int, int) next = (-1, -1);

            while (true)
            {
                switch (dir)
                {
                    case '^':
                        next = (current.Item1 - 1, current.Item2);
                        break;

                    case '>':
                        next = (current.Item1, current.Item2 + 1);
                        break;

                    case 'v':
                        next = (current.Item1 + 1, current.Item2);
                        break;

                    case '<':
                        next = (current.Item1, current.Item2 - 1);
                        break;

                    default:
                        throw new ArgumentException($"Unexpected direction {dir}.");
                }

                if (next.Item1 < 0 || next.Item1 > numRows - 1 || next.Item2 < 0 || next.Item2 > numCols - 1)
                {
                    break;
                }

                yield return next;

                current = next;
            }
        }

        private static (char[,], (int, int), IEnumerable<char> moves) ReadMap(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(15, test).ToList();
            
            var mapLines = new List<string>();

            var moves = new List<char>();

            bool mapMode = true;
            (int, int) robotPos = (-1, -1);
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    mapMode = false;
                    continue;
                }

                if (mapMode)
                {
                    mapLines.Add(line);
                }
                else
                {
                    foreach (char c in line)
                    {
                        if (c != '\r' && c != '\n')
                        {
                            moves.Add(c);
                        }
                    }
                }
            }

            int numRows = mapLines.Count;
            int numCols = mapLines[0].Length;
            var grid = new char[numRows, numCols];
            
            for (int i = 0; i < numRows;  i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    grid[i, j] = mapLines[i][j];
                    if (grid[i, j] == '@')
                    {
                        robotPos = (i, j);
                    }
                }
            }

            return (grid, robotPos, moves);
        }
    }
}
