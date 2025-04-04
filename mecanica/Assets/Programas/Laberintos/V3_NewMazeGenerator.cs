using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_NewMazeGenerator : MonoBehaviour
{
    public Vector2Int mazeSize;
    public int maxCost;
    public GameObject wallPrefab;
    public GameObject player1Prefab;
    public GameObject finishPrefab;
    private RandomCostGraph graph;

    [System.NonSerialized] public MST mst;
    private GameObject player1;
    private GameObject mazeContainer;

    public GameObject Techo;

    private void Start()
    {
        Techo.SetActive(false);
        StartCoroutine(TimepoTecho());

        CreateMaze();
        CreatePlayer();
    }

    private void CreateMaze()
    {
        mazeContainer = transform.Find("Maze").gameObject;

        CreateGraph();
        CreateMST();
        CreateMazeWalls();
        CreateMazeBorders();
        transform.Find("Maze").rotation = Quaternion.Euler(90, 0, 0);
        CenterPivotAfterCreation();
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
    }

    void CreateMazeBorders()
    {
        for (int i = 0; i < mazeSize.x; i++)
        {
            Vector2 posBottom = new Vector2(i, -0.5f);
            Vector2 posTop = new Vector2(i, mazeSize.y - 0.5f);
            GameObject bottom = Instantiate(wallPrefab, posBottom, Quaternion.identity, mazeContainer.transform);
            GameObject top = Instantiate(wallPrefab, posTop, Quaternion.identity, mazeContainer.transform);
            bottom.transform.localScale = new Vector3(1, 0.1f, 1);
            top.transform.localScale = new Vector3(1, 0.1f, 1);
        }

        for (int j = 0; j < mazeSize.y; j++)
        {
            Vector2 posLeft = new Vector2(-0.5f, j);
            Vector2 posRight = new Vector2(mazeSize.x - 0.5f, j);

            GameObject left = Instantiate(wallPrefab, posLeft, Quaternion.identity, mazeContainer.transform);
            GameObject right = Instantiate(wallPrefab, posRight, Quaternion.identity, mazeContainer.transform);

            left.transform.localScale = new Vector3(0.1f, 1, 1);
            right.transform.localScale = new Vector3(0.1f, 1, 1);
        }
    }

    void CreateMazeWalls()
    {
        List<Vector4> singleConnections = new List<Vector4>();
        foreach (Vector4 connection in graph.connectionCosts.Keys)
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
                GameObject wall = Instantiate(wallPrefab, wallPos, Quaternion.identity, mazeContainer.transform);
                Vector3 scaleVector = new Vector3(Mathf.Abs(nodeA.x - nodeB.x), Mathf.Abs(nodeA.y - nodeB.y), 0);
                wall.transform.localScale = Vector3.one - 0.9f * scaleVector;
            }
        }
    }

    void CreatePlayer()
    {
        Vector2 start = new Vector2(-mazeSize.x / 2f, -mazeSize.y / 2f);
        Vector2 end = new Vector2(mazeSize.x / 2f - 1, mazeSize.y / 2f - 1);
        player1 = Instantiate(player1Prefab, new Vector3(start.x, 1, start.y), Quaternion.identity);
        player1.transform.SetParent(transform);
        GameObject finish = Instantiate(finishPrefab, new Vector3(end.x, 1, end.y), Quaternion.identity);
        finish.transform.SetParent(transform);
    }

    void ClearMaze()
    {
        foreach (Transform child in mazeContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void CenterPivotAfterCreation()
    {
        Vector3 center = new Vector3(mazeSize.x / 2f, 0, mazeSize.y / 2f);
        mazeContainer.transform.position = -center;
    }

    IEnumerator TimepoTecho()
    {
        yield return new WaitForSeconds(1);
        Techo.SetActive(true);
    }
}
