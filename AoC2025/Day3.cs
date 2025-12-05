using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AoC2025
{
    public class Day3 : Day
    {
        [Benchmark]
        public void SolvePart1()
        {
            int joltageSum = 0;
            
            foreach(var batteries in inputLines)
            {
                int maxVal = 0;
                int nextMaxVal = 1;
                int pointer = 0;
                while (pointer < batteries.Length)
                {
                    if (batteries[pointer] > batteries[maxVal] && pointer < batteries.Length -1)
                    {
                        maxVal = pointer;
                        nextMaxVal = pointer+1;
                    }
                    else if(batteries[pointer] > batteries[nextMaxVal] && pointer > nextMaxVal)
                    {
                        nextMaxVal = pointer;
                    }
                    pointer++;
                }
                joltageSum += int.Parse(batteries[maxVal] + "" + batteries[nextMaxVal]);
            }
#if DEBUG
            Console.WriteLine(joltageSum);
#endif
        }


        [Benchmark]
        public void SolvePart2()
        {
            double joltageSum = 0;

            

            foreach(var batteries in inputLines)
            {
                int[] indices = new int[12];

                string joltage = "";

                for (int i = 0; i < indices.Length; i++)
                {
                    indices[i] = i;
                }

                int pointer = 0;
                while (pointer < batteries.Length)
                {
                    for (int i = 0; i < indices.Length; i++)
                    {
                        if(i > pointer || batteries.Length-pointer -1 < indices.Length - i - 1)
                        {
                            continue;
                        }

                        if (batteries[pointer] > batteries[indices[i]] && indices[i] < pointer)
                        {
                            indices[i] = pointer;
                            for (int j = 1; j < indices.Length - i; j++)
                            {
                                indices[i + j] = pointer + j;
                            }
                            break;
                        }

                    }

                    pointer++;
                }

                

                foreach(var index in indices)
                {
                    joltage += batteries[index];
                }

                joltageSum += double.Parse(joltage);
            }

#if DEBUG
            Console.WriteLine(joltageSum);
#endif
        }
    }
}
