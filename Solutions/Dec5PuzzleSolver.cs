using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec5PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (List<PrecedenceRule> pageOrderingRules, List<int[]> pageLists) = GetData(test);
            
            var valid = new List<int[]>();
            foreach (int[] pages in pageLists)
            {
                (bool isValid, _, bool corrected) = IsValid(pages, pageOrderingRules, correct: false);

                if (isValid && !corrected)
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
            (List<PrecedenceRule> pageOrderingRules, List<int[]> pageLists) = GetData(test);

            var correctedPages = new List<int[]>();
            foreach (int[] pages in pageLists)
            {
                (bool isValid, int[] result, bool corrected) = IsValid(pages, pageOrderingRules, correct: true);

                if (isValid && corrected)
                {
                    correctedPages.Add(result);
                }
            }

            long sum = 0;
            foreach (int[] pages in correctedPages)
            {
                int pos = pages.Length / 2;
                sum += pages[pos];
            }

            return sum.ToString();
        }

        private static (List<PrecedenceRule>, List<int[]>) GetData(bool test)
        {
            bool pageList = false;
            var pageOrderingRules = new List<PrecedenceRule>();
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
                    pageOrderingRules.Add(new PrecedenceRule { First = Int32.Parse(lineParts[0]), Second = Int32.Parse(lineParts[1]) });
                }
                else
                {
                    pageLists.Add(line.Split(',').Select(x => Int32.Parse(x)).ToArray());
                }
            }

            return (pageOrderingRules, pageLists);
        }

        private static (bool, int[], bool) IsValid(int[] pages, List<PrecedenceRule> pageOrderingRules, bool correct)
        {
            for (int i = 0; i < pages.Length; i++)
            {
                var rest = pages.Skip(i + 1).ToArray();
                PrecedenceRule violated = pageOrderingRules.FirstOrDefault(kvp => kvp.Second == pages[i] && rest.Contains(kvp.First));

                if (violated != null)
                {
                    if (correct)
                    {
                        return (true, Correct(pages, pageOrderingRules), true);
                        
                    }
                    else
                    {
                        return (false, pages, false);
                    }
                        
                }
            }

            return (true, pages, false);
        }

        private static int[] Correct(int[] pages, List<PrecedenceRule> precedenceRules)
        {
            bool isCorrect = false;
            int[] current = pages;
            int[] next = new int[current.Length];

            while (!isCorrect)
            {
                bool foundViolation = false;
                for (int i = 0; i < current.Length; i++)
                {
                    var rest = current.Skip(i + 1).ToArray();
                    PrecedenceRule violated = precedenceRules.FirstOrDefault(kvp => kvp.Second == current[i] && rest.Contains(kvp.First));

                    if (violated != null)
                    {
                        next = current.Take(i).
                                           Append(violated.First).
                                           Append(violated.Second).
                                           Concat(current.Skip(i + 1).Where(x => x != violated.First && x != violated.Second)).
                                           ToArray();

                        foundViolation = true;
                        break;
                    }
                }

                if (foundViolation)
                {
                    current = next;
                }
                else
                {
                    isCorrect = true;
                }
            }

            return current;
        }
    }

    internal class PrecedenceRule
    {
        public int First { get; set; }

        public int Second { get; set; }
    }
}
