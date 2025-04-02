using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V3_NewMazeGenerator : MonoBehaviour
{
    public Vector2Int mazeSize;
    public int maxCost;
    public GameObject wallPrefab;
    public GameObject player1Prefab;  // Prefab para el jugador
    public GameObject finishPrefab;  // Prefab para el final
    private RandomCostGraph graph;

    [System.NonSerialized] public MST mst;
    private GameObject player1;  // Jugador
    private GameObject mazeContainer;  // Contenedor del laberinto (Maze)

    public GameObject Techo;

    private void Start()
    {
        Techo.SetActive(false);
        StartCoroutine(TimepoTecho());

        CreateMaze();
        CreatePlayer();  // Ahora solo creamos un jugador
    }

    private void CreateMaze()
    {
        mazeContainer = transform.Find("Maze").gameObject;  // Obtener el objeto Maze donde se generará el laberinto

        CreateGraph();
        CreateMST();
        CreateMazeWalls();
        CreateMazeBorders();
        transform.Find("Maze").rotation = Quaternion.Euler(90, 0, 0);
        CenterPivotAfterCreation();  // Mover el pivote del laberinto al centro después de generarlo
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
        // Borde superior e inferior
        for (int i = 0; i < mazeSize.x; i++)
        {
            Vector2 posBottom = new Vector2(i, -0.5f);
            Vector2 posTop = new Vector2(i, mazeSize.y - 0.5f);
            GameObject bottom = Instantiate(wallPrefab, posBottom, Quaternion.identity, mazeContainer.transform);
            GameObject top = Instantiate(wallPrefab, posTop, Quaternion.identity, mazeContainer.transform);
            bottom.transform.localScale = new Vector3(1, 0.1f, 1);
            top.transform.localScale = new Vector3(1, 0.1f, 1);
        }

        // Borde izquierdo y derecho
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
        // El jugador debe comenzar en la esquina inferior izquierda del laberinto
        // Ya que el pivote del laberinto ahora está en el centro, debemos ajustar la posición.
        Vector2 start = new Vector2(-mazeSize.x / 2f, -mazeSize.y / 2f);  // Esquina inferior izquierda

        // El final debe estar en la esquina superior derecha del laberinto
        Vector2 end = new Vector2(mazeSize.x / 2f - 1, mazeSize.y / 2f - 1);  // Esquina superior derecha

        // Instancia el jugador en la posición calculada, ajustando la altura (en el eje Y)
        player1 = Instantiate(player1Prefab, new Vector3(start.x, 1, start.y), Quaternion.identity);
        player1.transform.SetParent(transform);  // Asignamos MazeGenerator como el padre

        // Instancia el final en la posición calculada
        GameObject finish = Instantiate(finishPrefab, new Vector3(end.x, 1, end.y), Quaternion.identity);
        finish.transform.SetParent(transform);  // Asignamos MazeGenerator como el padre

        /*
        // El jugador debe comenzar en la esquina inferior izquierda del laberinto
        // Ya que el pivote del laberinto ahora está en el centro, debemos ajustar la posición.
        Vector2 start = new Vector2(-mazeSize.x / 2f, -mazeSize.y / 2f);  // Esquina inferior izquierda

        // El final debe estar en la esquina superior derecha del laberinto
        Vector2 end = new Vector2(mazeSize.x / 2f - 1, mazeSize.y / 2f - 1);  // Esquina superior derecha

        // Instancia el jugador en la posición calculada, ajustando la altura (en el eje Y)
        player1 = Instantiate(player1Prefab, new Vector3(start.x, 1, start.y), Quaternion.identity);

        // Instancia el final en la posición calculada
        Instantiate(finishPrefab, new Vector3(end.x, 1, end.y), Quaternion.identity);
        */
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
        // Calcular el centro del laberinto
        Vector3 center = new Vector3(mazeSize.x / 2f, 0, mazeSize.y / 2f);

        // Mover solo el objeto "Maze" al centro (no afecta el contenido)
        mazeContainer.transform.position = -center;  // Moverlo en sentido opuesto al centro
    }

    IEnumerator TimepoTecho()
    {
        yield return new WaitForSeconds(1);
        Techo.SetActive(true);
    }
}
