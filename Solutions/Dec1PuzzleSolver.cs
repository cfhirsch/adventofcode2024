using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec1PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (List<int> first, List<int> second) = GetLists(test);

            first.Sort();
            second.Sort();

            long answer = Enumerable.Range(0, first.Count).Sum(i => Math.Abs(first[i] - second[i]));
            return answer.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            (List<int> first, List<int> second) = GetLists(test);
            long answer = Enumerable.Range(0, first.Count).Sum(i => first[i] * second.Count(x => x == first[i]));
            return answer.ToString();
        }

        private static (List<int>, List<int>) GetLists(bool test)
        {
            var first = new List<int>();
            var second = new List<int>();
            foreach (string line in PuzzleReader.GetPuzzleInput(1, test))
            {
                string[] lineParts = line.Split("   ");
                first.Add(Int32.Parse(lineParts[0]));
                second.Add(Int32.Parse(lineParts[1]));
            }

            return (first, second);
        }
    }
}
