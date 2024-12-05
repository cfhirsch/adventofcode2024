using adventofcode2024.Solutions;

namespace adventofcode2024.Utilities
{
    internal static class PuzzleSolverFactory
    {
        public static IPuzzleSolver GetPuzzleSolver(int day)
        {
            switch(day)
            {
                case 1:
                    return new Dec1PuzzleSolver();

                case 2:
                    return new Dec2PuzzleSolver();

                case 3:
                    return new Dec3PuzzleSolver();

                case 4:
                    return new Dec4PuzzleSolver();

                case 5:
                    return new Dec5PuzzleSolver();

                default:
                    throw new ArgumentOutOfRangeException("day");
            }
        }
    }
}
