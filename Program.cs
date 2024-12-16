// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using adventofcode2024.Utilities;

bool test = false;

var watch = new Stopwatch();
for (int i = 1; i <= 13; i++)
{
    IPuzzleSolver puzzleSolver = PuzzleSolverFactory.GetPuzzleSolver(i);
    Console.WriteLine("===========================================================================");
    Console.WriteLine($"Day {i}");

    watch.Reset();
    watch.Start();
    string result = puzzleSolver.SolvePartOne(test);
    watch.Stop();
    Console.WriteLine($"Solution for Day {i}, Part One took {watch.ElapsedMilliseconds} ms to find, is {result}.");

    watch.Reset();
    watch.Start();
    result = puzzleSolver.SolvePartTwo(test);
    watch.Stop();

    Console.WriteLine($"Solution for Day {i}, Part Two took {watch.ElapsedMilliseconds} ms to find, is {result}.");
    Console.WriteLine("===========================================================================");
    Console.WriteLine();
}
