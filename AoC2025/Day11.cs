
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AoC2025
{
    public class Day11 : Day
    {
        public void SolvePart1()
        {
            Graph<string> graph = new Graph<string>();

            foreach (var line in inputLines)
            {
                var nodes = line.Split(" ");

                var mainNode = nodes[0].Remove(nodes[0].Length - 1);

                for (int i = 1; i < nodes.Length; i++)
                {
                    graph.AddDirectedEdge(mainNode, nodes[i]);
                }
            }

            Stack<string> nodeQueue = new Stack<string>();

            string startNode = "you";
            string endNode = "out";

            int count = 0;

            nodeQueue.Push(startNode);


            while (nodeQueue.Count > 0)
            {
                var nextNode = nodeQueue.Pop();

                if (nextNode == endNode)
                {
                    count++;
                    continue;
                }

                foreach (var neighbourNode in graph.GetConnections(nextNode))
                {
                    nodeQueue.Push(neighbourNode);
                }
            }
            Console.WriteLine(count);


        }

        public void SolvePart2()
        {
            Graph<string> graph = new Graph<string>();

            Dictionary<string, Memory<string>> connectionLookup = new();

            foreach (var line in inputLines)
            {
                var nodes = line.Split(" ");

                var mainNode = nodes[0].Remove(nodes[0].Length - 1);

                for (int i = 1; i < nodes.Length; i++)
                {
                    graph.AddDirectedEdge(mainNode, nodes[i]);
                }

                connectionLookup[mainNode] = nodes.AsMemory(1);
            }

            connectionLookup["out"] = new Memory<string>(new string[0]);
            var validFFTNodes = GetValidNodes(graph,"fft");

            Graph<string> fftGraph = new Graph<string>();

            foreach (var node in validFFTNodes)
            {
                foreach(var connection in connectionLookup[node].Span)
                {
                    fftGraph.AddDirectedEdge(node, connection);
                }
            }

            var fftPaths = GetPathCount(fftGraph,"svr","fft");

            Console.WriteLine(fftPaths);

            var validDACNodes = GetValidNodes(graph, "dac");

            Graph<string> dacGraph = new Graph<string>();

            foreach (var node in validDACNodes)
            {
                foreach (var connection in connectionLookup[node].Span)
                {
                    dacGraph.AddDirectedEdge(node, connection);
                }
            }

            var dacPaths = GetPathCount(dacGraph, "fft", "dac");
            
            Console.WriteLine(dacPaths);

            var validOutNodes = GetValidNodes(graph, "out");
            
            Graph<string> outGraph = new Graph<string>();

            foreach (var node in validOutNodes)
            {
                foreach (var connection in connectionLookup[node].Span)
                {
                    outGraph.AddDirectedEdge(node, connection);
                }
            }

            var outPaths = GetPathCount(outGraph, "dac", "out");

            Console.WriteLine(outPaths);


            Console.WriteLine(fftPaths*dacPaths*outPaths);




        }

        public List<string> GetValidNodes(Graph<string> graph, string endNode)
        {
            Stack<string> nodeQueue = new Stack<string>();
            bool[] visited = new bool[graph.NodeCount];

            List<string> allowedNodes = new List<string>();

            for (int i = 0; i < graph.NodeCount; i++)
            {
                string startNode = graph.Nodes[i];

                nodeQueue = new Stack<string>();

                nodeQueue.Push(startNode);

                visited = new bool[graph.NodeCount];

                while (nodeQueue.Count > 0)
                {
                    var nextNode = nodeQueue.Pop();

                    visited[graph.GetNodeId(nextNode)] = true;

                    if (nextNode == endNode)
                    {
                        allowedNodes.Add(startNode);
                        break;
                    }

                    foreach (var neighbourNode in graph.GetConnections(nextNode))
                    {
                        if (visited[graph.GetNodeId(neighbourNode)]) continue;
                        nodeQueue.Push(neighbourNode);
                    }
                }
            }

            return allowedNodes;
        }

        public long GetPathCount(Graph<string> graph, string startNode, string endNode)
        {
            Stack<string> nodeQueue = new Stack<string>();

            long count = 0;

            nodeQueue = new Stack<string>();

            nodeQueue.Push(startNode);

            while (nodeQueue.Count > 0)
            {
                var nextNode = nodeQueue.Pop();

                if (nextNode == endNode)
                {
                    count++;
                }

                foreach (var neighbourNode in graph.GetConnections(nextNode))
                {
                    nodeQueue.Push(neighbourNode);
                }
            }

            return count;
        }


        public void SolvePart2A()
        {
            Graph<string> graph = new Graph<string>();

            foreach (var line in inputLines)
            {
                var nodes = line.Split(" ");

                var mainNode = nodes[0].Remove(nodes[0].Length - 1);

                for (int i = 1; i < nodes.Length; i++)
                {
                    graph.AddDirectedEdge(mainNode, nodes[i]);
                }
            }
            string startNode = "svr";
            string endNode = "fft";

            bool[] visited = new bool[graph.NodeCount];
            SearchState[] foundEnd = new SearchState[graph.NodeCount];

            Console.WriteLine(SearchForPaths(graph,startNode,endNode,visited, foundEnd, graph.GetNodeId("dac"),graph.GetNodeId("fft")));

        }

        public (SearchState, int) SearchForPaths(Graph<string> g, string currentNode, string endNode, bool[] visited, SearchState[] foundEnd, int dacNode, int fftNode, int depth = 0)
        {
            int result = 0;
            depth++;
            SearchState state = foundEnd[g.GetNodeId(currentNode)];
            if (foundEnd[g.GetNodeId(currentNode)] == SearchState.FoundEnd && visited[dacNode] && visited[fftNode]) { return (SearchState.FoundEnd, 1); }

            visited[g.GetNodeId(currentNode)] = true;

            if (endNode == currentNode)
            {
                if (visited[fftNode] && visited[dacNode])
                {
                    result = 1;
                }

                visited[g.GetNodeId(currentNode)] = false;
                foundEnd[g.GetNodeId(currentNode)] = SearchState.FoundEnd;
                return (SearchState.FoundEnd,result);
            }

            var connections = g.GetConnections(currentNode);

            foreach (var nextNode in connections)
            {

                if (foundEnd[g.GetNodeId(nextNode)] == SearchState.DeadEnd) continue;

                var searchResult = SearchForPaths(g, nextNode, endNode, visited, foundEnd, dacNode, fftNode,depth);

                result += searchResult.Item2;

                if(searchResult.Item1 == SearchState.DeadEnd)
                {
                    if(state != SearchState.FoundEnd) state = SearchState.DeadEnd;
                }
                else
                {
                    state = searchResult.Item1;
                }
            }

            if(connections.Count == 0)
            {
                state = SearchState.DeadEnd;
            }

            visited[g.GetNodeId(currentNode)] = false;
            foundEnd[g.GetNodeId(currentNode)] = state;
            return (state, result);
        }
    }
}

public enum SearchState
{
    None,
    FoundEnd,
    DeadEnd
}