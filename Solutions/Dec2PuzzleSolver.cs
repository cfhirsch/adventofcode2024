using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec2PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            int numSafe = 0;
            foreach (string line in PuzzleReader.GetPuzzleInput(2, test))
            {
                var nums = line.Split(" ").Select(x => Int32.Parse(x)).ToArray();
                var sequenceState = SequenceState.Start;
                bool safe = true;
                for (int i = 1; i < nums.Length; i++)
                {
                    int diff = Math.Abs(nums[i] - nums[i - 1]);
                    if (diff < 1 || diff > 3)
                    {
                        safe = false;
                    }
                    else
                    {
                        switch (sequenceState)
                        {
                            case SequenceState.Start:
                                sequenceState = nums[i] > nums[i - 1] ? SequenceState.Increasing : SequenceState.Decreasing;
                                break;

                            case SequenceState.Increasing:
                                if (nums[i] < nums[i - 1])
                                {
                                    safe = false;
                                }

                                break;

                            case SequenceState.Decreasing:
                                if (nums[i] > nums[i - 1])
                                {
                                    safe = false;
                                }

                                break;

                            default:
                                throw new ArgumentException($"Unexpected sequenceState {sequenceState}.");
                        }
                    }

                    if (!safe)
                    {
                        break;
                    }
                }

                if (safe)
                {
                    numSafe++;
                }
            }

            return numSafe.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private enum SequenceState
        {
            Start,
            Increasing,
            Decreasing
        }
    }
}
