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

                case 6:
                    return new Dec6PuzzleSolver();

                case 7:
                    return new Dec7PuzzleSolver();

                case 8:
                    return new Dec8PuzzlerSolver();

                case 9:
                    return new Dec9PuzzlerSolver();

                default:
                    throw new ArgumentOutOfRangeException("day");
            }
        }
    }
}
