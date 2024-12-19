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
            var lines = PuzzleReader.GetPuzzleInput(19, test).ToList();

            var towels = lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(t => t.Trim()).ToList();

            List<string> patterns = lines.Skip(2).ToList();

            var memoized = new Dictionary<string, long>();

            long possible = 0;

            foreach (string pattern in patterns)
            {
                possible += CountPossible(pattern, towels, memoized);
            }

            return possible.ToString();
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

        private static long CountPossible(string pattern, List<string> towels, Dictionary<string, long> memoized)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return 1;
            }

            if (memoized.ContainsKey(pattern))
            {
                return memoized[pattern];
            }

            if (!towels.Any(t => pattern.EndsWith(t)))
            {
                memoized[pattern] = 0;
                return 0;
            }

            // Number of possible ways to create pattern p with towels = 
            // sum(number of ways to create pattern q with towels over all towels t such that p = tq).
            long sum = 0;
            foreach (string towel in towels.Where(t => pattern.StartsWith(t)).OrderByDescending(t => t.Length))
            {
                sum += CountPossible(pattern.Substring(towel.Length), towels, memoized);
            }

            memoized[pattern] = sum;
            return sum;
        }
    }
}
