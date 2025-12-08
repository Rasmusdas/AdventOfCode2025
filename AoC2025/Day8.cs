using System.Diagnostics;
using BenchmarkDotNet.Attributes;

namespace AoC2025;

public class Day8 : Day
{
    [Benchmark]
    public void SolvePart1()
    {
        List<JunctionBox> boxes = new();

        foreach (var line in inputLines)
        {
            var numbers = line.Split(",");
            
            int x = int.Parse(numbers[0]);
            int y = int.Parse(numbers[1]);
            int z = int.Parse(numbers[2]);
            
            boxes.Add(new JunctionBox(x, y, z));
        }

        PriorityQueue<(JunctionBox, JunctionBox,long),long> boxPairs = new();

        int testCount = 0;
        int testInterval = 20000;
        long distance = long.MaxValue;
        
        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                var dist = boxes[j].Dist(boxes[i]);
                if (dist < distance)
                {
                    boxPairs.Enqueue((boxes[i], boxes[j],dist), dist);
                }
                
                // Every testInterval runs we extract the 1000 smallest distances to get a lower bound on how far away a distance can be.
                // This should reduce lead to much fewer insertions at the expense of heuristic calculations.
                // Benchmarking shows this to be 2-3x faster running it every 10000 intervals
                if((++testCount) % testInterval == 0)
                {
                    List<(JunctionBox, JunctionBox, long)> distanceTest = new(1024);
                    for (int k = 0; k < 1000; k++)
                    {
                        distanceTest.Add(boxPairs.Dequeue());
                    }

                    distance = Math.Min(distanceTest[^1].Item3, distance);

                    foreach (var v in distanceTest)
                    {
                        boxPairs.Enqueue(v,v.Item3);
                    }
                }
            }
        }
        
        Graph<JunctionBox> graph = new();
        for (int i = 0; i < 1000; i++)
        {
            var boxPair = boxPairs.Dequeue();
            graph.AddEdge(boxPair.Item1, boxPair.Item2);
        }
        
        var sizes = graph.GetComponentSizes();
        
        sizes.Sort((x,y) => y.CompareTo(x));
        
        int sum = 1;
        
        for (int i = 0; i < 3; i++)
        {
            sum*=sizes[i];
        }

#if DEBUG
        Console.WriteLine(sum);
#endif
    }
    
    [Benchmark]
    public void SolvePart2()
    {
        List<JunctionBox> boxes = new();

        foreach (var line in inputLines)
        {
            var numbers = line.Split(",");
            
            int x = int.Parse(numbers[0]);
            int y = int.Parse(numbers[1]);
            int z = int.Parse(numbers[2]);
            
            boxes.Add(new JunctionBox(x, y, z));
        }

        List<(JunctionBox, JunctionBox, double)> boxPairs = new();

        for (int i = 0; i < boxes.Count; i++)
        {
            for (int j = i + 1; j < boxes.Count; j++)
            {
                boxPairs.Add((boxes[i], boxes[j], boxes[j].Dist(boxes[i])));
            }
        }

        boxPairs.Sort((p1, p2) => p1.Item3.CompareTo(p2.Item3));
        Graph<JunctionBox> graph = new();
        for (int i = 0; i < 1000; i++)
        {
            graph.AddEdge(boxPairs[i].Item1, boxPairs[i].Item2);
        }

        int nextPair = 1000;
        
        while (boxes.Count != graph.GetComponentSize(boxPairs[0].Item1))
        {
            if (nextPair >= boxPairs.Count)
            {
                Console.WriteLine("Something went wrong");
                break;
            }
            
            graph.AddEdge(boxPairs[nextPair].Item1,boxPairs[nextPair].Item2);

            nextPair++;
        }

#if DEBUG
        Console.WriteLine(boxPairs[nextPair-1].Item1.X*boxPairs[nextPair-1].Item2.X);
#endif
    }
}

public class JunctionBox
{
    public readonly int X;
    public readonly int Y;
    public readonly int Z;

    public JunctionBox(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public long Dist(JunctionBox other)
    {
        return (long)(Math.Pow(other.X - X, 2) +
                      Math.Pow(other.Y - Y, 2) +
                      Math.Pow(other.Z - Z, 2));
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Z)}: {Z}";
    }
}

public class Graph<T>
{
    private List<List<int>> _adjancencyList = new();
    private List<T> _nodes = new();
    private Dictionary<T,int> _idLookup = new();
    private int _nextId = 0;

    public void AddNode(T node)
    {
        _adjancencyList.Add(new List<int>());
        _nodes.Add(node);
        _idLookup.Add(node, _nextId++);
    }

    public void AddEdge(T from, T to)
    {
        if (!_idLookup.TryGetValue(from,out _))
        {
            AddNode(from);
        }

        if (!_idLookup.TryGetValue(to, out _))
        {
            AddNode(to);
        }
        
        _adjancencyList[_idLookup[from]].Add(_idLookup[to]);
        _adjancencyList[_idLookup[to]].Add(_idLookup[from]);
    }

    public List<int> GetComponentSizes()
    {
        Dictionary<T,bool> visited = new();
        List<int> componentSizes = new();
        
        for (int i = 0; i < _nodes.Count; i++)
        {
            if (visited.ContainsKey(_nodes[i])) continue;
            visited[_nodes[i]] = true;
            
            var component = GetConnectedComponent(_nodes[i]);

            foreach (var v in component)
            {
                visited[v] = true;
            }
            
            componentSizes.Add(component.Count);
        }   
        
        return componentSizes;
    }

    public List<T> GetConnectedComponent(T node)
    {
        if (!_idLookup.TryGetValue(node,out _))
        {
            return new List<T>();
        }

        List<T> component = new(1024);

        var id = _idLookup[node];

        Queue<int> queue = new(1024);
        queue.Enqueue(id);
        var visited = new bool[_nodes.Count];
        
        while (queue.Count > 0)
        {
            var nextNode = queue.Dequeue();

            if (visited[nextNode]) continue;
            
            component.Add(_nodes[nextNode]);
            visited[nextNode] = true;

            foreach (var adjacent in _adjancencyList[nextNode])
            {
                if (visited[adjacent]) continue;
                queue.Enqueue(adjacent);
            }
        }

        return component;
    }
    
    public int GetComponentSize(T node)
    {
        if (!_idLookup.TryGetValue(node,out _))
        {
            return 0;
        }

        int count = 0;
        Queue<int> queue = new(1024);
        var visited = new bool[_nodes.Count];
        var id = _idLookup[node];
        queue.Enqueue(id);
        
        while (queue.Count > 0)
        {
            var nextNode = queue.Dequeue();

            if (visited[nextNode]) continue;
            visited[nextNode] = true;

            foreach (var adjacent in _adjancencyList[nextNode])
            {
                if (visited[adjacent]) continue;
                queue.Enqueue(adjacent);
            }

            count++;
        }

        return count;
    }

    public int GetNodeCount => _nodes.Count;
}