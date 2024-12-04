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
            string input = PuzzleReader.GetPuzzleInput(3, test).Aggregate((x, y) => x + y);
            int pos = 0;
            long sum = 0;

            bool dontMode = false;

            while (pos < input.Length)
            {
                string next;
                if (!dontMode)
                {
                    int dontPos = input.IndexOf("don't()", pos);
                    dontMode = dontPos > -1;

                    next = (dontPos > -1) ? input.Substring(pos, dontPos - pos) : input.Substring(pos);
                    pos = (dontPos > -1) ? dontPos + "don't()".Length : input.Length;

                    MatchCollection matches = mulreg.Matches(next);
                    foreach (Match match in matches)
                    {
                        sum += Int32.Parse(match.Groups[1].Value) * Int32.Parse(match.Groups[2].Value);
                    }
                }
                else
                {
                    int doPos = input.IndexOf("do()", pos);
                    pos = (doPos > -1) ? doPos + "do()".Length : input.Length;
                    dontMode = false;
                }
            }

            return sum.ToString();
        }
    }
}
