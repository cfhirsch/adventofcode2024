using System.Text.RegularExpressions;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec24PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (Dictionary<string, bool> wires, List<LogicGate> gates) = GetWiredAndGates(test);

            while (gates.Any(g => !g.Fired))
            {
                foreach (LogicGate gate in gates.Where(g => !g.Fired))
                {
                    if (wires.ContainsKey(gate.Inputs[0]) && wires.ContainsKey(gate.Inputs[1]))
                    {
                        bool input1 = wires[gate.Inputs[0]];
                        bool input2 = wires[gate.Inputs[1]];

                        switch (gate.Operation)
                        {
                            case "AND":
                                wires[gate.Output] = input1 && input2;
                                break;

                            case "OR":
                                wires[gate.Output] = input1 || input2;
                                break;

                            case "XOR":
                                wires[gate.Output] = input1 ^ input2;
                                break;

                            default:
                                throw new ArgumentException($"Unexpected operation {gate.Operation}.");
                        }

                        gate.Fired = true;
                    }
                }
            }

            long sum = Calculate(wires, "z");
            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            return "NotSolved";
        }

        private static long Calculate(Dictionary<string, bool> wires, string prefix)
        {
            int suffix = 0;
            long sum = 0;
            int exp = 0;
            string label = $"{prefix}00";

            while (wires.ContainsKey(label))
            {
                sum += (long)Math.Pow(2, exp) * (wires[label] ? 1 : 0);
                exp++;

                suffix++;
                if (suffix < 10)
                {
                    label = $"z0{suffix}";
                }
                else
                {
                    label = $"z{suffix}";
                }
            }

            return sum;
        }

        private static (Dictionary<string, bool>, List<LogicGate>) GetWiredAndGates(bool test)
        {
            bool wireMode = true;

            var wires = new Dictionary<string, bool>();
            var gates = new List<LogicGate>();

            // x00 AND y00 -> z00
            var reg = new Regex(@"^(.+) (AND|OR|XOR) (.+) -> (.+)$", RegexOptions.Compiled);
            string label;
            foreach (string line in PuzzleReader.GetPuzzleInput(24, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    wireMode = false;
                    continue;
                }

                if (wireMode)
                {
                    string[] parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
                    label = parts[0];
                    int val = Int32.Parse(parts[1].Trim());

                    wires.Add(label, val == 1);
                }
                else
                {
                    Match match = reg.Match(line);

                    string input1 = match.Groups[1].Value;
                    string op = match.Groups[2].Value;
                    string input2 = match.Groups[3].Value;
                    string output = match.Groups[4].Value;

                    var inputs = new List<string>
                    {
                        input1,
                        input2
                    };

                    gates.Add(new LogicGate { Inputs = inputs, Operation = op, Output = output });
                }
            }

            return (wires, gates);
        }

        private class LogicGate
        {
            public LogicGate()
            {
                this.Inputs = new List<string>();
            }

            public List<string> Inputs { get; set; }

            public string Output { get; set; }

            public string Operation { get; set; }

            public bool Fired { get; set; }
        }
    }
}
