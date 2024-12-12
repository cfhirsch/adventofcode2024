using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec11PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            string line = PuzzleReader.GetPuzzleInput(11, test).First();

            var stones = line.Split(' ').Select(x => Int64.Parse(x)).ToList();

            for (int i = 0; i < 25; i++)
            {
                int j = 0;
                while (j < stones.Count)
                {
                    if (stones[j] == 0)
                    {
                        stones[j] = 1;
                    }
                    else
                    {
                        string stoneLabel = stones[j].ToString();
                        if (stoneLabel.Length % 2 == 0)
                        {
                            stones.RemoveAt(j);
                            stones.Insert(j, Int64.Parse(stoneLabel.Substring(stoneLabel.Length / 2)));
                            stones.Insert(j, Int64.Parse(stoneLabel.Substring(0, stoneLabel.Length / 2)));
                            j++;
                        }
                        else
                        {
                            stones[j] *= 2024;
                        }
                    }

                    j++;
                }
            }

            return stones.Count.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            return "NotSolvedYet";
        }
    }
}
