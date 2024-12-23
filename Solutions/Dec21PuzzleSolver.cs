using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec21PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var paths = new Dictionary<string, int>();

            Dictionary<(char, char), List<string>> numKeyPadShortest = GetNumericKeyPadShortestPaths();
            Dictionary<(char, char), List<string>> numDirPadShortest = GetDirectionalKeyPadShortestPaths();

            foreach (string line in PuzzleReader.GetPuzzleInput(21, test))
            {
                IEnumerable<string> numKeyPadPaths = GetAllPossibleMoveSequences(line, 'A', numKeyPadShortest);
                int min = numKeyPadPaths.Select(p => p.Length).Min();

                var dirKeyPadPaths = new List<string>();
                foreach (string numKeyPadPath in numKeyPadPaths.Where(p => p.Length == min))
                {
                    dirKeyPadPaths.AddRange(GetAllPossibleMoveSequences(numKeyPadPath, 'A', numDirPadShortest));
                }

                var dirKeyPadPaths2 = new List<string>();
                min = dirKeyPadPaths.Select(p => p.Length).Min();
                foreach (string dirKeyPadPath in dirKeyPadPaths.Where(p => p.Length == min))
                {
                    dirKeyPadPaths2.AddRange(GetAllPossibleMoveSequences(dirKeyPadPath, 'A', numDirPadShortest));
                }

                paths[line] = dirKeyPadPaths2.Select(p => p.Length).Min();
            }

            long result = 0;
            foreach (KeyValuePair<string, int> kvp in paths)
            {
                result += kvp.Value * CodeToNum(kvp.Key);
            }

            return result.ToString();
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

                pos++;
            }

            return result;
        }

        private static IEnumerable<string> GetAllPossibleMoveSequences(string code, char current, Dictionary<(char, char), List<string>> shortestPaths)
        {
            if (string.IsNullOrEmpty(code))
            {
                yield return string.Empty;
                yield break;
            }

            foreach (string path in shortestPaths[(current, code[0])])
            {
                foreach (string subpath in GetAllPossibleMoveSequences(code.Substring(1), code[0], shortestPaths))
                {
                    yield return path + "A" + subpath;
                }
            }
        }

        private static Dictionary<(char, char), List<string>> GetNumericKeyPadShortestPaths()
        {
            var chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A' };
            return GetKeyPadShortestPaths(chars, GetNumericKeyPadNeigbors);
        }

        private static Dictionary<(char, char), List<string>> GetDirectionalKeyPadShortestPaths()
        {
            var chars = new char[] { '^', 'v', '<', '>', 'A' };
            return GetKeyPadShortestPaths(chars, GetDirectionalKeyPadNeighbors);
        }

        private static Dictionary<(char, char), List<string>> GetKeyPadShortestPaths(
            char[] chars,
            Func<char, IEnumerable<(char, char)>> neighborFunc)
        {
            var dict = new Dictionary<(char, char), List<string>>();

            for (int i = 0; i < chars.Length; i++)
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    dict[(chars[i], chars[j])] = GetAllShortestPaths(chars[i], chars[j], neighborFunc).ToList();
                }
            }

            return dict;
        }

        private static IEnumerable<string> GetAllShortestPaths(
            char c1, 
            char c2,
            Func<char, IEnumerable<(char, char)>> neighborFunc)
        {
            if (c1 == c2)
            {
                yield return string.Empty;
                yield break;
            }

            var queue = new Queue<KeyPadNode>();
            queue.Enqueue(new KeyPadNode { Character = c1, Moves = string.Empty });

            var visited = new HashSet<char>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                visited.Add(current.Character);

                if (current.Character == c2)
                {
                    yield return current.Moves;
                }

                foreach ((char dir, char c) in neighborFunc(current.Character))
                {
                    if (!visited.Contains(c))
                    {
                        queue.Enqueue(new KeyPadNode { Character = c, Moves = current.Moves + dir });
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
            public char Character { get; set; }

            public string Moves { get; set; }
        }
    }
}
