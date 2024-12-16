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

UPDATE: I ended up borrowing the solution from [here](https://github.com/itsnewtjam/aoc-2024/blob/master/src/solutions/day08.ts). I was overthinking things. First off, 
the distance metric is Manhatten. If we have two antennae, p1 and p2, calculate delta_x and _delta_y between them. One antinode will be at (p1.x - delta_x, p1.y - delta_y) - 
on the side of p1 away from p2 - and (p1.x + delta_x, p2.y + delta_y) - on the side of p2 away from p1.

Part 2: Now we need to add any point that is some multiple of delta_x, delta_y on either side of p1 and p2, AND add any points that are some multiple of delta_x, delta_y 
BETWEEN p1 and p2.

**Dec 9:**

Part 1: Wrote some code to represent the puzzle input as a list of tuples, consisting of the id and number of sectors in each block (and id of -1 denotes a free space block).
Then I start at the last block with file content, and move sectors into free sectors min(file size, free space) chunks at a time. After each move I connect
the free blocks at the end of the list.

Part 2: I had to come back to this one. I would have thought this one would have been easier than Part 1, but I ended up having to do some debugging to figure out there
was a bug in my intent to only look for free blocks to the left of each file that have enough space. After I fixed that, I got the right answer.

**Dec 10:**

Part 1: Not terribly difficult. I build up a list of zero locations when reading in the puzzle input. For each zero, I do a depth first search of all paths to a 9, 
and add each 9 point found to a hsahset. The score for a zero location is the number of items in this hashset.

Part 2: Very easy to modify the solution to Part 1 to recursively add up all the paths from a 0 to a 9. That is, the number of paths from a 0 to a 9 is the sum of 
all paths from neighboring 1s to a 9, each of which is in turn the sum of all paths from neighboring 2s to a 9, etc.

**Dec 11:**

Part 1: I'm starting with naive implementation where I parse the puzzle input into a list, and then update and insert items according to the rules. I got the correct answer,
but it took over 21 seconds which is not great.

Part 2: Woop woop! Finally figured out this can be solved using recursion and memoization.
- # of stones generated by a stone with label 0 after n blinks = number of stones generated by a stone with label 1 after n - 1 blinks.
- # of stones generated by a stone with label of even length after n blinks = # stones generated by a stone with label left half + # stones generated by right half after n - 1 blinks.
- # of stones with any other label = # of stones generated by a stone of label * 2024 after n - 1 blinks.

Given this, write a recursive method to solve, using a dictionary to store any solutions we've already found. 8 ms to solve part one, 56 ms to solve part two :).

**Dec 12:**

Part 1: First, collect all the plots, where a plot is a HashSet of tuples (i, j). Go through each square in the grid, and do a breadth first search, using a visited
HashSet to keep track of squares that have been already searched. To calculate the perimeter of a plot, enumerate each square, if there is no square above the current
square in the hashset, then there's a side above that square, and similarly below and to the left and right. Aside from a bug in my initial breadth first search code This
wasn't that difficult.

Part 2: Took me a bit, but I finally came up with a solution where, as I'm counting the segments as in Part 1, I also store them and record whether they are on the top,
bottom, left or right of the plot. Then for each position (top, left, right, bottom), I order then (left to right for top or bottom segments, top to bottom for left or right segments), 
and for each one, iterate for as long as I can find a segment that connects to the current one. Increment the number of sides, move to the next segment, etc.

I tried to make my solution a little more elegant, but that doesn't give the same answer, will have to revisit.

**Dec13:**

Part 1: Definitely overthought my first stab at this with A* search, and that was taking forever. Then I realized that this looks like a linear programming problem, but with 
equality instead of <= or >= constraints. And then, duh, two equations in two unknowns. Solve with linear algebra. If the solution for the number of a presses and b presses are
both integers, there's a solution, otherwise there isn't.