using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMazeGenerator : MonoBehaviour
{
    public Vector2Int mazeSize;
    public int maxCost;
    public GameObject wallPrefab;

    private RandomCostGraph graph;

    [System.NonSerialized] public MST mst;

    private void Start()
    {
        CreateMaze();
    }
    private void CreateMaze()
    {
        CreateGraph();
        CreateMST();
        CreateMazeWalls();
        CreateMazeBorders();
        //transform.Find("Maze").rotation = Quaternion.Euler(90, 0, 0);
    }

    void CreateGraph()
    {
        graph = new RandomCostGraph(mazeSize, maxCost);
        graph.CreateGraph();
    }

    void CreateMST()
    {
        mst = new MST(graph);
        mst.InitAlgorithm();
        mst.MST_Algorithm();
        Debug.Log("Respira");
    }

    void CreateMazeBorders()
    {

        for (int i = 0; i < mazeSize.x; i++)
        {
            Vector2 posBottom = new Vector2(i, -0.5f);
            Vector2 posTop = new Vector2(i, mazeSize.y - 0.5f);
            GameObject bottom = Instantiate(wallPrefab, posBottom, Quaternion.identity, transform.Find("Maze"));
            GameObject top = Instantiate(wallPrefab, posTop, Quaternion.identity, transform.Find("Maze"));
            bottom.transform.localScale = new Vector3(1, 0.1f, 1);
            top.transform.localScale = new Vector3(1, 0.1f, 1);
        }

        for (int j = 0; j < mazeSize.y; j++)
        {
            Vector2 posLeft = new Vector2(-0.5f, j);
            Vector2 posRight = new Vector2(mazeSize.x - 0.5f, j);

            GameObject left = Instantiate(wallPrefab, posLeft, Quaternion.identity, transform.Find("Maze"));
            GameObject right = Instantiate(wallPrefab, posRight, Quaternion.identity, transform.Find("Maze"));

            left.transform.localScale = new Vector3(0.1f, 1, 1);
            right.transform.localScale = new Vector3(0.1f, 1, 1);

        }
    }

    void CreateMazeWalls()
    {
        List<Vector4> singleConnections = new List<Vector4>();
        foreach(Vector4 connection in graph.connectionCosts.Keys)
        {
            Vector2 nodeA = graph.GetNodeA(connection);
            Vector2 nodeB = graph.GetNodeB(connection);
            Vector4 connectionRev = graph.CreateConnection(nodeB, nodeA);
            if (!singleConnections.Contains(connection) && !singleConnections.Contains(connectionRev))
            {
                singleConnections.Add(connection);
            }
        }

        foreach (Vector4 connection in singleConnections)
        {
            Vector2 nodeA = graph.GetNodeA(connection);
            Vector2 nodeB = graph.GetNodeB(connection);
            Vector4 connectionRev = graph.CreateConnection(nodeB, nodeA);
            if (!mst.T.Contains(connection) && !mst.T.Contains(connectionRev))
            {
                Vector2 wallPos = 0.5f * (nodeA + nodeB);
                GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity, transform.Find("Maze"));
                Vector3 scaleVector = new Vector3(Mathf.Abs(nodeA.x - nodeB.x), Mathf.Abs(nodeA.y - nodeB.y), 0);
                wall.transform.localScale = Vector3.one - 0.9f * scaleVector;
            }
        }
        
    }

    void ClearMaze()
    {
        foreach (Transform child in transform.Find("Maze"))
        {
            Destroy(child.gameObject);
        }
    }

}
