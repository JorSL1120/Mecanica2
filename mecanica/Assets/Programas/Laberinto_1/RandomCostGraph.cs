using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RandomCostGraph
{
    public Vector2Int size;
    public int maxCost;
    public List<Vector2> nodeList = new List<Vector2>();
    public Dictionary<Vector4, int> connectionCosts = new Dictionary<Vector4, int>();

    public RandomCostGraph(Vector2Int size, int maxCost)
    {
        this.size = size;
        this.maxCost = maxCost;
    }
   
    public void CreateGraph()
    {
        // Crea los nodos
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector2 newNode = new Vector2(i, j);
                nodeList.Add(newNode);
            }
        }


        // Define los costos en el grafo
        // Se definen las conexiones en ambas direcciones

       foreach(Vector2 node in nodeList)
       {
            List<Vector2> neighbours = Neighbours(node);

            foreach(Vector2 neigh in neighbours)
            {
                Vector4 connection = CreateConnection(node, neigh);
                Vector4 connectionRev = CreateConnection(neigh, node);

                bool cond1 = connectionCosts.ContainsKey(connection);
                bool cond2 = connectionCosts.ContainsKey(connectionRev);

                if (!cond1 && !cond2)
                {
                    int cost = Random.Range(1, maxCost);
                    connectionCosts.Add(connection, cost);
                    connectionCosts.Add(connectionRev, cost);
                }
            }
       }

    }

    // Devuelve una lista con los vecinos de node
    public List<Vector2> Neighbours(Vector2 node)
    {
        List<Vector2> directions = new List<Vector2> {Vector2.right,
                                                      Vector2.up,
                                                      Vector2.left,
                                                      Vector2.down };

        List<Vector2> result = new List<Vector2>();

        foreach (Vector2 direction in directions)                    
        {
            Vector2 neighbourSuspect = node + direction;
            if (nodeList.Contains(neighbourSuspect))
                result.Add(neighbourSuspect);
        }

        return result;
    }

    public List<Vector2> GraphWithout(Vector2 node)
    {
        List<Vector2> result = new List<Vector2>();
        foreach (Vector2 other in nodeList)
        {
            if (other != node)
                result.Add(other);
        }
        return result;
    }

    public Vector4 CreateConnection(Vector2 A, Vector2 B)
    {
        return new Vector4(A.x, A.y, B.x, B.y);
    }

    public Vector2 GetNodeA(Vector4 connection)
    {
        return new Vector2(connection.x, connection.y);
    }

    public Vector2 GetNodeB(Vector4 connection)
    {
        return new Vector2(connection.z, connection.w);
    }

}
