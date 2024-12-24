using System.Reflection.Metadata.Ecma335;
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

                case 10:
                    return new Dec10PuzzlerSolver();

                case 11:
                    return new Dec11PuzzlerSolver();

                case 12:
                    return new Dec12PuzzleSolver();

                case 13:
                    return new Dec13PuzzleSolver();

                case 14:
                    return new Dec14PuzzleSolver();

                case 15:
                    return new Dec15PuzzleSolver();

                case 16:
                    return new Dec16PuzzleSolver();

                case 17:
                    return new Dec17PuzzleSolver();

                case 18:
                    return new Dec18PuzzleSolver();

                case 19:
                    return new Dec19PuzzleSolver();

                case 20:
                    return new Dec20PuzzleSolver();

                case 21:
                    return new Dec21PuzzleSolver();

                case 22:
                    return new Dec22PuzzleSolver();

                case 23:
                    return new Dec23PuzzleSolver();

                case 24:
                    return new Dec24PuzzleSolver();

                default:
                    throw new ArgumentOutOfRangeException("day");
            }
        }
    }
}
