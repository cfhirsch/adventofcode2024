using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec5PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            bool pageList = false;

            var pageOrderingRules = new List<(int, int)>();
            var pageLists = new List<int[]>();

            foreach (string line in PuzzleReader.GetPuzzleInput(5, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    pageList = true;
                    continue;
                }

                if (!pageList)
                {
                    string[] lineParts = line.Split('|');
                    pageOrderingRules.Add((Int32.Parse(lineParts[0]), Int32.Parse(lineParts[1])));
                }
                else
                {
                    pageLists.Add(line.Split(',').Select(x => Int32.Parse(x)).ToArray());
                }
            }

            var valid = new List<int[]>();
            foreach (int[] pages in pageLists)
            {
                bool validOrdering = true;
                for (int i = 0; i < pages.Length; i++)
                {
                    if (pages.Skip(i + 1).Any(p => pageOrderingRules.Any(kvp => kvp.Item2 == pages[i] && kvp.Item1 == p)))
                    {
                        validOrdering = false;
                        break;
                    }
                }

                if (validOrdering)
                {
                    valid.Add(pages);
                }
            }

            long sum = 0;
            foreach (int[] pages in valid)
            {
                int pos = pages.Length / 2;
                sum += pages[pos];
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }


    }
}
