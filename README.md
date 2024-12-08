# adventofcode2024
My solutions to 2024 Advent of Code puzzles. The assumption is that puzzle input files are in C:\AdventOfCode\2024, of the format "Dec{day}Test.txt" for test 
files, "Dec{day}.txt" for the actual puzzle input.

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

Do mode: Seek up to the next appearance of "don't()" or end of string if there are no more such appearances. Use the same regex as in part 1 to find all the
multiplication operations to perform. Switch to Don't Mode.

Don't mode: Seek up to next appearance of "do()", or end of string if there are no more such appearances. Switch to Do mode.

In both the test example and my puzzle input, a "don't()" appears before a "do()", so I assumed that we always start in Do mode.

**Day 4:**

Part 1: Scanning each row and column in the input was straightforward, but it took me a bit to figure out how to get all diagonals without double counting.
For each row/column/diagonal of text, I used two regexes, one for "XMAS" and the other for "SAMX" to count all the matches.

Part 2: Part 2 was actually easier than Part 1 for me! I just setup the four possible X's as arrays, and looped through the grid looking for matches.
I initially got the bounds checking wrong, but once I fixed that I got the right answer.

**Day 5:**

Part 1: Pretty straightforward. I parsed the rules into a list of tuples. I then iterated through the page lists, and for each item in each list, checked whether there 
exists a subsequent page that is the first part of a rule that contains that item.

Part 2: This one took a little more work. I ended up refactoring my code a bit to handle both parts 1 and 2. A function common to both parts checks if a page list
is valid. If it's not valid, and we're in part 2, then the code iterates through each item in the page list, fixing violations one at a time until there are none left,
and then returns the corrected list.

**Day 6:**

Part 1: Straightforward. Read the file into a two dimensional char array, write the logic to move the guard, store every visited point into a hashset, return the number of 
items in the hashset at the end.

Part 2: Straightforward, although given that my solution took a few more seconds than I would like, there is likely an optimization that I'm missing. I simulate every possible
modification that adds a boundary where there wasn't one already, and where the guard is no currently standing. I keep a hashset of visited squares and guard directions. If
a square and guard direction has been hit before, we're in a loop. The other option is that the guard left the room.

UPDATE: I realized the only squares I need to modify are the ones that the guard visits in the unmodified grid - except for the guard's starting square. With that 
realization I was able to reduce the time to solve part 2 from around 20 seconds to a little over 8 seconds. Still a little too slow. 

I was alble to shave that down to a little over 4 seconds by replacing my original for loop with an iteration over the direction the guard is moving until I either hit
an obstacle or leave the grid.

**Dec 7:**

Part 1: Relatively straightforward if a little tedious. The hardest part was coming up with a way to iterate over all possible operand lists of length n.

Part 2: It was easy to extend my solution for part one to handle new operator. However, it takes about 9 seconds for my code to solve this part, which is a tad slower 
than I would like.

UPDATE: I optimized my solution by iterating and evaluating all possible chains of operations instead of doing them in separate loops. This shaved the time down 
to a little less than 1.5 seconds.

**Dec 8:**

Part 1: Right now I'm stumped on this one. My code gets the right answer on the test input, but the answer is too low for my puzzle input. I may end up just cribbing 
someone else's solution for this one.