# adventofcode2024
My solutions to 2024 Advent of Code puzzles.

**Day 1:**

Part 1: As usual, first day's puzzle is trivial. Just read the numbers in the file into two lists, sort the lists, then sum up the absolute differences of the entries.

Part 2: Also easy, as usual for the first day. Read the numbers into two lists, sum up first[i] * second.Count(x => x == first[i]) for i from 0 to length(first) - 1.

**Day 2:**

Part 1: Still pretty easy. Parse each line of the puzzle input into a list, for each list, compare each consecutive pair of items. If their difference is less than 1 or
greater than 3, the list is unsafe. Otherwise use an enum to keep track of whether the list starts out as increasing or decreasing. If the sequence changes direction
or the difference between consecutive items is out of bounds, the list is unsafe. Otherwise the list is safe.

Part 2: Got the wrong answer the first time; if the original line was bad, I thought I would only have to check a list with either item in the first violation removed.
But that gave me an answer that was too low. For unsafe lines, I ended up checking lists with any of the items from the original list removed, short circuiting if I found 
one that was safe. That ended up giving me the correct answer, and not affecting the runtime too much. I'll have to think about why my original assumption was incorrect.
