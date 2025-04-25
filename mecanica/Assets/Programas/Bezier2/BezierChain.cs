
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

    }

    void Update()
    {
        UpdateSharedControlPoints();
        SavePositions();
    }

    void UpdateSharedControlPoints()
    {
        int n = controlPoints[0].Count - 1;
        for (int j = 0; j < numLinks - 1; j++)
        {
            // Condiciones C1
            if (controlPoints[j + 1][0].position != previousPositions[j + 1][0] || controlPoints[j + 1][1].position != previousPositions[j + 1][1])
            {
                controlPoints[j][n - 1].position = 2 * controlPoints[j + 1][0].position - controlPoints[j + 1][1].position;
            }

            if (controlPoints[j][n].position != previousPositions[j][n] || controlPoints[j][n - 1].position != previousPositions[j][n - 1])
            {
                controlPoints[j + 1][1].position = 2 * controlPoints[j][n].position - controlPoints[j][n - 1].position;
                
            }

            // Condiciones C2
            
            if (controlPoints[j + 1][0].position != previousPositions[j + 1][0] || controlPoints[j + 1][1].position != previousPositions[j + 1][1] || controlPoints[j + 1][2].position != previousPositions[j + 1][2])
            {
                controlPoints[j][n - 2].position = 4 * (controlPoints[j + 1][0].position - controlPoints[j + 1][1].position) + controlPoints[j + 1][2].position;
            }

            if (controlPoints[j][n].position != previousPositions[j][n] || controlPoints[j][n - 1].position != previousPositions[j][n - 1] || controlPoints[j][n - 2].position != previousPositions[j][n - 2])
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

}
