using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec25PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (List<List<int>> locks, List<List<int>> keys) = GetData(test);

            int numMatches = 0;
            foreach (List<int> l in locks)
            {
                foreach (List<int> key in keys)
                {
                    if (Enumerable.Range(0, 5).All(i => l[i] + key[i] < 6))
                    {
                        numMatches++;
                    }
                }
            }

            return numMatches.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            return "Merry Christmas!";
        }

        private static (List<List<int>>, List<List<int>>) GetData(bool test)
        {
            var locks = new List<List<int>>();
            var keys = new List<List<int>>();

            bool first = true;
            bool isLock = false;
            List<int> temp = new List<int>();
            foreach (string line in PuzzleReader.GetPuzzleInput(25, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    first = true;

                    if (isLock)
                    {
                        locks.Add(temp);
                    }
                    else
                    {
                        keys.Add(temp);
                    }

                    temp = Enumerable.Repeat(0, 5).ToList();
                    continue;
                }

                if (first)
                {
                    isLock = line.Contains('#');
                    if (isLock)
                    {
                        temp = Enumerable.Repeat(0, 5).ToList();
                    }
                    else
                    {
                        temp = Enumerable.Repeat(5, 5).ToList();
                    }

                    first = false;
                }
                else
                {
                    for (int j = 0; j < line.Length; j++)
                    {
                        if (isLock && line[j] == '#')
                        {
                            temp[j]++;
                        }

                        if (!isLock && line[j] == '.')
                        {
                            temp[j]--;
                        }
                    }
                }
            }

            if (isLock)
            {
                locks.Add(temp);
            }
            else
            {
                keys.Add(temp);
            }

            return (locks, keys);
        }
    }
}
