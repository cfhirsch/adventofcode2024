// See https://aka.ms/new-console-template for more information
using adventofcode2024.Utilities;

bool test = false;

for (int i = 1; i <= 4; i++)
{
    IPuzzleSolver puzzleSolver = PuzzleSolverFactory.GetPuzzleSolver(i);
    Console.WriteLine("===========================================================================");
    Console.WriteLine($"Day {i}");
    Console.WriteLine($"Solution for Day {i}, Part One is {puzzleSolver.SolvePartOne(test)}.");
    Console.WriteLine($"Solution for Day {i}, Part Two is {puzzleSolver.SolvePartTwo(test)}.");
    Console.WriteLine("===========================================================================");
    Console.WriteLine();
}
