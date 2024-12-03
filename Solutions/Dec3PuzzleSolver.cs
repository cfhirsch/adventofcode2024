using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec3PuzzleSolver : IPuzzleSolver
    {
        private static Regex mulreg = new Regex(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);

        public string SolvePartOne(bool test)
        {
            string input = PuzzleReader.GetPuzzleInput(3, test).Aggregate((x, y) => x + y);
            MatchCollection matches = mulreg.Matches(input);
            long sum = 0;
            foreach (Match match in matches)
            {
                sum += Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
