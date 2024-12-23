using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec22PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            long sum = 0;
            foreach (string line in PuzzleReader.GetPuzzleInput(22, test))
            {
                long secret = Int64.Parse(line);
                for (int i = 0; i < 2000; i++)
                {
                    long intermediate = secret * 64;
                    secret = Mix(intermediate, secret);
                    secret = Prune(secret);
                    intermediate = secret / 32;
                    secret = Mix(intermediate, secret);
                    Prune(secret);
                    intermediate = secret * 2048;
                    secret = Mix(intermediate, secret);
                    secret = Prune(secret);
                }

                sum += secret;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static long Mix(long val, long secret)
        {
            return val ^ secret;
        }

        private static long Prune(long secret)
        {
            return secret % 16777216;
        }
    }
}
