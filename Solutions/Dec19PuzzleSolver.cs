using System.Text;
using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec19PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(19, test).ToList();

            string reg = ($"^({string.Join("|", lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()))})+$");

            var regex = new Regex(reg, RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

            List <string> patterns = lines.Skip(2).ToList();

            int possible = 0;

            foreach (string pattern in patterns)
            {                
                try
                {
                    if (regex.IsMatch(pattern))
                    {
                        possible++;
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    // Swallow timeout and move on.
                }
            }

            return possible.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
