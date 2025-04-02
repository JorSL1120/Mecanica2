using System.Collections.Generic;
using UnityEngine;
using DataStructures.PQ;
public class MST 
{
    public RandomCostGraph graph;
    private List<Vector2> S1 = new List<Vector2>();
    private List<Vector2> S2 = new List<Vector2>();
    public List<Vector4> T = new List<Vector4>();

    private PriorityQueue<int, Vector4> E = new PriorityQueue<int, Vector4>();
    public MST(RandomCostGraph graph)
    {
        this.graph = graph;
    }

    public void InitAlgorithm()
    {
        // Selecciona un nodo de manera aleatoria
        int randomIndex = Random.Range(0, graph.nodeList.Count);
        Vector2 startNode = graph.nodeList[randomIndex];

        // Inicializa S1
        S1.Add(startNode);

        // Inicializa S2
        S2 = graph.GraphWithout(startNode);

        // Inicializa E

        foreach (Vector2 neighbour in graph.Neighbours(startNode))
        {
            Vector4 connection = graph.CreateConnection(startNode, neighbour);

            int cost = graph.connectionCosts[connection];
            E.Enqueue(cost, connection);
        }
    }

    public void MST_Algorithm()
    {
        while (S2.Count > 0)
        {
            Vector4 cheaperConnection = E.Dequeue();

            //Vector2 alreadyConnected = new Vector2(cheaperConnection.x, cheaperConnection.y);
            Vector2 notConnected = new Vector2(cheaperConnection.z, cheaperConnection.w);

            if (!S1.Contains(notConnected))
            {
                S1.Add(notConnected);
                T.Add(cheaperConnection);
            }

            S2.Remove(notConnected);
            Vector2 justConnected = notConnected;

            List<Vector2> justConnectedNeighbours = graph.Neighbours(justConnected);
            foreach (Vector2 neigh in justConnectedNeighbours)
            {
                if (S2.Contains(neigh))
                {
                    Vector4 connectionToCheck = graph.CreateConnection(justConnected, neigh);
                    int cost = graph.connectionCosts[connectionToCheck];
                    E.Enqueue(cost, connectionToCheck);
                }
            }
        }
    }

    public List<Vector2> MSTNeighbours(Vector2 node)
    {
        List<Vector2> result = new List<Vector2>();
        List<Vector2> oldNeighbours = graph.Neighbours(node);

        foreach (Vector2 neigh in oldNeighbours)
        {
            Vector4 connection = graph.CreateConnection(node, neigh);
            Vector4 connectionRev = graph.CreateConnection(neigh, node);

            if (T.Contains(connection) || T.Contains(connectionRev))
                result.Add(neigh);
        }
        return result;
    }
}