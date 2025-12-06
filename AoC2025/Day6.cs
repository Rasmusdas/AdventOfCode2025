using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace AoC2025;

[EventPipeProfiler(EventPipeProfile.CpuSampling)]
public class Day6 : Day
{
    
    [Benchmark]
    public void SolvePart1()
    {
        var numberMatrix = inputLines.Select(x => x.Split(" ",StringSplitOptions.RemoveEmptyEntries)).ToArray();
        long sum = 0;
        for (int i = 0; i < numberMatrix[0].Length; i++)
        {
            string op = numberMatrix[^1][i];
            long currSum = op == "+" ? 0 : 1;
            for (int j = 0; j < numberMatrix.Length-1; j++)
            {
                if (op == "+")
                {
                    currSum += long.Parse(numberMatrix[j][i]);
                }
                else
                {
                    currSum *= long.Parse(numberMatrix[j][i]);
                }
            }

            sum += currSum;
        }
        
#if DEBUG
        Console.WriteLine(sum);
#endif
            
    }

    [Benchmark]
    public void SolvePart2()
    {
        int start = 0;
        int end = 1;
        long sum = 0;

        while (end < inputLines[0].Length)
        {
            if (inputLines[^1][end] != ' ' || end == inputLines[0].Length - 1)
            {
                if (end == inputLines[0].Length - 1) end++;
                char op = inputLines[^1][start];

                string[] numbers = new string[end-start];
                
                int digit = 0;
                for (int i = end-1; i >= start; i--)
                {
                    for (int j = 0; j < inputLines.Length-1; j++)
                    {
                        numbers[digit] += inputLines[j][i];
                    }
                    
                    digit++;
                }
                
                long currSum = op == '+' ? 0 : 1;
                
                foreach (var number in numbers)
                {
                    if (number.Trim() == "") continue;
                    
                    if (op == '+')
                    {
                        currSum+= long.Parse(number);
                    }
                    else
                    {
                        currSum *= long.Parse(number);
                    }
                }
                
                sum += currSum;

                start = end;
            }

            end++;
        }
        
#if DEBUG
        Console.WriteLine(sum);
#endif



    }
}