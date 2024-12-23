using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec21PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return "NotSolved";

            /*var paths = new Dictionary<string, int>();

            Dictionary<(char, char), List<string>> numKeyPadShortest = GetNumericKeyPadShortestPaths();
            Dictionary<(char, char), List<string>> numDirPadShortest = GetDirectionalKeyPadShortestPaths();


            foreach (string line in PuzzleReader.GetPuzzleInput(21, test))
            {
                // Get moves for numeric keypad
                List<char> numKeyPadMoves = GetKeyPadMoves(line.ToArray(), 'A', numKeyPadShortest);

                PrintMoves(numKeyPadMoves);

                // First robot direction keypad.
                List<char> dirKeyPadMoves1 = GetKeyPadMoves(numKeyPadMoves, 'A', numDirPadShortest);

                PrintMoves(dirKeyPadMoves1);

                // My direction keypad.
                List<char> mydirKeyPadMoves = GetKeyPadMoves(dirKeyPadMoves1, 'A', numDirPadShortest);

                PrintMoves(mydirKeyPadMoves);


                paths.Add(line, mydirKeyPadMoves.Count);
            }

            long result = 0;
            foreach (KeyValuePair<string, int> kvp in paths)
            {
                result += kvp.Value * CodeToNum(kvp.Key);
            }

            return result.ToString();*/
        }

        private static void PrintMoves(IEnumerable<char> moves)
        {
            foreach (char c in moves)
            {
                Console.Write(c);
            }

            Console.WriteLine();
        }


        public string SolvePartTwo(bool test)
        {
            return "NotSolved";
        }

        private static int CodeToNum(string code)
        {
            int result = 0;
            // Skip trailing zeros.
            int pos = 0;
            while (code[pos] == '0')
            {
                pos++;
            }

            while (pos < code.Length)
            {
                if (char.IsDigit(code[pos]))
                {
                    result = (10 * result) + Int32.Parse(code[pos].ToString());
                }
            }

            return result;
        }

        private static Dictionary<(char, char), IEnumerable<string>> GetNumericKeyPadShortestPaths()
        {
            var chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A' };
            return GetKeyPadShortestPaths(chars, GetNumericKeyPadNeigbors);
        }

        private static Dictionary<(char, char), IEnumerable<string>> GetDirectionalKeyPadShortestPaths()
        {
            var chars = new char[] { '^', 'v', '<', '>', 'A' };
            return GetKeyPadShortestPaths(chars, GetDirectionalKeyPadNeighbors);
        }

        private static Dictionary<(char, char), IEnumerable<string>> GetKeyPadShortestPaths(
            char[] chars,
            Func<char, IEnumerable<(char, char)>> neighborFunc)
        {
            var dict = new Dictionary<(char, char), IEnumerable<string>>();

            for (int i = 0; i < chars.Length; i++)
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    dict[(chars[i], chars[j])] = GetAllShortestPaths(chars[i], chars[j], neighborFunc, new HashSet<char>());
                }
            }

            return dict;
        }

        private static IEnumerable<string> GetAllShortestPaths(
            char c1, 
            char c2, 
            Func<char, IEnumerable<(char, char)>> neighborFunc,
            HashSet<char> visited)
        {
            if (c1 == c2)
            {
                yield return string.Empty;
                yield break;
            }

            visited.Add(c1);

            foreach ((char dir, char c) in neighborFunc(c1))
            {
                if (visited.Contains(c))
                {
                    var paths = new List<string>();
                    foreach (string subpath in GetAllShortestPaths(c, c2, neighborFunc, visited))
                    {
                        paths.Add(dir + subpath);
                    }

                    int min = paths.Min(p => p.Length);

                    foreach (string subpath in paths.Where(p => p.Length == min))
                    {
                        yield return c + subpath;
                    }
                }
            }

        }


        private static IEnumerable<(char, char)> GetNumericKeyPadNeigbors(char c)
        {
            switch (c)
            {
                case '0':
                    yield return ('^', '2');
                    yield return ('>', 'A');
                    break;

                case '1':
                    yield return ('>', '2');
                    yield return ('^', '4');
                    break;

                case '2':
                    yield return ('<', '1');
                    yield return ('^', '5');
                    yield return ('>', '3');
                    yield return ('v', '0');
                    break;

                case '3':
                    yield return ('<', '2');
                    yield return ('^', '6');
                    yield return ('v', 'A');
                    break;

                case '4':
                    yield return ('^', '7');
                    yield return ('>', '5');
                    yield return ('v', '1');
                    break;

                case '5':
                    yield return ('^', '8');
                    yield return ('>', '6');
                    yield return ('v', '2');
                    yield return ('<', '4');
                    break;

                case '6':
                    yield return ('^', '9');
                    yield return ('<', '5');
                    yield return ('v', '3');
                    break;

                case '7':
                    yield return ('>', '8');
                    yield return ('v', '4');
                    break;

                case '8':
                    yield return ('<', '7');
                    yield return ('>', '9');
                    yield return ('v', '5');
                    break;

                case '9':
                    yield return ('<', '8');
                    yield return ('v', '6');
                    break;

                case 'A':
                    yield return ('^', '3');
                    yield return ('<', '0');
                    break;

                default:
                    throw new ArgumentException($"Unexpected char {c}.");
            }
        }

        /*           +---+---+
           | ^ | A |
       +---+---+---+
       | < | v | > |
       +---+---+---+*/

        private static IEnumerable<(char, char)> GetDirectionalKeyPadNeighbors(char c)
        {
            switch (c)
            {
                case '^':
                    yield return ('>', 'A');
                    yield return ('v', 'v');
                    break;

                case 'A':
                    yield return ('<', '^');
                    yield return ('v', '>');
                    break;

                case '<':
                    yield return ('>', 'v');
                    break;

                case 'v':
                    yield return ('<', '<');
                    yield return ('^', '^');
                    yield return ('>', '>');
                    break;

                case '>':
                    yield return ('<', 'v');
                    yield return ('^', 'A');
                    break;

                default:
                    throw new ArgumentException($"Unexpected char {c}.");
            }
        }

        private class KeyPadNode
        {
            public KeyPadNode(char c)
            {
                this.Character = c;
                this.Moves = new List<char>();
            }

            public char Character { get; set; }

            public List<char> Moves { get; set; }
        }
    }
}
