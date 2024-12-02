# adventofcode2024
My solutions to 2024 Advent of Code puzzles.

**Day 1:**

Part 1: As usual, first day's puzzle is trivial. Just read the numbers in the file into two lists, sort the lists, then sum up the absolute differences of the entries.

Part 2: Also easy, as usual for the first day. Read the numbers into two lists, sum up first[i] * second.Count(x => x == first[i]) for i from 0 to length(first) - 1.
