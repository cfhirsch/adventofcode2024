using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec13PuzzleSolver : IPuzzleSolver
    {
        private static Regex buttonReg = new Regex(@"Button (A|B): X\+(\d+), Y\+(\d+)", RegexOptions.Compiled);
        private static Regex prizeReg = new Regex(@"Prize: X=(\d+), Y=(\d+)", RegexOptions.Compiled);

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
            List<ClawMachine> machines = ParseInput(test);

            long totalTokens = 0;
            foreach (ClawMachine machine in machines)
            {
                (long numTokens, bool foundPrize) = FindPrize(machine, isPartTwo);
                if (foundPrize)
                {
                    totalTokens += numTokens;
                }
            }

            return totalTokens.ToString();
        }

        private static (long, bool) FindPrize(ClawMachine machine, bool isPartTwo)
        {
            long prizeX = machine.Prize.Item1, prizeY = machine.Prize.Item2;

            if (isPartTwo)
            {
                prizeX += 10000000000000;
                prizeY += 10000000000000;
            }

            long detDenom = machine.ButtonA.Item1 * machine.ButtonB.Item2 - machine.ButtonA.Item2 * machine.ButtonB.Item1;

            long sumA = machine.ButtonB.Item2 * prizeX - machine.ButtonB.Item1 * prizeY;
            long sumB = -1 * machine.ButtonA.Item2 * prizeX + machine.ButtonA.Item1 * prizeY;

            double numA = sumA / (1.0 * detDenom);
            double numB = sumB / (1.0 * detDenom);

            bool hasSolution = numA == (long)numA && numB == (long)numB;
            long cost = (long)numA * 3 + (long)numB;

            return (cost, hasSolution);
        }

        private static List<ClawMachine> ParseInput(bool test)
        {
            var machines = new List<ClawMachine>();
            var lines = PuzzleReader.GetPuzzleInput(13, test).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    i++;
                    continue;
                }

                Match match = buttonReg.Match(line);

                (int, int) buttonA = (Int32.Parse(match.Groups[2].Value), Int32.Parse(match.Groups[3].Value));

                i++;
                line = lines[i];
                match = buttonReg.Match(line);
                (int, int) buttonB = (Int32.Parse(match.Groups[2].Value), Int32.Parse(match.Groups[3].Value));

                i++;
                line = lines[i];
                match = prizeReg.Match(line);
                (int, int) prize = (Int32.Parse(match.Groups[1].Value), Int32.Parse(match.Groups[2].Value));

                machines.Add(new ClawMachine { ButtonA = buttonA, ButtonB = buttonB, Prize = prize });
                i++;
            }

            return machines;
        }

        private class ClawMachine
        {
            public (int, int) ButtonA { get; set; }
            public (int, int) ButtonB { get; set; }

            public (int, int) Prize { get; set; }
        }
    }
}
