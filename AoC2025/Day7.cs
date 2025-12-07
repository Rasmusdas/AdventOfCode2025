using BenchmarkDotNet.Attributes;

namespace AoC2025;

public class Day7 : Day
{
    private char[][] grid;
    private long[,] memory;
    
    
    [Benchmark]
    public void SolvePart1()
    {
        grid = inputLines.Select(x => x.ToCharArray()).ToArray();
        
#if DEBUG
        Console.WriteLine(ShootBeamPart1(1,grid[0].IndexOf('S')));
#endif
    }


    
    
    [Benchmark]
    public void SolvePart2()
    {
        grid = inputLines.Select(x => x.ToCharArray()).ToArray();
        memory = new long[grid.Length,grid[0].Length];
#if DEBUG
        Console.WriteLine(ShootBeamPart2(1,grid[0].IndexOf('S')));
#endif
    }

    public int ShootBeamPart1(int depth, int position)
    {
        if (position < 0 || position >= grid[0].Length || depth >= grid.Length || grid[depth][position] == '|') return 0;
        
        if (grid[depth][position] == '.')
        {
            grid[depth][position] = '|';
            return ShootBeamPart1(depth + 1, position);
        }

        if (grid[depth][position] == '^')
        {
            return ShootBeamPart1(depth, position - 1) + ShootBeamPart1(depth, position + 1) + 1;
        }
        
        
        Console.WriteLine("Invalid State Reached");

        return 0;
    }
    
    public long ShootBeamPart2(int depth, int position)
    {
        if (position < 0 || position >= grid[0].Length) return 1;

        if (depth >= grid.Length) return 1;

        if (memory[depth, position] > 0)
        {
            return memory[depth, position];
        }

        long value = 0;
        
        if (grid[depth][position] == '.')
        {
            value = ShootBeamPart2(depth + 1, position);
        }
        else if (grid[depth][position] == '^')
        {
            value = ShootBeamPart2(depth, position - 1) + ShootBeamPart2(depth, position + 1);
        }
        
        memory[depth, position] = value;
        
        return value;
    }

    public void DrawGrid()
    {
        Thread.Sleep(500);
        for (int i = 0; i < memory.GetLength(0); i++)
        {
            for (int j = 0; j < memory.GetLength(1); j++)
            {
                Console.Write(memory[i,j]);
            }

            Console.WriteLine();
        }
    }
}