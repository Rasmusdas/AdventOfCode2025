namespace AoC2025;

public class Day12 : Day
{
    public void SolvePart1()
    {
        var index = inputLines.LastIndexOf("");
        List<Present> presents = new();
        for (int i = 0; i < index; i+=5)
        {
            int elements = inputLines[i + 1].Replace(".", "").Length;
            elements += inputLines[i + 2].Replace(".", "").Length;
            elements += inputLines[i + 3].Replace(".", "").Length;

            presents.Add(new Present(elements, 3,
                inputLines[i + 1] + "\n" + inputLines[i + 2] + "\n" + inputLines[i + 3]));
        }

        List<Grid> grids = new();
        
        for (int i = index+1; i < inputLines.Length; i++)
        {
            var descriptors = inputLines[i].Split(" ");

            var sizeDescriptors = descriptors[0].Replace(":", "").Split("x");
            
            int x = int.Parse(sizeDescriptors[0]);
            int y = int.Parse(sizeDescriptors[1]);

            var presentDescriptors = descriptors.AsSpan(1);
            List<int> presentCounts = new();
            foreach (var presentDescriptor in presentDescriptors)
            {
                presentCounts.Add(int.Parse(presentDescriptor));
            }
            
            Grid g =  new Grid(x, y, presentCounts); 
            
            grids.Add(g);
        }
        
        int canAlwaysFit = 0;
        int canNeverFit = 0;
        int canMaybeFit = 0;


        foreach (var grid in grids)
        {
            var maxSize = grid.X * grid.Y;
            var maxSlots = (grid.X / 3) * (grid.Y / 3);
            
            int elementSum = grid.Presents.Index().Select((x) => presents[x.Index].Elements * x.Item).Sum();
            int elementSlots = grid.Presents.Sum();

            if (maxSlots >= elementSlots)
            {
                canAlwaysFit++;
            }
            else if (maxSize < elementSum)
            {
                canNeverFit++;
            }
            else
            {
                canMaybeFit++;
            }
        }
        
        Console.WriteLine(canMaybeFit);
        Console.WriteLine(canAlwaysFit);
        Console.WriteLine(canNeverFit);
    }
}

public struct Present(int elements, int size, string format)
{
    public int Elements = elements;
    public int Size = size;
    public string Format = format;

    public override string ToString()
    {
        return $"{nameof(Elements)}: {Elements}, {nameof(Size)}: {Size}, \n{nameof(Format)}:\n{Format}";
    }
}

public struct Grid(int x, int y, List<int> presents)
{
    public int X = x;
    public int Y = y;
    public List<int> Presents = presents;
}