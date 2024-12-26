using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec15PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test)
        {
            return "NotSolved";
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            (HashSet<(int, int)> wallLocations, 
             HashSet<Box> boxes, 
             (int, int) robotLocation,
             List<char> moves,
             int numRows, 
             int numCols)
                = ParseMap(test, isPartTwo);

            (int, int) next = (-1, -1);
            (int, int) current = robotLocation;
            foreach (char move in moves)
            {
                switch (move)
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
                }

                if (wallLocations.Contains(next))
                {
                    continue;
                }

                if (boxes.Any(b => b.Left == next || b.Right == next))
                {
                    var boxesToMove = BoxesToMove(wallLocations, boxes, next, move, numRows, isPartTwo);

                    if (!boxesToMove.Any())
                    {
                        continue;
                    }

                    boxes = boxes.Except(boxesToMove).ToHashSet();
                    foreach (Box box in boxesToMove)
                    {
                        switch (move)
                        {
                            case '^':
                                boxes.Add(new Box(
                                    box.Left.Item1 - 1, 
                                    box.Left.Item2,
                                    box.Right.Item1 - 1, 
                                    box.Right.Item2));

                                break;

                            case 'v':
                                boxes.Add(new Box(
                                    box.Left.Item1 + 1,
                                    box.Left.Item2,
                                    box.Right.Item1 + 1,
                                    box.Right.Item2));

                                break;

                            case '>':
                                boxes.Add(new Box(
                                    box.Left.Item1,
                                    box.Left.Item2 + 1,
                                    box.Right.Item1,
                                    box.Right.Item2 + 1));

                                break;

                            case '<':
                                boxes.Add(new Box(
                                    box.Left.Item1,
                                    box.Left.Item2 - 1,
                                    box.Right.Item1,
                                    box.Right.Item2 - 1));

                                break;
                        }
                    }
                }

                current = next;

                //PrintMap(test, wallLocations, boxes, current, numRows, numCols, isPartTwo);
            }

            long sum = 0;
            foreach (Box box in boxes)
            {
                sum += 100 * box.Left.Item1 + box.Left.Item2;
            }

            return sum.ToString();
        }

        // Is box b1 above box b2?
        private static bool IsAbove(Box b1, Box b2)
        {
            if (b1.Left.Item1 == b2.Left.Item1 - 1)
            {
                if (b1.Left.Item2 == b2.Left.Item2 ||
                    b1.Left.Item2 == b2.Right.Item2 ||
                    b1.Right.Item2 == b2.Left.Item2 ||
                    b1.Right.Item2 == b2.Right.Item2)
                {
                    return true;
                }
            }

            return false;
        }

        private static List<Box> BoxesToMove(
            HashSet<(int, int)> wallLocations,
            HashSet<Box> boxes,
            (int, int) startPos,
            char move,
            int numRows,
            bool isPartTwo)
        {
            (int, int) current = startPos;
            var boxesToMove = new List<Box>();
            int row = current.Item1;
            int col = current.Item2;
            Box currentBox = null;
            switch (move)
            {
                case '>':
                    currentBox = boxes.FirstOrDefault(b => b.Left == current);
                    while (currentBox != null)
                    {
                        boxesToMove.Add(currentBox);
                        current = (current.Item1, current.Item2 + (isPartTwo ? 2 : 1));
                        currentBox = boxes.FirstOrDefault(b => b.Left == current);
                    }

                    if (wallLocations.Contains(current))
                    {
                        boxesToMove.Clear();
                    }

                    break;

                case '<':
                    currentBox = boxes.FirstOrDefault(b => b.Right == current);
                    while (currentBox != null)
                    {
                        boxesToMove.Add(currentBox);
                        current = (current.Item1, current.Item2 - (isPartTwo ? 2 : 1));
                        currentBox = boxes.FirstOrDefault(b => b.Right == current);
                    }

                    if (wallLocations.Contains(current))
                    {
                        boxesToMove.Clear();
                    }

                    break;

                case '^':
                    var box1 = boxes.First(b => b.Left == current || b.Right == current);
                    boxesToMove.Add(box1);

                    row--;
                    while (row >= 0)
                    {
                        current = (row, current.Item2);
                        if (wallLocations.Contains(current))
                        {
                            boxesToMove.Clear();
                            break;
                        }

                        var boxesInRow = boxes.Where(b => b.Left.Item1 == row);
                        var boxesToAdd = boxesInRow.Where(b => boxesToMove.Any(b2 => IsAbove(b, b2)));

                        if (!boxesToAdd.Any())
                        {
                            break;
                        }

                        if (boxesToAdd.Any(b => wallLocations.Any(
                            w => w.Item1 == b.Left.Item1 - 1 &&
                                 (w.Item2 == b.Left.Item2 || w.Item2 == b.Right.Item2))))
                        {
                            boxesToMove.Clear();
                            break;
                        }

                        boxesToMove.AddRange(boxesToAdd);
                        row--;
                    }

                    break;

                case 'v':
                    var box2 = boxes.First(b => b.Left == current || b.Right == current);
                    boxesToMove.Add(box2);

                    row++;
                    while (row <= numRows - 1)
                    {
                        current = (row, current.Item2);
                        if (wallLocations.Contains(current))
                        {
                            boxesToMove.Clear();
                            break;
                        }

                        var boxesInRow = boxes.Where(b => b.Left.Item1 == row);
                        var boxesToAdd = boxesInRow.Where(b => boxesToMove.Any(b2 => IsAbove(b2, b)));

                        if (!boxesToAdd.Any())
                        {
                            break;
                        }

                        if (boxesToAdd.Any(b => wallLocations.Any(
                            w => w.Item1 == b.Left.Item1 + 1 &&
                                 (w.Item2 == b.Left.Item2 || w.Item2 == b.Right.Item2))))
                        {
                            boxesToMove.Clear();
                            break;
                        }

                        boxesToMove.AddRange(boxesToAdd);
                        row++;
                    }

                    break;
            }

            return boxesToMove;
        }

        private static void PrintMap(
            bool test, 
            HashSet<(int, int)> wallLocations,
            HashSet<Box> boxes,
            (int, int) robotPosition,
            int numRows,
            int numCols,
            bool isPartTwo)
        {
            if (!test)
            {
                return;
            }

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    (int, int) current = (i, j);

                    if (robotPosition == current)
                    {
                        Console.Write('@');
                    }
                    else if (wallLocations.Contains(current))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        if (isPartTwo)
                        {
                            if (boxes.Any(b => b.Left == current))
                            {
                                Console.Write('[');
                            }
                            else if (boxes.Any(b => b.Right == current))
                            {
                                Console.Write(']');
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }
                        else
                        {
                            if (boxes.Any(b => b.Left == current))
                            {
                                Console.Write('O');
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }
                    }
                }

                Console.WriteLine();
            }
        }

        private static (HashSet<(int, int)>, HashSet<Box>, (int, int), List<char>, int, int) ParseMap(bool test, bool isPartTwo)
        {
            var wallLocations = new HashSet<(int, int)>();
            var boxes = new HashSet<Box>();
            var robotLocation = (-1, -1);

            var lines = PuzzleReader.GetPuzzleInput(15, test).ToList();

            var mapLines = new List<string>();

            var moves = new List<char>();

            bool mapMode = true;
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
            
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (mapLines[i][j] == '#')
                    {
                        if (isPartTwo)
                        {
                            wallLocations.Add((i, 2 * j));
                            wallLocations.Add((i, 2 * j + 1));
                        
                        }
                        else
                        {
                            wallLocations.Add((i, j));
                        }
                    }

                    if (mapLines[i][j] == '@')
                    {
                        if (isPartTwo)
                        {
                            robotLocation = (i, 2 * j);
                        }
                        else
                        {
                            robotLocation = (i, j);
                        }
                    }

                    if (mapLines[i][j] == 'O')
                    {
                        if (isPartTwo)
                        {
                            boxes.Add(new Box(i, j * 2, i, j * 2 + 1));
                        }
                        else
                        {
                            boxes.Add(new Box(i, j, i, j));
                        }
                    }
                }
            }

            return (wallLocations, boxes, robotLocation, moves, numRows, isPartTwo ? numCols * 2 : numCols);
        }

        private class Box : IEquatable<Box>
        {
            public Box(int leftx, int lefty, int rightx, int righty)
            {
                this.Left = (leftx, lefty);
                this.Right = (rightx, righty);
            }

            public (int, int) Left { get; set; }

            public (int, int) Right { get; set; }

            public bool Equals(Box? other)
            {
                if (other == null)
                {
                    return false;
                }

                return this.Left == other.Left && this.Right == other.Right;

            }

            public override int GetHashCode()
            {
                return this.Left.GetHashCode() ^ this.Right.GetHashCode();
            }

            public override string ToString()
            {
                return $"({this.Left.Item1},{this.Left.Item2}),({this.Right.Item1},{this.Right.Item2})";
            }
        }
    }
}
