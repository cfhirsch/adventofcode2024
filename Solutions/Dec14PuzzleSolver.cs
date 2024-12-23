using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec14PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test)
        {
            return Solve(test, isPartTwo: true);
        }

        private static List<Robot> GetRobots(bool test)
        {
            var robots = new List<Robot>();

            foreach (string line in PuzzleReader.GetPuzzleInput(14, test))
            {
                string[] lineParts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                
                string[] subParts = lineParts[0].Split("=");
                string[] subsubParts = subParts[1].Split(",");

                (int, int) position = (Int32.Parse(subsubParts[0]), Int32.Parse(subsubParts[1]));

                subParts = lineParts[1].Split("=");
                subsubParts = subParts[1].Split(",");

                (int, int) velocity = (Int32.Parse(subsubParts[0]), Int32.Parse(subsubParts[1]));

                robots.Add(new Robot { Position = position, Velocity = velocity });
            }

            return robots;
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            List<Robot> robots = GetRobots(test);
            int numCols = test ? 7 : 103;
            int numRows = test ? 11 : 101;

            int i = 0;
            bool overlap = true;
            while (isPartTwo ? overlap : i < 100)
            {
                foreach (Robot robot in robots)
                {
                    robot.Update(numRows, numCols);
                }

                overlap = IsOverlap(robots);
                i++;
            }

            if (!isPartTwo)
            {
                long safetyFactor = 1;

                // Top left quadrant
                safetyFactor *= robots.Count(r => r.Position.Item1 < numRows / 2 && r.Position.Item2 < numCols / 2);

                // Top right quadrant
                safetyFactor *= robots.Count(r => r.Position.Item1 < numRows / 2 && r.Position.Item2 > numCols / 2);

                // Bottom left quadrant
                safetyFactor *= robots.Count(r => r.Position.Item1 > numRows / 2 && r.Position.Item2 < numCols / 2);

                // Bottom right quadrant
                safetyFactor *= robots.Count(r => r.Position.Item1 > numRows / 2 && r.Position.Item2 > numCols / 2);

                return safetyFactor.ToString();
            }

            /*for (int k = 0; k < numCols; k++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    int count = robots.Count(x => x.Position.Item1 == k && x.Position.Item2 == j);
                    if (count == 0)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write($"{count}");
                    }
                }

                Console.WriteLine();
            }

            Console.ReadLine();*/

            return i.ToString();
        }

        private static bool IsOverlap(List<Robot> robots)
        {
            var set = new HashSet<(int, int)>();
            foreach (Robot robot in robots)
            {
                if (set.Contains(robot.Position))
                {
                    return true;
                }

                set.Add(robot.Position);
            }

            return false;
        }

        // p=0,4 v=3,-3
        private class Robot
        {
            public (int, int) Position { get; set; }

            public (int, int) Velocity { get; set; }

            public void Update(int numRows, int numCols)
            {
                int newX = this.Position.Item1 + this.Velocity.Item1;
                if (newX < 0)
                {
                    newX = numRows + newX;
                }
                
                if (newX > numRows - 1)
                {
                    newX -= numRows;
                }

                int newY = this.Position.Item2 + this.Velocity.Item2;
                if (newY < 0)
                {
                    newY = numCols + newY;
                }

                if (newY > numCols - 1)
                {
                    newY -= numCols;
                }

                this.Position = (newX, newY);
            }
        }
    }
}
