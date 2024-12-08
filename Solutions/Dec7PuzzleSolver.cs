using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec7PuzzleSolver : IPuzzleSolver
    {
        private static Regex numReg = new Regex(@"(\d+)", RegexOptions.Compiled);

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
            var equations = new List<Equation>();
            foreach (string line in PuzzleReader.GetPuzzleInput(7, test))
            {
                string[] parts = line.Split(':');

                var operands = new List<int>();
                foreach (Match match in numReg.Matches(parts[1]))
                {
                    operands.Add(Int32.Parse(match.Value));
                }

                equations.Add(new Equation { Result = Int64.Parse(parts[0]), Operands = operands });
            }

            long sum = 0;
            foreach (Equation equation in equations)
            {
                foreach (long result in Evaluate(equation.Operands, isPartTwo))
                {
                    if (result == equation.Result)
                    {
                        sum += result;
                        break;
                    }
                }
            }

            return sum.ToString();
        }

        internal static IEnumerable<long> Evaluate(IEnumerable<int> numbers, bool isPartTwo)
        {
            if (numbers.Count() == 1)
            {
                yield return numbers.First();
            }
            else
            {
                int last = numbers.Last();
                foreach (long result in Evaluate(numbers.Take(numbers.Count() - 1), isPartTwo))
                {
                    yield return result + last;

                    yield return result * last;

                    if (isPartTwo)
                    {
                        yield return Int64.Parse($"{result}{last}");
                    }
                }
            }
        }
    }

    internal class Equation
    {
        public long Result { get; set; }

        public List<int> Operands { get; set; }
    }
}
