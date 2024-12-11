using adventofcode2024.Utilities;

namespace adventofcode2024.Solutions
{
    internal class Dec9PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            string input = PuzzleReader.GetPuzzleInput(9, test).First();

            // We'll store a block as a tuple of ints. The first int will be the id of the block,
            // the second int will be the number of sectors. If the first int is -1 this is an empty block.
            bool empty = false;
            var blocks = new List<(int, int)>();

            int id = 0;

            foreach (char c in input)
            {
                if (!empty)
                {
                    blocks.Add((id, Int32.Parse(c.ToString())));
                    id++;
                }
                else
                {
                    blocks.Add((-1, Int32.Parse(c.ToString())));
                }

                empty = !empty;
            }

            // While there is more than one free block...
            while (blocks.Count(b => b.Item1 == -1) > 1)
            {
                (id, int numSectors) = blocks.Last(b => b.Item1 > -1);
                int currentPos = blocks.LastIndexOf((id, numSectors));

                (_, int numFree) = blocks.First(b => b.Item1 == -1);
                int freeIndex = blocks.IndexOf((-1, numFree));

                int blocksToMove = Math.Min(numSectors, numFree);
                blocks.Insert(freeIndex, (id, blocksToMove));
                freeIndex++;
                currentPos++;

                numFree -= blocksToMove;
                if (numFree == 0)
                {
                    blocks.RemoveAt(freeIndex);
                    currentPos--;
                }
                else
                {
                    blocks[freeIndex] = (-1, numFree);
                }

                numSectors -= blocksToMove;
                if (numSectors == 0)
                {
                    blocks[currentPos] = (-1, blocksToMove);
                }
                else 
                {
                    blocks[currentPos] = (blocks[currentPos].Item1, numSectors);
                    blocks.Add((-1, blocksToMove));
                }

                // Connect current sector to any previous empty sectors.
                int pos = blocks.Count - 1;
                while (pos > 0 && blocks[pos - 1].Item1 == -1)
                {
                    blocks[pos - 1] = (-1, blocks[pos - 1].Item2 + blocks[pos].Item2);
                    blocks.RemoveAt(pos);
                    pos--;
                }
            }

            long checkSum = 0;
            int index = 0;
            foreach ((int, int) tuple in blocks.Where(b => b.Item1 > -1))
            {
                for (int j = 1; j <= tuple.Item2; j++)
                {
                    checkSum += tuple.Item1 * index;
                    index++;
                }
            }

            return checkSum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            string input = PuzzleReader.GetPuzzleInput(9, test).First();

            // We'll store a block as a tuple of ints. The first int will be the id of the block,
            // the second int will be the number of sectors. If the first int is -1 this is an empty block.
            bool empty = false;
            var blocks = new List<(int, int)>();

            int id = 0;

            int maxId = -1;

            foreach (char c in input)
            {
                if (!empty)
                {
                    if (id > maxId)
                    {
                        maxId = id;
                    }

                    blocks.Add((id, Int32.Parse(c.ToString())));
                    id++;
                }
                else
                {
                    int numSectors = Int32.Parse(c.ToString());

                    if (numSectors > 0)
                    {
                        blocks.Add((-1, Int32.Parse(c.ToString())));
                    }
                }

                empty = !empty;
            }

            for (int i = maxId; i >= 0; i--)
            {
                (id, int numSectors) = blocks.First(b => b.Item1 == i);
                int currentPos = blocks.LastIndexOf((id, numSectors));

                (_, int numFree) = blocks.First(b => b.Item1 == -1);
                int freeIndex = blocks.IndexOf((-1, numFree));

                while (numFree < numSectors && freeIndex < currentPos)
                {
                    if (!blocks.Skip(freeIndex + 1).Any(b => b.Item1 == -1))
                    {
                        break;
                    }

                    (_, numFree) = blocks.Skip(freeIndex + 1).First(b => b.Item1 == -1);
                    freeIndex = blocks.IndexOf((-1, numFree), freeIndex + 1);
                }

                if (numFree < numSectors || freeIndex >= currentPos)
                {
                    continue;
                }

                blocks.Insert(freeIndex, (id, numSectors));
                freeIndex++;
                currentPos++;

                numFree -= numSectors;
                if (numFree == 0)
                {
                    blocks.RemoveAt(freeIndex);
                    currentPos--;
                }
                else
                {
                    blocks[freeIndex] = (-1, numFree);
                }

                blocks[currentPos] = (-1, numSectors);

                // Compact empty sectors.
                int pos = blocks.Count - 1;
                while (pos > 0)
                {
                    if (blocks[pos].Item1 == -1 && blocks[pos - 1].Item1 == -1)
                    {
                        blocks[pos - 1] = (-1, blocks[pos].Item2 + blocks[pos - 1].Item2);
                        blocks.RemoveAt(pos);
                    }
                    
                    pos--;
                }
            }

            long checkSum = 0;
            int index = 0;
            foreach ((int, int) tuple in blocks)
            {
                if (tuple.Item1 == -1)
                {
                    index += tuple.Item2;
                }
                else
                {
                    for (int j = 1; j <= tuple.Item2; j++)
                    {
                        checkSum += tuple.Item1 * index;
                        index++;
                    }
                }
            }

            return checkSum.ToString();
        }

    }
}
