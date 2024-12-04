# adventofcode2024
My solutions to 2024 Advent of Code puzzles. The assumption is that puzzle input files are in C:\AdventOfCode\2024, of the format "Dec<day>Test.txt" for test 
files, "Dec<day>.txt" for the actual puzzle input.

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

One edge case I can think of is if we have, for example, 7, 8, 7, 6, 5. The offending pair is "8, 7" but one gets a safe list if one removes the first entry.

**Day 3:**

Part 1: Easy peasy. Just load the puzzle input into a string and run a regex match for mul((\d+),(\d+)).

Part 2: A little more involved but still not difficult. Basically I divided parsing of the input string into two modes:

Do mode: Seek up to the next appearance of "don't()" or end of string if there are no more such appearances. Use the same regex as in part 1 to find all The
multiplication operations to perform. Switch to Don't Mode.

Don't mode: Seek up to next appearance of "do()", or end of string if there are no more such appearances. Switch to Do mode.

In both the test example and my puzzle input, a "don't()" appears before a "do()", so I assumed that we always start in Do mode.
