using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierVehicle : MonoBehaviour
{
    public BezierChain bezierChain;
    public float time;
    public float travelTime, slerp;
    public bool isMoving = false;
    public static Vector3 normalizedVelocity;

    // Update is called once per frame
    void Update()
    {
        if(isMoving && time < bezierChain.numLinks)
        {
            transform.position = PiecewiseBezier(time);
            Vector3 tangent = Tangent(time);
            Vector3 normal = Normal(time);
            Vector3 binormal = Vector3.Cross(tangent, normal);
            Quaternion rotation = Quaternion.LookRotation(tangent, binormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, slerp*Time.deltaTime);
            normalizedVelocity = tangent;
        }

        if (time == bezierChain.numLinks)
        {
            isMoving = false;
        }

        if (isMoving)
        {
            time += Time.deltaTime/travelTime;
            time = Mathf.Clamp(time, 0, bezierChain.numLinks);
        }
       
    }

    Vector3 PiecewiseBezier(float t)
    {
        float linkIndex = Mathf.Floor(t);
        return BezierFunctions.Bezier(t - linkIndex, BezierChain.controlPoints[(int)linkIndex]);
    }

    Vector3 PiecewiseBezierDerivative(float t)
    {
        float linkIndex = Mathf.Floor(t);
        return BezierFunctions.BezierDerivative(t - linkIndex, BezierChain.controlPoints[(int)linkIndex]);
    }

    Vector3 PiecewiseBezierSecondDerivative(float t)
    {
        float linkIndex = Mathf.Floor(t);
        return BezierFunctions.BezierSecondDerivative(t - linkIndex, BezierChain.controlPoints[(int)linkIndex]);
    }

    Vector3 Tangent(float t)
    {
        return PiecewiseBezierDerivative(t).normalized;
    }

    Vector3 Normal(float t)
    {
        Vector3 derivative = PiecewiseBezierDerivative(t);
        Vector3 secondDerivative = PiecewiseBezierSecondDerivative(t);

        Vector3 crossDerivatives = Vector3.Cross(derivative, secondDerivative);
        float normCrossDerivatives = crossDerivatives.magnitude;
        float normDerivative = derivative.magnitude;

        return Vector3.Cross(crossDerivatives, derivative) / (normCrossDerivatives * normDerivative);
    }

}
