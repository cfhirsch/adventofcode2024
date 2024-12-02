namespace adventofcode2024.Utilities
{
    internal static class PuzzleReader
    {
        public static IEnumerable<string> GetPuzzleInput(int day, bool test)
        {
            string testString = test ? "test" : string.Empty;
            string filePath = $"c:\\AdventOfCode\\2024\\Dec{day}{testString}.txt";
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}
