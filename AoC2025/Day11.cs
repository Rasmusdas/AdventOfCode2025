
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
#if DEBUG
            Console.WriteLine(count);
#endif


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
            var validFFTNodes = GetValidNodes(graph, "fft");

            Graph<string> fftGraph = new Graph<string>();

            foreach (var node in validFFTNodes)
            {
                foreach (var connection in connectionLookup[node].Span)
                {
                    fftGraph.AddDirectedEdge(node, connection);
                }
            }

            var fftPaths = GetPathCount(fftGraph, "svr", "fft");

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


#if DEBUG
            Console.WriteLine(fftPaths * dacPaths * outPaths);
#endif



        }

        public List<string> GetValidNodes(Graph<string> graph, string endNode)
        {
            List<string> allowedNodes = new List<string>();

            for (int i = 0; i < graph.NodeCount; i++)
            {
                string startNode = graph.Nodes[i];

                Stack<string>  nodeQueue = new Stack<string>();

                nodeQueue.Push(startNode);

                bool[] visited = new bool[graph.NodeCount];

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
    }
}