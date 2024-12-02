// See https://aka.ms/new-console-template for more information
using adventofcode2024.Utilities;

bool test = false;

for (int i = 1; i <= 1; i++)
{
    IPuzzleSolver puzzleSolver = PuzzleSolverFactory.GetPuzzleSolver(i);
    Console.WriteLine($"Solution for Day {i}, Part One is {puzzleSolver.SolvePartOne(test)}.");
}
