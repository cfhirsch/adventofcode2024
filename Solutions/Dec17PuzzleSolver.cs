using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec17PuzzleSolver : IPuzzleSolver
    {
        private static Regex reg = new Regex(@"Register \w: (\d+)", RegexOptions.Compiled);

        public string SolvePartOne(bool test)
        {
            (long regA, long regB, long regC, List<int> program) = ParseInput(test);

            return Compute(program, regA, regB, regC);
        }

        public string SolvePartTwo(bool test)
        {
            // Need to output   2,4,1,5,7,5,4,5,0,3,1,6,5,5,3,0
            (_, _, _, List<int> program) = ParseInput(test);

            string match = "2,4,1,5,7,5,4,5,0,3,1,6,5,5,3,0";
            int pos = match.Length - 1;

            var ranges = new List<(long, long)>();
            ranges.Add((0, 8));

            while (pos >= 0)
            {
                var next = new List<(long, long)>();

                string suffix = match.Substring(pos);
                foreach ((long low, long high) in ranges)
                {
                    for (long i = low; i < high; i++)
                    {
                        string output = Compute(program, i, 0, 0);

                        if (pos == 0 && output == match)
                        {
                            return i.ToString();
                        }

                        if (output.EndsWith(suffix))
                        {
                            next.Add((i * 8, (i + 1) * 8 - 1));
                        }
                    }
                }

                ranges = next;
                pos -= 2;
            }

            return "NotSolved";
        }

        private static string Compute(List<int> program, long regA, long regB, long regC)
        {
            var output = new List<long>();

            int ip = 0;
            long numerator = 0;
            long denominator = 0;
            while (ip < program.Count)
            {
                switch (program[ip])
                {
                    case 0: // adv
                        ip++;
                        numerator = regA;
                        denominator = (int)Math.Pow(2, GetComboOperand(program[ip], regA, regB, regC));
                        regA = numerator / denominator;
                        ip++;
                        break;

                    case 1: // bxl
                        ip++;
                        regB ^= program[ip];
                        ip++;
                        break;

                    case 2: // bst
                        ip++;
                        regB = GetComboOperand(program[ip], regA, regB, regC) % 8;
                        ip++;
                        break;

                    case 3: // jnz
                        if (regA != 0)
                        {
                            ip = program[ip + 1];
                        }
                        else
                        {
                            ip += 2;
                        }

                        break;

                    case 4: // bxc
                        regB = regB ^ regC;
                        ip += 2;
                        break;

                    case 5: // out
                        ip++;
                        output.Add(GetComboOperand(program[ip], regA, regB, regC) % 8);
                        ip++;
                        break;

                    case 6: //bdv
                        ip++;
                        numerator = regA;
                        denominator = (int)Math.Pow(2, GetComboOperand(program[ip], regA, regB, regC));
                        regB = numerator / denominator;
                        ip++;
                        break;

                    case 7: // cdv
                        ip++;
                        numerator = regA;
                        denominator = (long)Math.Pow(2, GetComboOperand(program[ip], regA, regB, regC));
                        regC = numerator / denominator;
                        ip++;
                        break;
                }
            }

            return string.Join(",", output.ToArray());
        }

        private static long GetComboOperand(int op, long regA, long regB, long regC)
        {
            switch(op)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return op;

                case 4:
                    return regA;

                case 5:
                    return regB;

                case 6:
                    return regC;

                default:
                    throw new ArgumentException($"Unexpected operand {op}.");
            }


        }

        private static (long, long, long, List<int>) ParseInput(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(17, test).ToList();
            
            Match match = reg.Match(lines[0]);
            long regA = Int64.Parse(match.Groups[1].Value);

            match = reg.Match(lines[1]);
            long regB = Int64.Parse(match.Groups[1].Value);

            match = reg.Match(lines[2]);
            long regC = Int64.Parse(match.Groups[1].Value);

            List<int> program = lines[4].Substring("Program: ".Length).Split(",").Select(x => Int32.Parse(x)).ToList();

            return (regA, regB, regC, program);
        }
    }
}
