using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec1PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var first = new List<int>();
            var second = new List<int>();
            foreach (string line in PuzzleReader.GetPuzzleInput(1, test))
            {
                string[] lineParts = line.Split("   ");
                first.Add(Int32.Parse(lineParts[0]));
                second.Add(Int32.Parse(lineParts[1]));
            }

            first.Sort();
            second.Sort();

            long answer = Enumerable.Range(0, first.Count).Sum(i => Math.Abs(first[i] - second[i]));
            return answer.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
