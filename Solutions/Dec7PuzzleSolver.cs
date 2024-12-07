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
            var equations = new List<Equation>();
            foreach (string line in PuzzleReader.GetPuzzleInput(7, test))
            {
                string[] parts = line.Split(':');

                var operands = new List<int>();
                foreach(Match match in numReg.Matches(parts[1]))
                {
                    operands.Add(Int32.Parse(match.Value));
                }

                equations.Add(new Equation { Result = Int64.Parse(parts[0]), Operands = operands });
            }

            long sum = 0;
            foreach (Equation equation in equations)
            {
                foreach (List<Operator> operandList in Iterate(equation.Operands))
                {
                    long result = Evaluate(equation.Operands, operandList);
                    if (result == equation.Result)
                    {
                        sum += result;
                        break;
                    }
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        internal static long Evaluate(List<int> numbers, List<Operator> operators)
        {
            long result = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
            {
                switch (operators[i - 1])
                {
                    case Operator.Plus:
                        result += numbers[i];
                        break;

                    case Operator.Times:
                        result *= numbers[i];
                        break;

                    default:
                        throw new ArgumentException($"Unexpected operator {operators[i - 1]}");
                }
            }

            return result;
        }

        private static IEnumerable<List<Operator>> Iterate(List<int> operands)
        {
            if (operands.Count == 1)
            {
                yield return new List<Operator>();
            }
            else
            {
                int val = operands.First();
                List<int> rest = operands.Skip(1).ToList();

                foreach (List<Operator> result in Iterate(rest))
                {
                    var opList = new List<Operator>();
                    opList.Add(Operator.Plus);

                    opList = opList.Concat(result).ToList();

                    yield return opList;

                    opList = new List<Operator>();
                    opList.Add(Operator.Times);

                    opList = opList.Concat(result).ToList();

                    yield return opList;
                }
            }
        }
    }

    internal class Equation
    {
        public long Result { get; set; }

        public List<int> Operands { get; set; }
    }

    internal enum Operator
    {
        Plus,
        Times
    }
}
