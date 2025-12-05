using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AoC2025
{
    public class Day4 : Day
    {

        [Benchmark]
        public void SolvePart1()
        {
            var paperRolls = inputLines;
            var validRolls = 0;
            for (int i = 0; i < paperRolls.Length; i++)
            {
                for (int j = 0; j < paperRolls[i].Length; j++)
                {
                    if(paperRolls[i][j] == '@')
                    {
                        int count = 0;
                        foreach(var (x,y) in GetNeighbours(i,j, paperRolls.Length, paperRolls[i].Length))
                        {
                            if (paperRolls[x][y] == '@')
                            {
                                count++;
                            }
                        }
                        if(count < 4)
                        {
                            validRolls++;
                        }
                    }
                }
            }
#if DEBUG
            Console.WriteLine(validRolls);
#endif
        }


        IEnumerable<(int, int)> GetNeighbours(int x, int y, int maxX, int maxY)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (x + i >= 0 && x + i < maxX && y + j >= 0 && y + j < maxY) yield return(x+i, y+j);
                }
            }
        }

        [Benchmark]
        public void SolvePart2()
        {
            var paperRolls = inputLines.Select((x) => x.ToCharArray()).ToArray();
            var validRolls = 0;
            bool removed = true;

            while(removed)
            {
                removed = false;
                for (int i = 0; i < paperRolls.Length; i++)
                {
                    for (int j = 0; j < paperRolls[i].Length; j++)
                    {
                        if (paperRolls[i][j] == '@')
                        {
                            int count = 0;
                            foreach (var (x, y) in GetNeighbours(i, j, paperRolls.Length, paperRolls[i].Length))
                            {
                                if (paperRolls[x][y] == '@')
                                {
                                    count++;
                                }
                            }
                            if (count < 4)
                            {
                                validRolls++;
                                paperRolls[i][j] = '.';
                                removed = true;
                            }
                        }
                    }
                }
            }
#if DEBUG
            Console.WriteLine(validRolls);
#endif

        }
    }
}
