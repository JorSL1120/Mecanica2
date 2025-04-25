using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BezierLink : MonoBehaviour
{
    public int linkIndex;
    public List<Transform> P = new List<Transform>();
    private int curvePoints = 50;


    void FixedUpdate()
    {
        SampleCurve();
    }

    public void InitCurve()
    {
        int linkControlPoints = BezierChain.controlPoints[0].Count;
        
        for (int i = 0; i < linkControlPoints; i++)
        {
            P.Add(BezierChain.controlPoints[linkIndex][i]);
        }

        GetComponent<LineRenderer>().positionCount = curvePoints;
        GetComponent<LineRenderer>().widthMultiplier = 0.25f;
        Color randomColor = new Color(Random.value, Random.value, Random.value);
        GetComponent<LineRenderer>().startColor = randomColor;
        GetComponent<LineRenderer>().endColor = randomColor;
    }

    public void SampleCurve()
    {
        for (int i = 0; i < curvePoints; i++)
        {
            float s = (float)i / (float)(curvePoints - 1);
            GetComponent<LineRenderer>().SetPosition(i, BezierFunctions.Bezier(s, P));
        }
    }

}
