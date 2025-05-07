
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierChain : MonoBehaviour
{
    public int numLinks, linkControlPoints;
    public static List<List<Transform>> controlPoints = new List<List<Transform>>();
    public GameObject controlPointPrefab;
    public GameObject bezierLinkPrefab;

    private List<List<Vector3>> previousPositions = new List<List<Vector3>>();
    

    void Awake()
    {
        CreateControlPoints();
    }

    private void Start()
    {
        for (int j = 0; j < numLinks; j++)
        {
            GameObject bezierLink = Instantiate(bezierLinkPrefab, transform);
            bezierLink.GetComponent<BezierLink>().linkIndex = j;
            bezierLink.GetComponent<BezierLink>().InitCurve();
        }

        SavePositions();
        DontDestroyOnLoad(gameObject);

    }

    void Update()
    {
        UpdateSharedControlPoints();
        SavePositions();
    }

    void UpdateSharedControlPoints()
    {
        // Si los puntos de control están vacíos o han sido destruidos, recrearlos
        if (controlPoints == null || controlPoints.Count == 0 || controlPoints[0] == null || controlPoints[0].Count == 0)
        {
            CreateControlPoints();
            return;
        }

        int n = controlPoints[0].Count - 1;
        for (int j = 0; j < numLinks - 1; j++)
        {
            if (controlPoints[j] == null || controlPoints[j + 1] == null)
                continue;

            // Condiciones C1
            if (controlPoints[j + 1][0] != null && controlPoints[j + 1][1] != null &&
                (controlPoints[j + 1][0].position != previousPositions[j + 1][0] || controlPoints[j + 1][1].position != previousPositions[j + 1][1]))
            {
                controlPoints[j][n - 1].position = 2 * controlPoints[j + 1][0].position - controlPoints[j + 1][1].position;
            }

            if (controlPoints[j][n] != null && controlPoints[j][n - 1] != null &&
                (controlPoints[j][n].position != previousPositions[j][n] || controlPoints[j][n - 1].position != previousPositions[j][n - 1]))
            {
                controlPoints[j + 1][1].position = 2 * controlPoints[j][n].position - controlPoints[j][n - 1].position;
            }

            // Condiciones C2
            if (controlPoints[j + 1][2] != null && controlPoints[j][n - 2] != null &&
                (controlPoints[j + 1][0].position != previousPositions[j + 1][0] || controlPoints[j + 1][1].position != previousPositions[j + 1][1] || controlPoints[j + 1][2].position != previousPositions[j + 1][2]))
            {
                controlPoints[j][n - 2].position = 4 * (controlPoints[j + 1][0].position - controlPoints[j + 1][1].position) + controlPoints[j + 1][2].position;
            }

            if (controlPoints[j][n - 2] != null &&
                (controlPoints[j][n].position != previousPositions[j][n] || controlPoints[j][n - 1].position != previousPositions[j][n - 1] || controlPoints[j][n - 2].position != previousPositions[j][n - 2]))
            {
                controlPoints[j + 1][2].position = 4 * (controlPoints[j][n].position - controlPoints[j][n - 1].position) + controlPoints[j][n - 2].position;
            }
        }
    }

    void SavePositions()
    {
        previousPositions.Clear();
        for (int j = 0; j < numLinks; j++)
        {
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < linkControlPoints; i++)
            {
                int childIndex = j * (linkControlPoints - 1) + i;
                points.Add(transform.Find("ControlPoints").GetChild(childIndex).position);
            }
            previousPositions.Add(points);
        }
    }

    void CreateControlPoints()
    {
        int totalPoints = numLinks * (linkControlPoints - 1) + 1;

        //  Define tus posiciones manualmente aquí
        Vector3[] manualPositions = new Vector3[]
        {
            // Link 0
            new Vector3(0, 0, 0),
            new Vector3(1, 2, 3),
            new Vector3(5, 5, 6),
            new Vector3(4, -3, 10),
            new Vector3(-4, 2, 15),
            new Vector3(0, 6, 18),
            new Vector3(6, 10, 20),
            new Vector3(10, -2, 25),
            new Vector3(0, 0, 27),
            new Vector3(-3, -3, 34),  // <- Link 0 termina aquí

            // Link 1
            new Vector3(0, 3, 38),
            new Vector3(5, 0, 43),
            new Vector3(12, 2, 49),
            new Vector3(15, 6, 53),
            new Vector3(10, 8, 59),
            new Vector3(6, 12, 68),
            new Vector3(3, 9, 72),
            new Vector3(-1, 6, 77),
            new Vector3(-5, 3, 83),
            new Vector3(-8, -2, 88),  // <- Link 1 termina aquí

            // Link 2
            new Vector3(-12, -6, 95),
            new Vector3(-14, -10, 100),
            new Vector3(-9, -12, 105),
            new Vector3(-4, -8, 110),
            new Vector3(0, -4, 115),
            new Vector3(4, -2, 120),
            new Vector3(8, 0, 125),
            new Vector3(10, 3, 130),
            new Vector3(12, 5, 135),
            new Vector3(15, 8, 140),  // <- Link 2 termina aquí

            // Link 3
            new Vector3(18, 11, 145),
            new Vector3(20, 14, 150),
            new Vector3(17, 12, 155),
            new Vector3(14, 8, 160),
            new Vector3(10, 6, 165),
            new Vector3(5, 3, 170),
            new Vector3(0, 0, 175),
            new Vector3(-5, -2, 180),
            new Vector3(-10, -4, 185),
            new Vector3(-15, -6, 190), // <- Link 3 termina aquí

            // Link 4
            new Vector3(-18, -9, 195),
            new Vector3(-20, -12, 200),
            new Vector3(-17, -14, 205),
            new Vector3(-14, -16, 210),
            new Vector3(-10, -18, 215),
            new Vector3(-5, -20, 220), // <- Link 4 termina aquí
        };

        if (manualPositions.Length != totalPoints)
        {
            Debug.LogError("Cantidad de posiciones manuales no coincide con la cantidad total esperada.");
            return;
        }

        Transform cpParent = transform.Find("ControlPoints");

        for (int k = 0; k < totalPoints; k++)
        {
            GameObject cp = Instantiate(controlPointPrefab, manualPositions[k], Quaternion.identity, cpParent);
        }

        // Agrupar en tramos
        for (int j = 0; j < numLinks; j++)
        {
            List<Transform> points = new List<Transform>();
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            for (int i = 0; i < linkControlPoints; i++)
            {
                int index = j * (linkControlPoints - 1) + i;
                Transform cp = cpParent.GetChild(index);
                points.Add(cp);
                cp.GetComponent<MeshRenderer>().material.color = randomColor;
            }
            controlPoints.Add(points);
        }
    }


    /*
    void CreateControlPoints()
    {
        int controlPointsCount = numLinks * (linkControlPoints - 1) + 1;
        for (int k = 0; k < controlPointsCount; k++)
        {
            Vector3 pos = new Vector3(0, 0, 2*k);
            Quaternion rot = Quaternion.identity;
            GameObject controlPoint = Instantiate(controlPointPrefab, pos, rot, transform.Find("ControlPoints"));
        }

        for (int j = 0; j < numLinks; j++)
        {
            List<Transform> points = new List<Transform>();
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            for (int i = 0; i < linkControlPoints; i++)
            {
                int childIndex = j * (linkControlPoints - 1) + i;
                points.Add(transform.Find("ControlPoints").GetChild(childIndex));
                transform.Find("ControlPoints").GetChild(childIndex).GetComponent<MeshRenderer>().material.color = randomColor;
            }
            controlPoints.Add(points);
        }
    }
    */

}
