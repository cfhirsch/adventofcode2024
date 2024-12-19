using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec19PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(19, test).ToList();

            var towels = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();

            List<string> patterns = lines.Skip(2).ToList();

            var memoized = new Dictionary<string, bool>();

            int possible = 0;

            foreach (string pattern in patterns)
            {
                if (IsPossible(pattern, towels, memoized))
                {
                    possible++;
                }
            }

            return possible.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static bool IsPossible(string pattern, List<string> towels, Dictionary<string, bool> memoized)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return true;
            }

            if (memoized.ContainsKey(pattern))
            {
                return memoized[pattern];
            }

            foreach (string towel in towels.Where(t => pattern.StartsWith(t)).OrderByDescending(t => t.Length))
            {
                if (IsPossible(pattern.Substring(towel.Length), towels, memoized))
                {
                    memoized[pattern] = true;
                    return true;
                }
            }

            memoized[pattern] = false;
            return false;
        }
    }
}
