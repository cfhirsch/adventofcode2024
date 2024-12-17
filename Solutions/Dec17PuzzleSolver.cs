using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec17PuzzleSolver : IPuzzleSolver
    {
        private static Regex reg = new Regex(@"Register \w: (\d+)", RegexOptions.Compiled);

        public string SolvePartOne(bool test)
        {
            (int regA, int regB, int regC, List<int> progam) = ParseInput(test);

            var output = new List<int>();

            int ip = 0;
            int numerator = 0;
            int denominator = 0;
            while (ip < progam.Count)
            {
                switch (progam[ip])
                {
                    case 0: // adv
                        ip++;
                        numerator = regA;
                        denominator = (int)Math.Pow(2, GetComboOperand(progam[ip], regA, regB, regC));
                        regA = numerator / denominator;
                        ip++;
                        break;

                    case 1: // bxl
                        ip++;
                        regB ^= progam[ip];
                        ip++;
                        break;

                    case 2: // bst
                        ip++;
                        regB = GetComboOperand(progam[ip], regA, regB, regC) % 8;
                        ip++;
                        break;

                    case 3: // jnz
                        if (regA != 0)
                        {
                            ip = progam[ip + 1];
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
                        output.Add(GetComboOperand(progam[ip], regA, regB, regC) % 8);
                        ip++;
                        break;

                    case 6: //bdv
                        ip++;
                        numerator = regA;
                        denominator = (int)Math.Pow(2, GetComboOperand(progam[ip], regA, regB, regC));
                        regB = numerator / denominator;
                        ip++;
                        break;

                    case 7: // cdv
                        ip++;
                        numerator = regA;
                        denominator = (int)Math.Pow(2, GetComboOperand(progam[ip], regA, regB, regC));
                        regC = numerator / denominator;
                        ip++;
                        break;
                }
            }

            return string.Join(",", output.ToArray());
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static int GetComboOperand(int op, int regA, int regB, int regC)
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

        private static (int, int, int, List<int>) ParseInput(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(17, test).ToList();
            
            Match match = reg.Match(lines[0]);
            int regA = Int32.Parse(match.Groups[1].Value);

            match = reg.Match(lines[1]);
            int regB = Int32.Parse(match.Groups[1].Value);

            match = reg.Match(lines[2]);
            int regC = Int32.Parse(match.Groups[1].Value);

            List<int> program = lines[4].Substring("Program: ".Length).Split(",").Select(x => Int32.Parse(x)).ToList();

            return (regA, regB, regC, program);
        }
    }
}
