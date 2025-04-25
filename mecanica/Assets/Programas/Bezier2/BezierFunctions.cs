using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierFunctions
{
    // Bezier Functions

    private static int Factorial(int n)
    {
        int result = 1;
        for (int k = 1; k <= n; k++)
        {
            result *= k;
        }
        return result;
    }

    private static int Binomial(int n, int i)
    {
        int result = Factorial(n) / (Factorial(i) * Factorial(n - i));
        return result;
    }

    private static float PolBern(int n, int i, float s)
    {
        float result = Binomial(n, i) * Mathf.Pow(1f - s, n - i) * Mathf.Pow(s, i);
        return result;
    }

    public static Vector3 Bezier(float s, List<Transform> P)
    {
        int order = P.Count - 1;
        Vector3 result = Vector3.zero;
        for (int i = 0; i <= order; i++)
        {
            result += PolBern(order, i, s) * P[i].position;
        }
        return result;
    }

    public static Vector3 BezierDerivative(float s, List<Transform> P)
    {
        int order = P.Count - 1;
        Vector3 result = Vector3.zero;
        for (int i = 0; i <= order - 1; i++)
        {
            result += PolBern(order - 1, i, s) * (P[i + 1].position - P[i].position);
        }
        return order * result;
    }

    public static Vector3 BezierSecondDerivative(float s, List<Transform> P)
    {
        int order = P.Count - 1;
        Vector3 result = Vector3.zero;
        for (int i = 0; i <= order - 2; i++)
        {
            result += PolBern(order - 2, i, s) * (P[i + 2].position - 2* P[i+1].position + P[i].position);
        }
        return order * (order - 1) * result;
    }
}
