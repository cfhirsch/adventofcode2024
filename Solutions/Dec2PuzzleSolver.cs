using System.Data;
using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec2PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(partTwo: false, test: test);
        }

        public string SolvePartTwo(bool test)
        {
            return Solve(partTwo: true, test: test);
        }

        private static string Solve(bool partTwo, bool test)
        {
            int numSafe = 0;
            foreach (string line in PuzzleReader.GetPuzzleInput(2, test))
            {
                var nums = line.Split(" ").Select(x => Int32.Parse(x)).ToList();

                (bool isSafe, int badPos) = CheckLine(nums);

                if (isSafe)
                {
                    numSafe++;
                }
                else if (partTwo)
                {
                    for (int i = 0; i <= nums.Count; i++)
                    {
                        (isSafe, _) = CheckLine(Enumerable.Range(0, nums.Count).Where(j => j != i).Select(j => nums[j]).ToList());
                        if (isSafe)
                        {
                            numSafe++;
                            break;
                        }
                    }
                }
            }

            return numSafe.ToString();
        }

        private static (bool, int) CheckLine(List<int> nums)
        {
            var sequenceState = SequenceState.Start;
            for (int i = 1; i < nums.Count; i++)
            {
                int diff = Math.Abs(nums[i] - nums[i - 1]);
                if (diff < 1 || diff > 3)
                {
                    return (false, i - 1);
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
                                return (false, i - 1);
                            }

                            break;

                        case SequenceState.Decreasing:
                            if (nums[i] > nums[i - 1])
                            {
                                return (false, i - 1);
                            }

                            break;

                        default:
                            throw new ArgumentException($"Unexpected sequenceState {sequenceState}.");
                    }
                }
            }

            return (true, -1);
        }

        private enum SequenceState
        {
            Start,
            Increasing,
            Decreasing
        }
    }
}
